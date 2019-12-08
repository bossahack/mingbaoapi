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

        public UserInfoModel ShopLogin(string username, string pwd)
        {
            var userInfo=userInfoDal.GetByName(username);
            if (userInfo == null)
                throw new Exception("用户名密码错误");
            if(userInfo.LoginPwd!=SecurityUtil.GetInstance().GetMD5String(pwd))
                throw new Exception("用户名密码错误");
            if(!userInfo.HasShop)
                throw new Exception("您未开通店铺，请进入小程序端开通");
            var shop = ShopDal.GetInstance().GetByUser(userInfo.Id);
            UserInfoModel result = new UserInfoModel()
            {
                Id = userInfo.Id,
                ShopId= shop.Id,               
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
            TransactionHelper.Run(()=> {
                userInfoDal.Update(userInfo);
                UserFeeService.GetInstance().Init(current.Id);
            });
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
            if (current.Id == userId)
                return;
            var userInfo = userInfoDal.Get(current.Id);
            if (userInfo.Recommender > 0)
                return;
            userInfo.Recommender = userId;
            userInfoDal.Update(userInfo);
        }

        public object GetLoginInfo()
        {
            var current = UserUtil.CurrentUser();
            var userInfo = userInfoDal.Get(current.Id);
            return new
            {
                Phone=userInfo.LoginName,
                Pwd=userInfo.LoginPwd
            };
        }

        public void UpdateLoginPwd(string pwd)
        {
            var current = UserUtil.CurrentUser();
            var userInfo = userInfoDal.Get(current.Id);
            userInfo.LoginPwd = SecurityUtil.GetInstance().GetMD5String(pwd);
            userInfoDal.Update(userInfo);
        }
    }
}
