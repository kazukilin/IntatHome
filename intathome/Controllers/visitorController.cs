using Codeplex.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Diagnostics;

namespace intathome.Controllers
{
    public class visitorController : ApiController
    {

        public static List<byte[]> img = new List<byte[]>();
        public static List<string> dt = new List<string>();
        public static List<string> tm = new List<string>();
        public static List<int> sz = new List<int>();

        [HttpGet]
        public string Get()
        {
            var obj = lightController.connectSQL("select * from visitor", 5);
            var objc = new
            {
                date = dt,
                time = tm,
                image = img,
                size = sz
            };
            var objct = DynamicJson.Serialize(objc);
            return objct;
        }
        [HttpPost]
        public string Post(HttpRequestMessage request)
        {
            var req = request.Content.ReadAsStringAsync().Result;
            string base64 = req;
            byte[] bs = Convert.FromBase64String(base64);
            blobSQL(bs);
            return "ok";
        }
        static string dfmt = "yyyyMMdd";
        static string tfmt = "HHmmss";
        public static void blobSQL(byte[] file)
        {
            string adr = personal.Address();
            string ident = personal.Identification();
            string pass = personal.Password();
            string constr = @"Data Source=" + adr + ";Initial Catalog=intathome;Connect Timeout=10;User ID=" + ident + ";Password=" + pass;

            string query = "INSERT INTO visitor (date,time,[image],size) VALUES(@date,@time,@bin,@size) SET NOCOUNT ON;";
            using (SqlConnection con = new SqlConnection(constr))
            using (SqlCommand com = new SqlCommand(query, con))
            {
                con.Open();
                var datef = new SqlParameter("@date", System.Data.SqlDbType.Char,8)
                {
                    Value = new DateTimeOffset(DateTime.UtcNow).ToOffset(new TimeSpan(9, 0, 0)).ToString(dfmt)
                };
                var timef = new SqlParameter("@time", System.Data.SqlDbType.Char,6)
                {
                    Value = new DateTimeOffset(DateTime.UtcNow).ToOffset(new TimeSpan(9, 0, 0)).ToString(tfmt)
                };
                var vers = new SqlParameter("@bin", System.Data.SqlDbType.Binary, file.Length)
                {
                    Value = file
                };
                var sizef = new SqlParameter("@size", System.Data.SqlDbType.Int,file.Length)
                {
                    Value = file.Length
                };
                com.Parameters.Add(datef);
                com.Parameters.Add(timef);
                com.Parameters.Add(vers);
                com.Parameters.Add(sizef);
                com.Prepare();
                com.ExecuteNonQuery();
            }

        }
    }
}