using Dapper;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using KeyAttribute = Dapper.KeyAttribute;

namespace TestDemo.DapperDemo
{

    public class user
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string age { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Dapper
    {
        //主要引用包 Dapper
        private readonly string sqlconnection = "Data Source=RENFB;Initial Catalog=test;User Id=sa;Password=sa;";
        public Dapper()
        {
            
        }


        
        public SqlConnection OpenConnection()
        {
            //这里sqlconnection就是数据库连接字符串
            SqlConnection connection = new SqlConnection(sqlconnection);
            connection.Open();
            return connection;
        }

        #region Dapper——手写SQL语句操作数据库
        //新增
        public int InsertWithSql(user person)
        {
            using (var conn = this.OpenConnection())
            {
                string _sql = "INSERT INTO User(name,address,age)VALUES(@name,@address,@age)";
                return conn.Execute(_sql, person);
            }
        }
        //批量新增
        public int InsertWithSql(List<user> persons)
        {

            using (var conn = this.OpenConnection())
            {
                string _sql = "INSERT INTO User(name,address,age)VALUES(@name,@address,@age)";
                return conn.Execute(_sql, persons);
            }
        }

        //删除
        public int DeleteColumn()
        {
            user user2 = new user();
            user2.id = 15;
            using (var conn = this.OpenConnection())
            {
                const string query = "delete from User where id=@id";
                return conn.Execute(query, user2);
            }
        }

        //批量删除
        public int DeleteColumn(List<user> persons)
        {
            using (var conn = this.OpenConnection())
            {
                return conn.Execute("delete from User where id=@id", persons);
            }
        }

        //修改
        public int UpdateColumn()
        {
            user user3 = new user();
            user3.id = 14;
            user3.name = "Dapper03";
            user3.address = "太康";
            user3.age = "25";
            using (var conn = this.OpenConnection())
            {
                const string query = "update User set name=@name,address=@address,age=@age where id=@id";
                return conn.Execute(query, user3);
            }

        }

        //查找(获取单个user对象)
        public user SelectColumn(int user_id)
        {
            user user4 = new user();
            user4.id = 14;
            using (var conn = this.OpenConnection())
            {
                const string query = "select * from User where id=@id";
                return conn.Query<user>(query, new { id = user_id }).SingleOrDefault<user>();
            }
        }
        //查找(获取user对象集合)
        public IEnumerable<user> SelectUser()
        {
            using (var conn = this.OpenConnection())
            {
                const string query = "select * from User order by id asc";
                return conn.Query<user>(query, null);
            }

        }
        #endregion

        #region Dapper——针对实体对象操作数据库(采用Dapper扩展包：Dapper.SimpleCRUD)
        ///<summary>
        ///实体插入数据
        ///</summary>
        public int? InsertWithEntity2()
        {
            using (var conn = this.OpenConnection())
            {
                var user = new user { name = "Dapper02", address = "周口", age = "22" };
                return conn.Insert(user);
            }
        }

        #endregion

        public class Column
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime ModifiedDate { get; set; }
            //public int Parentid { get; set; } 在数据库中此外键id关联到ColumnCat表id
            public ColumnCat ColumnCat { get; set; }
        }

        public class ColumnCat
        {
            public int Parentid { get; set; }
            public DateTime ModifiedOn { get; set; }
            public string Name { get; set; }
        }
        public class NewId
        {
            public int Id { get; set; }
        }

        # region Dapper也可以加载填充嵌套对象，考虑这样一种情形，考虑到新闻的类别属性，返回类别对象

        public IList<Column> SelectColumnsWithColumnCat()
        {
            using (var conn = this.OpenConnection())
            {
                const string query = "select c.Id,c.Name,c.ModifiedDate,c.ColumnCatid, cat.id,cat.[Name],cat.ModifiedOn,cat.Parentid " +
                                    "from[Column] as c" +
                                    "left outer join ColumnCat as cat on c.ColumnCatid = cat.id";
                return conn.Query<Column, ColumnCat, Column>(query,
                    (column, columncat) => { column.ColumnCat = columncat; return column; },
                    null, null, false, "Id", null, null).ToList<Column>();

            }
        }

        public int InsertColumnCat(ColumnCat cat)
        {
            using (var conn = this.OpenConnection())
            {
                const string query = "insert into ColumnCat([name],ModifiedOn,Parentid)" +
                                     "values(@name,@ModifiedOn,@Parentid)";
                int row = conn.Execute(query, cat);
                //更新对象的Id为数据库里新增的Id,假如增加之后不需要获得新增的对象，
                //只需将对象添加到数据库里，可以将下面的一行注释掉。
                SetIdentity(conn, id => cat.Parentid = id, "id", "ColumnCat");
                return row;

            }

        }

        private void SetIdentity(SqlConnection conn, Action<int> setId, string primarykey, string tableName)
        {
            if (string.IsNullOrEmpty(primarykey)) primarykey = "id";
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("tableName参数不能为空，为查询的表名");
            }
            string query = string.Format("SELECT max({0}) as Id FROM {1}", primarykey, tableName);
            NewId identity = conn.Query<NewId>(query, null).Single<NewId>();
            setId(identity.Id);
        }

        /* 由于Dapper是通过类的属性自动绑定的，所以增 加了NewId类来获取增加对象后的Id, 
         * 本来打算使用@@identity，Net3.5下使用总是报错，只好使用Max函数获取。
         * 当然如果不需要获得 更新后的对象的ID, 可以不使用SetIdentity, 这个函数通用。
         * 编译Dapper源码生成的是Net4.0下使用的，可以借助Net4.0新增的dynamic动态类型,
         */

        //SetIdentity的实现将非常方便。如下：
        public void SetIdentity<T>(SqlConnection conn, Action<T> setId)
        {
            dynamic identity = conn.Query("SELECT @@IDENTITY AS Id").Single();
            T newId = (T)identity.Id;
            setId(newId);
        }

        #endregion

        #region Dapper——复杂用法
        /// <summary>
        /// In操作
        /// </summary>
        public List<user> QueryIn()
        {
            using (var conn = this.OpenConnection())
            {
                var sql = "select * from Person where id in @ids";
                //参数类型是Array的时候，dappper会自动将其转化
                return conn.Query<user>(sql, new { ids = new int[2] { 1, 2 }, }).ToList();
            }
        }

        public List<user> QueryIn(int[] ids)
        {
            using (var conn = this.OpenConnection())
            {
                var sql = "select * from Person where id in @ids";
                //参数类型是Array的时候，dappper会自动将其转化
                return conn.Query<user>(sql, new { ids }).ToList();
            }
        }

        /// <summary>
        /// 多语句操作
        /// </summary>
        public void QueryMultiple()
        {
            using (var conn = this.OpenConnection())
            {
                var sql = "select * from User; select * from Column";
                var multiReader = conn.QueryMultiple(sql);
                var personList = multiReader.Read<user>();
                var bookList = multiReader.Read<Column>();
                multiReader.Dispose();
            }
        }

        //Join操作
        public Column QueryJoin(ColumnCat cat)
        {
            //Query的三个泛型参数分别是委托回调类型1，委托回调类型2，返回类型。
            //Query形参的三个参数分别是sql语句，map委托，对象参数。
            using (var conn = this.OpenConnection())
            {
                var sql = @"select b.Parentid,b.Name,p.id,p.Name
                        from Column as p
                        join ColumnCat as b
                        on p.id = b.Parentid
                        where b.id = @id;";
                var result = conn.Query<Column, ColumnCat, Column>(sql,
                (column, columnCat) =>
                {
                    column.ColumnCat = columnCat;
                    return column;
                },
                cat);
                //splitOn: "bookName");
                return (Column)result;
            }
        }

        //存储过程操作(假如数据库中已经建立名字为【sp_GetUsers】的存储过程)
        public void DapperProcedure()
        {
            using (var conn = this.OpenConnection())
            {
                //new { id = 5 }向存储过程中传递的参数
                var info = conn.Query<user>("sp_GetUsers", new { id = 5 }, commandType: CommandType.StoredProcedure);
            }

        }




        #endregion

        #region Dapper——的高级用法(包含事务处理)
        //Dapper对事务处理的例子,如删除类别的同时删除类别下的所有新闻。
        //或者删除产品的同时，删除产品图片表里关联的所有图片。
        public int DeleteColumnCatAndColumn(ColumnCat cat)
        {
            using (var conn = this.OpenConnection())
            {
                //【Column】表关联着【ColumnCat】表
                const string deleteColumn = "delete from [Column] where ColumnCatid=@catid";
                const string deleteColumnCat = "delete from ColumnCat where id=@Id";
                IDbTransaction transaction = conn.BeginTransaction();//事务处理
                try
                {
                    int row = conn.Execute(deleteColumn, new { catid = cat.Parentid }, transaction, null, null);
                    row += conn.Execute(deleteColumnCat, new { id = cat.Parentid }, transaction, null, null);
                    transaction.Commit();
                    return row;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return 0;
                }


            }
        }


        #endregion
    }
}
