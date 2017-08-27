namespace WechatReader
{
    // When `Reader` needs to find a file, it provides its relative path in iOS filesystem
    // and `ILocator.Locate` should return its actual path on disk.
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
