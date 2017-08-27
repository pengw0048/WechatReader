using System;
using System.IO;
using System.Linq;
using WechatReader;
using ObjectDumper;

namespace ConsoleApp1
{
    class TestLocator : ILocator
    {
        public string Locate(string path)
        {
            return Path.Combine("C:\\test", path);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var reader = new Reader(new TestLocator());
            reader.GetWCDBFriends();
            var me = reader.GetUser();
            Console.WriteLine(me.DumpToString("me"));
            reader.GetMMFriends().OrderBy(a => Guid.NewGuid()).Take(10).ToList().ForEach(o => Console.WriteLine(o.DumpToString("mm_friend")));
            reader.GetWCDBFriends().OrderBy(a => Guid.NewGuid()).Take(10).ToList().ForEach(o => Console.WriteLine(o.DumpToString("wcdb_friend")));
            reader.Dispose();
            Console.WriteLine("OK");
            Console.ReadLine();
        }
    }
}
