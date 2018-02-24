using Codeplex.Data;
using System.Diagnostics;
using System.Net.Http;
using System.Web.Http;

namespace intathome.Controllers
{
    public class responseController : ApiController
    {
        [HttpGet]
        public string Get()
        {
            var obj = lightController.connectSQL("select * from autoresponse", 1);
            var json = DynamicJson.Serialize(obj);
            return json;
        }
        [HttpPost]
        public string Post(HttpRequestMessage request)
        {
            bool response;
            var req = request.Content.ReadAsStringAsync().Result;
            Debug.WriteLine(req);
            var json = DynamicJson.Parse(req);
            try
            {
                response = json.response;
            }
            catch
            {
                return "invalid syntax";
            }
            lightController.connectSQL("delete from autoresponse;", 2);
            lightController.connectSQL("insert into autoresponse values ('" + response + "');", 2);
            return "success";
        }
    }
}