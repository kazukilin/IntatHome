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
    public class blobController : ApiController
    {
        [HttpGet]
        public string Get()
        {
            var obj = lightController.connectSQL("select * from voice", 4);
            var json = DynamicJson.Serialize(obj);
            return json;
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

        public static void blobSQL(byte[] file)
        {
            string adr = personal.Address();
            string ident = personal.Identification();
            string pass = personal.Password();
            string constr = @"Data Source=" + adr + ";Initial Catalog=intathome;Connect Timeout=10;User ID=" + ident + ";Password=" + pass;
            int version = 0;

            string id = Console.ReadLine();
            //version値を読む
            using (SqlConnection con = new SqlConnection(constr))
            using (SqlCommand com = new SqlCommand("select version from voice SET NOCOUNT ON;", con))
            {
                con.Open();
                SqlDataReader sdr = com.ExecuteReader();
                while (sdr.Read() == true)
                {
                    version = (int)sdr["version"];
                }
            }

            using (SqlConnection con = new SqlConnection(constr))
            using (SqlCommand com = new SqlCommand("delete from voice SET NOCOUNT ON;", con))
            {
                con.Open();
                com.ExecuteNonQuery();
            }//挿入時は今までのを消す

            string query = "INSERT INTO voice ([file],version,filesize) VALUES(@bina,@version,@size) SET NOCOUNT ON;";
            using (SqlConnection con = new SqlConnection(constr))
            using (SqlCommand com = new SqlCommand(query, con))
            {
                con.Open();
                var param = new SqlParameter("@bina", System.Data.SqlDbType.Binary, file.Length)
                {
                    Value = file
                };
                var sizef = new SqlParameter("@size", System.Data.SqlDbType.Int, file.Length)
                {
                    Value = file.Length
                };
                var vers = new SqlParameter("@version", System.Data.SqlDbType.Int, version)
                {
                    Value = version + 1
                };
                com.Parameters.Add(sizef);
                com.Parameters.Add(param);
                com.Parameters.Add(vers);
                com.Prepare();
                com.ExecuteNonQuery();
            }

        }
    }
}
