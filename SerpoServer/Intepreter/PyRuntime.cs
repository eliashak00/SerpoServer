using System.Runtime.CompilerServices;
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace SerpoServer.Intepreter
{
    public class PyRuntime
    {
        private readonly ScriptEngine _mEngine = Python.CreateEngine();
        private readonly ScriptScope _mScope;

        public PyRuntime()
        {
            _mScope = _mEngine.CreateScope();
            
        }

        public CompiledCode Compile(string code)
        {
            var source = _mEngine.CreateScriptSourceFromString(code, SourceCodeKind.AutoDetect);
          
            return source.Compile();

        }
        public T Execute<T>(string code)
        {
            var source = _mEngine.CreateScriptSourceFromString(code, SourceCodeKind.AutoDetect);
            
            return source.Execute<T>(_mScope); 
        }
    }
}