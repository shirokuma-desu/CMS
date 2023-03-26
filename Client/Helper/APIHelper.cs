using Newtonsoft.Json;
using System.Text;

namespace Client.Helper
{
    public class APIHelper
    {
        public static async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data)
        {
            try
            {
                var httpClient = new HttpClient();
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TResponse>(responseContent);
                }

            } catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

            return default;
        }
    }
}
