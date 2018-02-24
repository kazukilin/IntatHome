using Codeplex.Data;
using System.Net.Http;
using System.Web.Http;

namespace intathome.Controllers
{
    public class batteryController : ApiController
    {
        public string Get()
        {
            var obj = lightController.connectSQL("select * from battery", 2);
            var json = DynamicJson.Serialize(obj);
            return json;
        }
        public string Post(HttpRequestMessage request)
        {
            bool main, second;
            var req = request.Content.ReadAsStringAsync().Result;
            var json = DynamicJson.Parse(req);
            try
            {
                main = json.main;
                second = json.second;
            }
            catch
            {
                return "invalid syntax";
            }
            lightController.connectSQL("delete from battery;", 2);
            lightController.connectSQL("insert into battery values ('" + main + "','" + second + "');", 2);
            return "success";
        }
    }
}