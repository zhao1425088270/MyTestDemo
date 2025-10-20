// See https://aka.ms/new-console-template for more information
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using TestDemo.OtherClass;

//Console.WriteLine("hello");
//Console.WriteLine(Environment.GetEnvironmentVariable("OSS_ACCESS_KEY_ID"));
//Console.WriteLine(Environment.GetEnvironmentVariable("OneDrive"));

#region IOrderedEnumerable 性质
//var index = 0;
//var aa = new List<string>{"Yellow", "Baa", "Qjfhja" }.Where(x => {

//    index++;
//    return x.StartsWith("Y");
//}).OrderBy(x=>x);
//Console.WriteLine(index);
//Console.WriteLine(aa.Count());
//Console.WriteLine(index);
#endregion

#region OtherClass 内容方法执行

//var types = (
//        from x in Assembly.GetExecutingAssembly().GetTypes()
//        where
//            x.Namespace?.StartsWith(
//                "TestDemo.OtherClass"
//            ) == true
//        select x
//    ).ToList();

//foreach (var type in types)
//{
//    Console.WriteLine("***************************************");
//    // 类名获取
//    //if (type.Name == "Cat") { continue; }

//    // 类的属性获取
//    var properties = type.GetProperties();
//    foreach (PropertyInfo item in properties)
//    {
//        string name = item.Name;
//        Console.WriteLine($"OtherClass文件中的属性名称：{name}");
//        object? instanceOne = Activator.CreateInstance(type);//类似实例化对象 例如 new Dog()
//        if (instanceOne == null) continue;
//        object value = item.GetValue(instanceOne);
//        Console.WriteLine($"OtherClass文件中的属性值：{value}");
//    }

//    // 类的方法获取与执行
//    var instance = Activator.CreateInstance(type);
//    var method = type.GetMethod("Anction");
//    if (method != null)
//    {
//        var result = method.Invoke(instance, null);
//        Console.WriteLine($"OtherClass文件中的类（{type.Name}）的Anction方法执行结果是：{result}");
//    }
//}
#endregion

#region 字符串结果
string strTemp = "a1 某某某";//每个中文字符占 2个字节。 a,1,空格各占1个字节
//.GetBytes(strTemp) - 将字符串转换为字节数组
int a = System.Text.Encoding.Default.GetBytes(strTemp).Length;
int b = strTemp.Length;
Console.WriteLine($"a的值：{a}");
Console.WriteLine($"a的值：{b}");
#endregion