using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Book.Utils
{
    public class HttpHelper
    {
        //private static string Url => "http://www.baidu.com/";
        private static HttpClient httpClient;
        static HttpHelper()
        {
            //HttpClientHandler httpClientHandler = new HttpClientHandler();
            //httpClientHandler.UseCookies = true;
            httpClient = new HttpClient();
        }
        public static string Get(string url)
        {
            return httpClient.GetStringAsync(url).Result;
        }
        public static string Post(string url, Dictionary<string, string> dict)
        {
            FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(dict);
            return httpClient.PostAsync(url, formUrlEncodedContent).Result.Content.ReadAsStringAsync().Result;
        }
        public static string Post(string url, string txt)
        {
            StringContent stringContent = new StringContent(txt);
            return httpClient.PostAsync(url, stringContent).Result.Content.ReadAsStringAsync().Result;
        }
        
    }
}
