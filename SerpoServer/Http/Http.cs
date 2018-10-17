using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SerpoServer.Data.Models.Enums;

namespace SerpoServer.Http
{
    public class Http : IHttp
    {
        public async Task<object> Send(string reciever, object data, RequestMethods method)
        {
            var jData = JsonConvert.SerializeObject(data);
            using (var http = new HttpClient())
            {
                HttpResponseMessage response;
                switch (method)
                {
                    case RequestMethods.Get:
                        response = await http.GetAsync(reciever, HttpCompletionOption.ResponseContentRead);
                        break;
                    case RequestMethods.Post:
                        response = await http.PostAsync(reciever, new StringContent(jData));
                        break;
                    case RequestMethods.Put:
                        response = await http.PutAsync(reciever, new StringContent(jData));
                        break;
                    case RequestMethods.Delete:
                        response = await http.DeleteAsync(reciever);
                        break;
                    default:
                        response = null;
                        break;
                }

                return response == null
                    ? null
                    : JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
            }
        }

        public Task<object> Send(string reciever, string key, object data, RequestMethods method)
        {
            throw new NotImplementedException();
        }

        public async Task<object> Recieve(string content, string key, RequestMethods method)
        {
            return content;
        }
    }
}