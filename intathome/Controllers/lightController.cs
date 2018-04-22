using Codeplex.Data;
using System.Web.Http;
using System.Data.SqlClient;
using System.Net.Http;

namespace intathome.Controllers
{
    public class lightController : ApiController
    {
        static string begins, ends, ontimes,ipadd;
        static bool responses,functions,mains,seconds;

        [HttpGet]
        public string Get()
        {
            var obj = connectSQL("select * from lightsetting",0);
            var json = DynamicJson.Serialize(obj);
            return json;
        }
        [HttpPost]
        public string Post(HttpRequestMessage request)
        {
            var value = request.Content.ReadAsStringAsync().Result;
            try
            {
                var json = DynamicJson.Parse(value);
                begins = json.begin;
                ends = json.end;
                functions = json.function;
                ontimes = json.ontime;
            }
            catch
            {
                return "invalid syntax";
            }
            connectSQL("delete from lightsetting;", 2);
            string sqlstr = "insert into lightsetting values ('" + begins + "','" + ends + "','" + functions + "','" + ontimes + "');";
            connectSQL(sqlstr, 2);
            return "success";
        }

        public static dynamic connectSQL(string query, int id)
        {
            var json = "";
            string adr = personal.Address();
            string ident = personal.Identification();
            string pass = personal.Password();
            string constr = @"Data Source=" + adr + ";Initial Catalog=intathome;Connect Timeout=10;User ID=" + ident + ";Password=" + pass;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlCommand com = new SqlCommand(query, con);
                SqlDataReader sdr = com.ExecuteReader();
                if (id == 0)
                {
                    while (sdr.Read() == true)
                    {
                        begins = (string)sdr["begin"];
                        ends = (string)sdr["end"];
                        functions = (bool)sdr["function"];
                        ontimes = (string)sdr["ontime"];
                    }
                    var obj = new
                    {
                        begin = begins,
                        end = ends,
                        function = functions,
                        ontime = ontimes,
                    };
                    return json = DynamicJson.Serialize(obj);
                }
                else if (id == 1)
                {
                    while (sdr.Read() == true)
                    {
                        responses = (bool)sdr["response"];
                    }
                    var obj = new
                    {
                        response = responses,
                    };
                    return json = DynamicJson.Serialize(obj);
                }
                else if (id == 2)
                {
                    while (sdr.Read() == true)
                    {
                        mains = (bool)sdr["main"];
                        seconds = (bool)sdr["second"];
                    }
                    var obj = new
                    {
                        main = mains,
                        second = seconds,
                    };
                    return json = DynamicJson.Serialize(obj);
                }
                else if (id == 3)
                {
                    while (sdr.Read() == true)
                    {
                        ipadd = (string)sdr["ipaddress"];
                    }
                    return ipadd;
                }
            }
            return null;
        }
    }
}