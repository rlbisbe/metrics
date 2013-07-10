using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Metrics.Services
{
    public interface IHttpService
    {
        Task<JsonObject> GetJsonResult(string url);
    }

    class HttpService : IHttpService
    {
        public async Task<JsonObject> GetJsonResult(string url)
        {
            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 1024 * 1024; // Read up to 1 MB of data
            var response = await client.GetAsync(new Uri(url));
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                throw new NullReferenceException();

            var result = await response.Content.ReadAsStringAsync();
            return JsonObject.Parse(result);
        }
    }
}
