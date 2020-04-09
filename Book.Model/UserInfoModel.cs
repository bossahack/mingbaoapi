using System;

namespace Book.Model
{
    public class UserInfoModel
    {

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Token { get; set; }


        public int ShopId { get; set; }

        public string WXName { get; set; }

        public int Type { get; set; }

        public string WxNum { get; set; }
    }

    ///// <summary>
    ///// 微信小程序登录信息结构
    ///// </summary>
    //public class WechatLoginInfo
    //{
    //    public string code { get; set; }
    //    public string encryptedData { get; set; }
    //    public string iv { get; set; }
    //    public string rawData { get; set; }
    //    public string signature { get; set; }
    //}
    ///// <summary>
    ///// 微信小程序用户信息结构
    ///// </summary>
    //public class WechatUserInfo
    //{
    //    public string openId { get; set; }
    //    public string nickName { get; set; }
    //    public string gender { get; set; }
    //    public string city { get; set; }
    //    public string province { get; set; }
    //    public string country { get; set; }
    //    public string avatarUrl { get; set; }
    //    public string unionId { get; set; }
    //    public Watermark watermark { get; set; }

    //    public class Watermark
    //    {
    //        public string appid { get; set; }
    //        public string timestamp { get; set; }
    //    }
    //}
    ///// <summary>
    ///// 微信小程序从服务端获取的OpenId和SessionKey信息结构
    ///// </summary>
    //public class OpenIdAndSessionKey
    //{
    //    public string openid { get; set; }
    //    public string session_key { get; set; }
    //    public string errcode { get; set; }
    //    public string errmsg { get; set; }
    //}

    public class JoinUsModel
    {
        public string WxNum { get; set; }
    }

    public class UserInfoRecommendModel
    {
        public string WXName { get; set; }
        public int Id { get; set; }
        public bool HasShop { get; set; }
    }

    /// <summary>
    /// 用户列表
    /// </summary>
    public class UserSearchModel
    {        
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        public string WXName { get; set; }

        public bool HasShop { get; set; }

        public int Type { get; set; }        

        public DateTime CreateDate { get; set; }
    }

    public class UserSearchParam: PageSearch
    {
        public DateTime CreateDateBegin { get; set; }
        public DateTime CreateDateEnd { get; set; }
        public bool? HasShop { get; set; }
        public bool? IsRecommender  { get; set; }

    }
}
