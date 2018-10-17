using Nancy;
using Nancy.Extensions;
using Nancy.Responses;
using SerpoServer.Data.Models;

namespace SerpoServer
{
    public static class SiteSelector
    {
        public static spo_site GetSite(this NancyContext ctx)
        {
            if (ctx.Items.TryGetValue("site", out var site)) return (spo_site) site;

            return null;
        }

        public static void RequiresSite(this INancyModule mod)
        {
            mod.AddBeforeHookOrExecute(x => x.GetSite() != null ? null : new RedirectResponse("/admin/site"));
        }
    }
}