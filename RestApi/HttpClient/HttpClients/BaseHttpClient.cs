using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BasketballClient.HttpClients
{
    public abstract class BaseHttpClient
    {
        private static readonly JsonSerializerOptions _serializerSettings = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters =
            {
                new JsonStringEnumConverter(),
            },
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
        };

        private readonly string _baseRestApiUrl;
        private readonly HttpClient _client;

        public BaseHttpClient(string baseRestApiUrl)
        {
            _baseRestApiUrl = baseRestApiUrl;
            _client = new HttpClient();
            var requestHeaders = _client.DefaultRequestHeaders;

            if (requestHeaders.Accept == null || !requestHeaders.Accept.Any(m => m.MediaType == "application/json"))
            {
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        protected async Task<HttpResponseMessage> GetAsync(string route)
        {
            return await _client.GetAsync(_baseRestApiUrl + route);
        }

        protected async Task<HttpResponseMessage> PostAsync<T>(string route, T body)
        {
            var serializedBody = Serialize(body);
            var stringContent = JsonContent.Create(body, options: _serializerSettings);

            return await _client.PostAsync(_baseRestApiUrl + route, stringContent);
        }

        protected async Task<HttpResponseMessage> PutAsync<T>(string route, T body)
        {
            var serializedBody = Serialize(body);
            var stringContent = JsonContent.Create(body, options: _serializerSettings);

            return await _client.PutAsync(_baseRestApiUrl + route, stringContent);
        }

        protected async Task<HttpResponseMessage> DeleteAsync(string route)
        {
            return await _client.DeleteAsync(_baseRestApiUrl + route);
        }

        private static string Serialize(object obj)
        {
            return JsonSerializer.Serialize(obj, _serializerSettings);
        }

        protected static T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, _serializerSettings);
        }
    }
}