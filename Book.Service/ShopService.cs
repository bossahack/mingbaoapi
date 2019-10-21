using Book.Dal;
using Book.Dal.Model;
using Book.Model;
using Book.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;

namespace Book.Service
{
    public class ShopService
    {
        private static ShopService _Instance;
        public static ShopService GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new ShopService();
            return _Instance;
        }


        public ShopResponse GetShopInfo()
        {
            var current = UserUtil.CurrentUser();
            var shop = ShopDal.GetInstance().Get(current.ShopId);
            if (shop == null)
                return null;
            return new ShopResponse()
            {
                Id = shop.Id,
                Address = shop.Address,
                Logo = shop.Logo,
                Name = shop.Name,
                Recommender = shop.Recommender,
                Status = shop.Status
            };
        }

        public List<ShopResponse> GetUserShops()
        {
            var current = UserUtil.CurrentUser();
            var userShopRelations = UserShopDal.GetInstance().GetByUser(current.Id);
            if (userShopRelations == null || userShopRelations.Count == 0)
                return null;

            var shops = ShopDal.GetInstance().GetList(userShopRelations.Select(c=>c.ShopId).ToList());

            var result = new List<ShopResponse>();
            foreach(var shop in shops)
            {
                result.Add(new ShopResponse() {
                    Id = shop.Id,
                    Address = shop.Address,
                    Logo = shop.Logo,
                    Name = shop.Name,
                    Recommender = shop.Recommender,
                    Status = shop.Status
                });
            }
            return result;
        }

        public void CollectShop(int shopid)
        {
            var current = UserUtil.CurrentUser();
            if (UserShopDal.GetInstance().Exist(current.Id, shopid))
                throw new Exception("您已经关注了此商家");

            var usershop = new UserShop()
            {
                UserId = current.Id,
                ShopId = shopid,
                CreateDate = DateTime.Now,
                Total = 0,
            };

            UserShopDal.GetInstance().Add(usershop);
        }

        public object GetQRCode()
        {
            var current = UserUtil.CurrentUser();
            var shop = ShopDal.GetInstance().Get(current.ShopId);
            if (shop == null)
                return null;

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
                CacheHelper.SetCache(cacheName, accessToken,TimeSpan.FromSeconds(response.expires_in));
            }
            string codeResult= HttpHelper.Post($"https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token={accessToken}", new Dictionary<string, string>()
            {
                ["scene"] = "id="+shop.Id,
                ["page"] = "page/home/home",
                ["width"] = "430",
            });
            return codeResult;
        }
    }

    public class WXTokenResponse
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public int errcode { get; set; }
        public string errmsg { get; set; }
    }
}
