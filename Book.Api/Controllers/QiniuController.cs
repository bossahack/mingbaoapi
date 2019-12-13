using Book.Api.Filters;
using Book.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Book.Api.Controllers
{
    public class QiniuController : ApiController
    {
        [ShopFilter]
        public string GetSimpleKeyShop()
        {
            return QiniuService.GetInstance().GetSimpleKey();
        }

        public void RemoveUnuseImg()
        {
            QiniuService.GetInstance().RemoveUnuseImg();
        }
    }
}
