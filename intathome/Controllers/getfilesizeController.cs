using Codeplex.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace intathome.Controllers
{
    public class getfilesizeController : ApiController
    {
        [HttpGet]
        public string Get()
        {
            var obj = lightController.connectSQL("select * from voice", 4);
            var js = DynamicJson.Parse(obj);
            double v = (double)js.size;
            return v.ToString();
        }
    }
}
