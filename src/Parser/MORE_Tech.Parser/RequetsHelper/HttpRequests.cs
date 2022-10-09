using System.Net;

namespace MORE_Tech.Parser.RequetsHelper
{
    public static class HttpRequests
    {
         private static HttpClient _client;

        static HttpRequests()
        {
            _client = new();
        }
        public async static Task<string> Send(Uri  url)
        {
            _client.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.29.2");
            HttpResponseMessage response = await _client.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }

            return string.Empty;
        }
    }
}
