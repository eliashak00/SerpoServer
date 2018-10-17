using System.Threading.Tasks;
using SerpoServer.Data.Models.Enums;

namespace SerpoServer.Http
{
    public interface IHttp
    {
        Task<object> Send(string reciever, object data, RequestMethods method);

        Task<object> Send(string reciever, string key, object data, RequestMethods method);

        Task<object> Recieve(string content, string key, RequestMethods method);
    }
}