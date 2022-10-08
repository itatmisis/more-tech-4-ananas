using System.Collections.Generic;
using System.Net;
using MORE_Tech.Parser.Configuration;


namespace MORE_Tech.Parser.ParserImplementations.VKParse
{
    internal static class Requests
    {
        private static HttpClient _httpClient;

        static Requests()
        {
            _httpClient = new HttpClient();
        }

        public static async Task<string> Send(Dictionary<string, string> keys, VKSettings config)
        {

          //  var data = new Leaf.xNet.HttpRequest { UserAgent = Leaf.xNet.Http.OperaUserAgent(), KeepAlive = true }; 
            string parameters="";
            foreach (KeyValuePair<string, string> entry in keys)
            {
                parameters+=($"&{entry.Key}={entry.Value}");
            }
            var message = new HttpRequestMessage()
            {
                RequestUri = new Uri($"{config.ApiUrl}{config.ApiMethod}?{parameters}&access_token={config.ApiKey}&v={config.ApiVersion}"),
            };

            HttpResponseMessage response = await _httpClient.SendAsync(message);

            if(response.StatusCode == HttpStatusCode.OK)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return string.Empty;
        }
    }
}
