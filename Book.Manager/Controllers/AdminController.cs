using Book.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Book.Manager.Controllers
{
    public class AdminController : ApiController
    {
        [HttpPost]
        public object Login()
        {
            var id = 1;
            return new
            {
                ID = id,
                Token = SecurityUtil.GetInstance().EncryptString(id.ToString())
            };
            throw new Exception("用户名密码错误");
        } 
    }
}
