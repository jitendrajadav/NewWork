using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICICIMerchant.DBHelper
{
    public static class DBHandler
    {
        public static string url = "https://pg.icicims.com/imswebservices/";
        public static string login_url_paddup = "oauth/token";
        public static string paperroll_url_paddup = "fdindia/case/casecreation";
        public static string lastStatement_url_paddup = "fdindia/case/casecreation";
        public static string terminal_url_paddup = "fdindia/case/casecreation";
        public static string lastTicketHistory_url_paddup = "fdindia/case/history/caseHistory";
        public static string prevTicketHistory_url_paddup = "fdindia/case/casestatus";
        //public static string lastTicketHistory_url_paddup = "fdindia/case/history/caseHistory";
        //public static string prevTicketHistory_url_paddup = "fdindia/case/casestatus";
        //public static string lastTicketHistory_url_paddup = "fdindia/case/history/caseHistory";
        //public static string prevTicketHistory_url_paddup = "fdindia/case/casestatus";

        public static string key1 = "tejoratechonolog";
        public static string key2 = "tejorafdindiatec";
        public static string ivKey = "icicimerchatserv";

    }
}
