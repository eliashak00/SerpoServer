using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Nancy.Owin;

namespace SerpoServer.NetCore
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            BuildWebHost(args).Run();

            Console.WriteLine("Server running!");
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return new WebHostBuilder().UseStartup<Program>().UseKestrel().Build();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseOwin(x => x.UseNancy());
        }
    }
}