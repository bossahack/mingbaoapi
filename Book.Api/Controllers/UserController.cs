using Book.Model;
using Book.Service;
using System.Web.Http;

namespace Book.Api.Controllers
{
    public class UserController : ApiController
    {

        [HttpPost]
        public object UpdateWXInfo(WechatLoginInfo wcLoginInfo)
        {
            return UserService.GetInstance().UpdateWXInfo(wcLoginInfo);
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

        public void JoinUs(JoinUsModel model)
        {
            UserService.GetInstance().JoinUs(model);
        }
        
    }
}
