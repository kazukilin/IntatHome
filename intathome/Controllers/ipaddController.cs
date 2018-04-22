using Codeplex.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace intathome.Controllers
{
    public class ipaddController : ApiController
    {
        [HttpGet]
        public string Get()
        {
            var obj = lightController.connectSQL("select * from ipaddress", 3);
            return obj;
        }
        [HttpPost]
        public string Post(HttpRequestMessage request)
        {
            var req = request.Content.ReadAsStringAsync().Result;
            lightController.connectSQL("delete from ipaddress;", 2);
            lightController.connectSQL("insert into ipaddress values ('" + req + "');", 2);
            return req;
        }
    }
}
