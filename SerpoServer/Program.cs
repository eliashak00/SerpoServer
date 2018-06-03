using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Owin;
using Microsoft.Owin.Builder;
using Nancy.Owin;
using Owin;
using SerpoServer;
namespace SerpoServer
{
    class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
           
        }

        public static IWebHost BuildWebHost(string[] args) =>

            new WebHostBuilder().UseStartup<Program>().UseKestrel().Build();
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
            app.UseOwin(x => x.UseNancy());
            
   
        }
    }
}
