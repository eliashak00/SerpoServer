using Microsoft.Extensions.PlatformAbstractions;
using Nancy;

namespace SerpoServer
{
    public class RootPathProvider : IRootPathProvider
    {
        public string GetRootPath()
        {
            var p = "bin/Debug/netcoreapp2.1/";
            var r = PlatformServices.Default.Application.ApplicationBasePath.Replace(p, string.Empty);
            return r;
        }
    }
}