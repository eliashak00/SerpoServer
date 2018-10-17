using Nancy.ViewEngines.SuperSimpleViewEngine;

namespace SerpoServer.Lib
{
    public class ViewEngineConverter
    {
        public string Render(string content, dynamic model)
        {
            return new SuperSimpleViewEngine().Render(content, model, new StringViewEngineHost());
        }
    }
}