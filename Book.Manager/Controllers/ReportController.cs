using Book.Manager.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Book.Manager.Controllers
{
    [AdminFilter]
    public class ReportController : ApiController
    {
    }
}
