using Nancy.ViewEngines.SuperSimpleViewEngine;

namespace SerpoServer.Lib
{
    public class StringViewEngineHost : IViewEngineHost
    {
        public string HtmlEncode(string input)
        {
            return input;
        }

        public string GetTemplate(string templateName, object model)
        {
            return templateName;
        }

        public string GetUriString(string name, params string[] parameters)
        {
            return string.Empty;
        }

        public string ExpandPath(string path)
        {
            return string.Empty;
        }

        public string AntiForgeryToken()
        {
            return string.Empty;
        }

        public object Context { get; }
    }
}