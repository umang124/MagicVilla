using MagicVilla_Utility;
using MagicVilla_VillaAPI;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace MagicVilla_Web.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse responseModel { get; set; }
        public IHttpClientFactory httpClient { get; set; }

        public BaseService(IHttpClientFactory httpClient)
        {
            this.responseModel = new();
            this.httpClient = httpClient;
        }
        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                using (var client = httpClient.CreateClient("MagicAPI"))
                {
                    HttpRequestMessage message = new HttpRequestMessage();
                    message.Headers.Add("Accept", "application/json");
                    message.RequestUri = new Uri(apiRequest.Url);

                    if (apiRequest.Data != null)
                    {
                        var jsonContent = JsonConvert.SerializeObject(apiRequest.Data);
                        message.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    }

                    switch (apiRequest.ApiType)
                    {
                        case ApiType.POST:
                            message.Method = HttpMethod.Post;
                            break;
                        case ApiType.PUT:
                            message.Method = HttpMethod.Put;
                            break;
                        case ApiType.DELETE:
                            message.Method = HttpMethod.Delete;
                            break;
                        default:
                            message.Method = HttpMethod.Get;
                            break;
                    }

                    HttpResponseMessage apiResponse = await client.SendAsync(message);
                    apiResponse.EnsureSuccessStatusCode();
                    var x = apiResponse.EnsureSuccessStatusCode();

                    var apiContent = await apiResponse.Content.ReadAsStringAsync();
                    var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);

                    return APIResponse;
                }
            }
            catch (Exception ex)
            {
                var dto = new APIResponse
                {
                    ErrorMessages = new List<string> { Convert.ToString(ex.Message) },
                    IsSuccess = false
                };
                var res = JsonConvert.SerializeObject(dto);
                var APIResponse = JsonConvert.DeserializeObject<T>(res);
                return APIResponse;
            }
        }

    }
}
