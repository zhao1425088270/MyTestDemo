// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

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

var types = (
        from x in Assembly.GetExecutingAssembly().GetTypes()
        where
            x.Namespace?.StartsWith(
                "TestDemo.OtherClass"
            ) == true
        select x
    ).ToList();

foreach (var type in types)
{
    if (type.Name == "Cat") { continue; }
    var instance = Activator.CreateInstance(type);
    var method = type.GetMethod("Anction");
    if (method != null)
    {
        var result = method.Invoke(instance, null);
        Console.WriteLine(result);
    }
}
#endregion