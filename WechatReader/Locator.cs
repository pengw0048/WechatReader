namespace WechatReader
{
    public interface ILocator
    {
        string Locate(string path);
    }

    public class IdenticalLocator : ILocator
    {
        public string Locate(string path)
        {
            return path;
        }
    }
}
