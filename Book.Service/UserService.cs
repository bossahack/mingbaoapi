using Book.Dal;
using Book.Dal.Model;
using Book.Model;
using Book.Utils;
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
            UserInfoModel result = null;
            if (dbUser == null)
            {
                UserInfo userCreate = new UserInfo()
                {
                    Wxid = oiask.openid,
                    CreateDate = DateTime.Now
                };
                var id = userInfoDal.Create(userCreate);
                result= new UserInfoModel()
                {
                    Id = id,
                };
            }else
            {
                result= new UserInfoModel()
                {
                    Id = dbUser.Id,
                    WXName =dbUser.WxName,
                    Type=dbUser.Type,
                    WxNum=dbUser.WxNum
                };
            }
            result.Token = SecurityUtil.GetInstance().EncryptString($"{result.Id}-{result.ShopId}");
            return result;
        }


        public UserInfoModel UpdateWXInfo(WechatUserInfo wechatUserInfo)
        {
            var current = UserUtil.CurrentUser();
            var dbUser = userInfoDal.Get(current.Id);           
            if (wechatUserInfo.avatarUrl != dbUser.WxPhoto || wechatUserInfo.nickName != dbUser.WxName)//更新头像
            {
                dbUser.WxPhoto = wechatUserInfo.avatarUrl;
                dbUser.WxName = wechatUserInfo.nickName;
                userInfoDal.Update(dbUser);
            }
            return new UserInfoModel()
            {
                Id = dbUser.Id,
                WXName= wechatUserInfo.nickName,
                Type=dbUser.Type
            };
        }

        public UserInfoModel ShopLogin()
        {

            UserInfoModel result = new UserInfoModel()
            {
                Id = 2,
                ShopId=2,
               
            };
            
            result.Token = SecurityUtil.GetInstance().EncryptString($"{result.Id}-{result.ShopId}");
            return result;
        }

        public void JoinUs(JoinUsModel model)
        {
            var current = UserUtil.CurrentUser();
            var userInfo = userInfoDal.Get(current.Id);
            userInfo.WxNum = model.WxNum;
            userInfo.Type = 1;
            userInfoDal.Update(userInfo);
        }
        
        public string GetQRCode()
        {
            var current = UserUtil.CurrentUser();
            string scene = "user" + current.Id;
            var bytes= WxService.GetInstance().GetQrCode(scene, 300);
            return Convert.ToBase64String(bytes);
        }

        public void Recommend(int userId)
        {
            var current = UserUtil.CurrentUser();
            var userInfo = userInfoDal.Get(current.Id);
            if (userInfo.Recommender > 0)
                return;
            userInfo.Recommender = userId;
            userInfoDal.Update(userInfo);
        }
    }
}
