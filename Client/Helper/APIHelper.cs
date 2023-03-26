using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Client.Helper
{
    public class APIHelper
    {
        public static async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest? data, string accessToken)
        {
            try
            {
                var httpClient = new HttpClient();
                if(accessToken != null)
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, content);

                if(response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<TResponse>(responseContent);
                }
            }
            catch(Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
            }

            await Console.Out.WriteLineAsync("RETURN DEFAULT");
            return default;
        }

        public static async Task<TResponse?> GetAsync<TResponse>(string url, string accessToken)
        {
            try
            {
                var httpClient = new HttpClient();
                if(accessToken != null)
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

                var response = await httpClient.GetAsync(url);

                if(response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<TResponse>(responseContent);
                }
            }
            catch(Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
            }

            await Console.Out.WriteLineAsync("RETURN DEFAULT");
            return default;
        }
    }
}
