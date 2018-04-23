using Codeplex.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace intathome.Controllers
{
    public class getresponseController : ApiController
    {
        [HttpGet]
        public string Get()
        {
            var obj = lightController.connectSQL("select * from autoresponse", 1);
            var js = DynamicJson.Parse(obj);
            bool ar = (bool)js.response;
            string ret = "";
            if (ar == true) ret = "true";
            else ret = "false";
            return ret;
        }
    }
}
