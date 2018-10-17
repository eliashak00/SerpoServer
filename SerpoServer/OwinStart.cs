using Owin;

namespace SerpoServer
{
    public class OwinStart
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy();
        }
    }
}