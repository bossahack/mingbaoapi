using Book.Dal.Model;
using Book.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Book.Api.Controllers
{
    public class DictController : ApiController
    {
        public string Get(string flag)
        {
            return DictService.GetInstance().Get(flag)?.Value;
        }
        public List<Dict> GetList(string flags)
        {
            return DictService.GetInstance().GetList(flags);
        }

        public void test(int id)
        {
            OrderService.GetInstance().sendUdp(id);
        }
    }
}
