using System.Text.RegularExpressions;

namespace WechatSharedClasses
{
    // Represents a user.
    public class Person
    {
        public string UsrName;
        public string NickName;
        public string ConRemark;
        public string ConChatRoomMem;
        public string DbContactChatRoom;
        public string Portrait;
        public string PortraitHD;
        public string Alias;

        public void TryParseConStrRes2(string conStrRes2)
        {
            if (conStrRes2 == null) return;
            var match = Regex.Match(conStrRes2, @"<alias>(.*?)<\/alias>");
            if (match.Success) Alias = match.Groups[1].Value;
            match = Regex.Match(conStrRes2, @"<HeadImgUrl>(.+?)<\/HeadImgUrl>");
            if (match.Success) Portrait = match.Groups[1].Value;
            match = Regex.Match(conStrRes2, @"<HeadImgHDUrl>(.+?)<\/HeadImgHDUrl>");
            if (match.Success) PortraitHD = match.Groups[1].Value;
        }
    }
}
