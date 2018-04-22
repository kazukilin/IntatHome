using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace intathome
{
    public class Base64Encoder
    {
        private static readonly Encoding encoding = Encoding.UTF8;
        public static string Encode(string s)
        {
            return Convert.ToBase64String(encoding.GetBytes(s));
        }
        public static string Decode(string s)
        {
            return encoding.GetString(Convert.FromBase64String(s));
        }
    }
}