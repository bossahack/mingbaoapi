﻿using Book.Dal;
using Book.Dal.Model;
using Book.Model;
using Book.Model.Enums;
using Book.Utils;
using Newtonsoft.Json;
using System;
using System.Linq;
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
                    WxNum=dbUser.WxNum,
                    WxPhone=dbUser.WxPhone,
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
                WXName = wechatUserInfo.nickName,
                Type = dbUser.Type,
                WxNum=dbUser.WxNum,
                WxPhone = dbUser.WxPhone
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
                throw new Exception("已有推荐人，不可变更");
            if ((DateTime.Now - userInfo.CreateDate).TotalDays > 7)
                throw new Exception("已注册超过7天，操作失败");
            if (userInfo.HasShop)
                throw new Exception("开通店铺前，才可设置推荐人，操作失败");

            userInfo.Recommender = userId;
            userInfo.RecommenderType = (int)RecommenderType.User;
            userInfoDal.Update(userInfo);
        }

        public void RecommendByShop(int shopId)
        {
            var current = UserUtil.CurrentUser();
            
            var userInfo = userInfoDal.Get(current.Id);
            if (userInfo.Recommender > 0)
                throw new Exception("已有推荐人，不可变更");
            if ((DateTime.Now - userInfo.CreateDate).TotalDays > 7)
                throw new Exception("已注册超过7天，操作失败");
            if (userInfo.HasShop)
                throw new Exception("开通店铺前，才可设置推荐人，操作失败");

            userInfo.Recommender = shopId;
            userInfo.RecommenderType = (int)RecommenderType.Shop;
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

        public Page<UserSearchModel> Search(UserSearchParam para)
        {
            var db = userInfoDal.Search(para);
            if (db.Total == 0)
                return new Page<UserSearchModel>() {
                    Total=0
                };
            return new Page<UserSearchModel>()
            {
                Total = db.Total,
                Items = db.Items.Select(c => new UserSearchModel()
                {
                    CreateDate=c.CreateDate,
                    HasShop=c.HasShop,
                    Id=c.Id,
                    Type=c.Type,
                    WXName=c.WxName
                }).ToList()
            };
        }

        public int GetRegisterNum()
        {
            return userInfoDal.GetRegisterNum(DateTime.Now);
        }

        public int GetRegisterNumByUser()
        {
            return userInfoDal.GetRegisterNum(DateTime.Now, (int)RecommenderType.User);
        }
        public int GetRegisterNumByShop()
        {
            return userInfoDal.GetRegisterNum(DateTime.Now, (int)RecommenderType.Shop);
        }

        public UserInfoModel UpdatePhone(string code)
        {
            var current = UserUtil.CurrentUser();
            var now = DateTime.Now;
            var record = PhoneCodeRecordDal.GetInstance().GetFirst(current.Id);
            if (record == null)
                throw new Exception("验证码已失效，请重新发送验证码");

            if((now- record.CreateTime).TotalMinutes>10)
                throw new Exception("验证码已失效，请重新发送验证码");

            if(record.Code.ToLower()!=code.ToLower())
                throw new Exception("验证码错误");

            var dbUser = userInfoDal.Get(current.Id);
            if(dbUser==null)
                throw new Exception("用户不存在");

            dbUser.WxPhone = record.Phone;
            userInfoDal.Update(dbUser);

            return new UserInfoModel()
            {
                Id = dbUser.Id,
                WXName = dbUser.WxName,
                Type = dbUser.Type,
                WxPhone = dbUser.WxPhone
            };
        }

        public void SendCode(string phone)
        {
            var current = UserUtil.CurrentUser();

            var now = DateTime.Now;
            var h24 = now.AddDays(-1);
            var recordCount = PhoneCodeRecordDal.GetInstance().GetTotalAfterTime(current.Id, h24);
            if (recordCount > 2)
                throw new Exception("一天内只可发送3条验证码");

            var d180 = now.AddDays(-180);
            recordCount= PhoneCodeRecordDal.GetInstance().GetTotalAfterTime(current.Id, h24);
            if (recordCount > 8)
                throw new Exception("半年内发送短信数过多");
            
            var code = GenerateCode();
            TransactionHelper.Run(()=> {

                PhoneCodeRecordDal.GetInstance().Create(new PhoneCodeRecord()
                {
                    Code = code,
                    Phone = phone,
                    CreateTime = now,
                    UserId = current.Id
                });
                new ShortMessageService().Send("+86" + phone, code);
            });

        }

        private string GenerateCode()
        {
            var num=new Random().Next(9999);
            return num.ToString().PadLeft(4);
        }
    }
}
