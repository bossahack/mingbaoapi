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
        public object ShopLogin()
        {
            return UserService.GetInstance().ShopLogin();
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
    }
}
