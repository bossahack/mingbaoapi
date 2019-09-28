using Book.Dal;
using Book.Dal.Model;
using Book.Model;
using Newtonsoft.Json;
using System;
using System.Web.Configuration;

namespace Book.Service
{
    public class UserService
    {


        private static UserService _Instance;
        public static UserService GetInstance()
        {
            if (_Instance != null)
                return _Instance;
            _Instance = new UserService();
            return _Instance;
        }
        private static UserInfoDal userInfoDal = UserInfoDal.GetInstance();

        public UserInfoModel Login(WechatLoginInfo wcLoginInfo)
        {
            var dec = new WeChatAppDecrypt(WebConfigurationManager.AppSettings["wxID"], WebConfigurationManager.AppSettings["wxKey"]);
            OpenIdAndSessionKey oiask = JsonConvert.DeserializeObject<OpenIdAndSessionKey>(dec.GetOpenIdAndSessionKeyString(wcLoginInfo.code));
            if (oiask == null)
                throw new Exception("登陆失败");

            var dbUser = UserInfoDal.GetInstance().Get(oiask.openid);
            if (dbUser == null)
            {
                UserInfo userCreate = new UserInfo()
                {
                    Wxid = oiask.openid,
                    CreateDate = DateTime.Now
                };
                var id = userInfoDal.Create(userCreate);
                return new UserInfoModel()
                {
                    Id = id
                };
            }
            var shopid = 0;
            if (dbUser.HasShop)
            {
                var shop = ShopDal.GetInstance().GetByUser(dbUser.Id);
                shopid = shop.Id;
            }
            return new UserInfoModel()
            {
                Id = dbUser.Id,
                ShopId = shopid
            };
        }


        public UserInfoModel UpdateWXInfo(WechatLoginInfo wcLoginInfo)
        {
            var dec = new WeChatAppDecrypt(WebConfigurationManager.AppSettings["wxID"], WebConfigurationManager.AppSettings["wxKey"]);
            var result = dec.Decrypt(wcLoginInfo);
            if (result == null)
            {
                throw new Exception("登陆失败");
            }
            var dbUser = userInfoDal.Get(result.openId);
            if (dbUser == null)
            {
                UserInfo userCreate = new UserInfo()
                {
                    Wxid = result.openId,
                    WxName = result.nickName,
                    WxPhoto = result.avatarUrl,
                    //wx = result.unionId
                };
                var id = userInfoDal.Create(userCreate);
                return new UserInfoModel()
                {
                    Id = id
                };
            }
            else
            {
                if (result.avatarUrl != dbUser.WxPhoto || result.nickName != dbUser.WxName)//更新头像
                {
                    dbUser.WxPhoto = result.avatarUrl;
                    dbUser.WxName = result.nickName;
                    userInfoDal.Update(dbUser);
                }
                var shopid = 0;
                if (dbUser.HasShop)
                {
                    var shop = ShopDal.GetInstance().GetByUser(dbUser.Id);
                    shopid = shop.Id;
                }
                return new UserInfoModel()
                {
                    Id = dbUser.Id,
                    ShopId = shopid
                };
            }
        }
    }
}
