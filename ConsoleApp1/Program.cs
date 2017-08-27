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
            var friends = reader.GetFriendsDict();
            foreach (var session in reader.GetChatSessions())
            {
                if (friends.ContainsKey(session))
                {
                    var friend = friends[session];
                    Console.WriteLine(friend.UsrName + " " + friend.Alias + " " + friend.NickName + " " + friend.ConRemark);
                }
                else Console.WriteLine(session);
            }
            reader.Dispose();
            Console.WriteLine("OK");
            Console.ReadLine();
        }
    }
}
