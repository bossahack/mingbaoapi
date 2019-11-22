using Book.Model;
using Book.Service;
using System.Web.Http;

namespace Book.Api.Controllers
{
    public class UserController : ApiController
    {
        [Filters.UserFilter]
        [HttpPost]
        public object UpdateWXInfo([FromBody]WechatUserInfo wechatUserInfo)
        {
            return UserService.GetInstance().UpdateWXInfo(wechatUserInfo);
        }

        [HttpPost]
        public object Login(WechatLoginInfo wcLoginInfo)
        {
            return UserService.GetInstance().Login(wcLoginInfo);
        }

        [HttpPost]
        public object ShopLogin(string username,string pwd)
        {
            return UserService.GetInstance().ShopLogin(username,pwd);
        }

        [Filters.UserFilter]
        [HttpPost]
        public void JoinUs(JoinUsModel model)
        {
            UserService.GetInstance().JoinUs(model);
        }

        [Filters.UserFilter]
        [HttpGet]
        public string GetUserQrCode()
        {
           return UserService.GetInstance().GetQRCode();
        }

        [Filters.UserFilter]
        [HttpPost]
        public void Recommend(int userId)
        {
            UserService.GetInstance().Recommend(userId);
        }

        [Filters.UserFilter]
        [HttpGet]
        public object GetLoginInfo()
        {
            return UserService.GetInstance().GetLoginInfo();
        }

        [Filters.UserFilter]
        [HttpPost]
        public void UpdateLoginPwd(string pwd)
        {
            UserService.GetInstance().UpdateLoginPwd(pwd);
        }
    }
}
