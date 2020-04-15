using Book.Model;
using Book.Service;
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
        public object Login(AdminLoginParam para)
        {
            return AdminService.GetInstance().Login(para);
        } 
    }
}
