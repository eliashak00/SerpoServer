using System.IO;
using System.Text;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using SerpoServer.Database;

namespace SerpoServer.Intepreter
{
    public class Python : IIntepreter
    {
        private readonly ScriptEngine Engine;
        private readonly MemoryDatabase MemoryDatabase;

        private readonly SerpoLib SerpoLib;

        public Python(SerpoLib serpoLib, MemoryDatabase memoryDatabase)
        {
            Engine = IronPython.Hosting.Python.CreateEngine();
            SerpoLib = serpoLib;
            MemoryDatabase = memoryDatabase;
        }

        public bool IsValid(string expr)
        {
            var src = Engine.CreateScriptSourceFromString(expr, SourceCodeKind.AutoDetect);
            try
            {
                src.Compile();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public MemoryStream Execute<T>(string expr)
        {
            var s = new MemoryStream();

            var scope = Engine.CreateScope();
            scope.SetVariable("SPO", SerpoLib);
            Engine.Runtime.IO.SetOutput(s, Encoding.UTF8);
            Engine.Runtime.IO.SetErrorOutput(s, Encoding.UTF8);
            try
            {
                expr = @"import sys
                sys.stdout=resvar" + expr;
                var src = Engine.CreateScriptSourceFromString(expr, SourceCodeKind.AutoDetect);
                src.Execute(scope);

                return s;
            }
            catch
            {
                return null;
            }
        }
    }
}