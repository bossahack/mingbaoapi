using Book.Model;
using Book.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Book.Manager.Controllers
{
    public class UserController : ApiController
    {
        public Page<UserSearchModel> Search(UserSearchParam para)
        {
            return UserService.GetInstance().Search(para);
        }
    }
}