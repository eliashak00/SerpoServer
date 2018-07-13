using Nancy;

namespace SerpoServer
{
    public class RootPathProvider : IRootPathProvider
    {
        public string GetRootPath()
        {
            return new DefaultRootPathProvider().GetRootPath().Replace("/bin/Debug/netcoreapp2.0", "");
        }
    }
}