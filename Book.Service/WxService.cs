using Book.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Book.Service
{
    public class WxService
    {
        private static WxService _Instance;
        public static WxService GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new WxService();
            return _Instance;
        }


        public byte[] GetQrCode(string scene,int size=430)
        {
            string accessToken;
            const string cacheName = "accessToken";
            var cache = CacheHelper.GetCache(cacheName);
            if (cache != null)
            {
                accessToken = CacheHelper.GetCache(cacheName).ToString();
            }
            else
            {
                string id = WebConfigurationManager.AppSettings["wxID"];
                string key = WebConfigurationManager.AppSettings["wxKey"];
                string url = $"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={id}&secret={key}";
                string result = HttpHelper.Get(url);
                var response = Newtonsoft.Json.JsonConvert.DeserializeObject<WXTokenResponse>(result);
                accessToken = response.access_token;
                CacheHelper.SetCache(cacheName, accessToken, TimeSpan.FromSeconds(response.expires_in));
            }
            string json = "{\"scene\":\"" + scene + "\",\"width\":"+size+",\"page\":\"pages/love\"}";
            //string json = "{'scene' = 'id="+shop.Id+"','width':430,'page'='page/home'}";
            var codeResult = HttpHelper.Post($"https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token={accessToken}", json);
            return codeResult;
        }


        public class WXTokenResponse
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
            public int errcode { get; set; }
            public string errmsg { get; set; }
        }
    }
}
