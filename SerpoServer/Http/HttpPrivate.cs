using System.Net.Http;
using System.Threading.Tasks;
using Jose;
using Newtonsoft.Json;
using SerpoServer.Data.Models.Enums;

namespace SerpoServer.Http
{
    public class HttpPrivate : IHttp
    {
        private string key;

        public async Task<object> Send(string reciever, object data, RequestMethods method)
        {
            var jData = JsonConvert.SerializeObject(data);
            var tData = JWT.Encode(data, key, JweAlgorithm.DIR, JweEncryption.A256CBC_HS512);
            using (var http = new HttpClient())
            {
                HttpResponseMessage response;
                switch (method)
                {
                    case RequestMethods.Get:
                        response = await http.GetAsync(reciever, HttpCompletionOption.ResponseContentRead);
                        break;
                    case RequestMethods.Post:
                        response = await http.PostAsync(reciever, new StringContent(tData));
                        break;
                    case RequestMethods.Put:
                        response = await http.PutAsync(reciever, new StringContent(tData));
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
            this.key = key;
            return Send(reciever, data, method);
        }

        public async Task<object> Recieve(string content, string key, RequestMethods method)
        {
            try
            {
                return JWT.Decode<object>(content, key, JweAlgorithm.DIR, JweEncryption.A256CBC_HS512);
            }
            catch
            {
                return null;
            }
        }
    }
}