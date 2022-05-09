using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Collections;
using System.Text.Json;

namespace WebAPIClient
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            await ProcessRepositories();
        }

        private static async Task ProcessRepositories()
        {

            //request0
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("*/*"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
            //client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", "admin"),
                new KeyValuePair<string, string>("client_secret", "admin"),
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("scope", "krc-genk")
            });
            var response = client.PostAsync("http://localhost:5002/connect/token",
                content);
            var body = response.Result.Content.ReadAsStringAsync().Result;
            var repository = JsonSerializer.Deserialize<Repository>(body);

            var msg1 = await response;
            Console.WriteLine("START PRINT");
            Console.WriteLine(repository.access_token);
            Console.WriteLine("END PRINT");

            //request1
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("*/*"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + repository.access_token);

            var stringTask = client.GetStringAsync("http://localhost:5000/api/seatholders");

            var msg = await stringTask;
            Console.Write(msg);
        }

        //public static async Task<TResult> PostFormUrlEncoded<TResult>(string url, IEnumerable<KeyValuePair<string, string>> postData)
        //{
        //    using (var httpClient = new HttpClient())
        //    {
        //        using (var content = new FormUrlEncodedContent(postData))
        //        {
        //            content.Headers.Clear();
        //            content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

        //            HttpResponseMessage response = await httpClient.PostAsync(url, content);

        //            return await response.Content.ReadAsAsync<TResult>();
        //        }
        //    }
        //}

    }
}
