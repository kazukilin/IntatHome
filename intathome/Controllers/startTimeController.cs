using Codeplex.Data;
using System.Diagnostics;
using System.Net.Http;
using System.Web.Http;

namespace intathome.Controllers
{
    public class startTimeController : ApiController
    {
        [HttpGet]
        public string Get()
        {
            var obj = lightController.connectSQL("select * from lightsetting", 0);
            var json = DynamicJson.Parse(obj);
            var startTime = json.begin;
            return startTime;
        }
    }
}
