using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICICIMerchant.DBHelper
{
    public static class DBHandler
    {
        public static string url = "https://pg.icicims.com/imswebservices";
        public static string login_url_paddup = "/oauth/token";
        public static string encryptiPass = "tejoratechonolog";
        public static string encryptiPassSec = "tejorafdindiatec";
        public static string ivKey = "icicimerchatserv";

        //var key1 = CryptoJS.enc.Utf8.parse('tejoratechonolog');
        //var ivKey = CryptoJS.enc.Utf8.parse('icicimerchatserv');
        //var key2 = CryptoJS.enc.Utf8.parse('tejorafdindiatec');
    }
}
