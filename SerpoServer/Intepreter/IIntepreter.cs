using System.IO;

namespace SerpoServer.Intepreter
{
    public interface IIntepreter
    {
        bool IsValid(string expr);
        MemoryStream Execute<T>(string expr);
    }
}