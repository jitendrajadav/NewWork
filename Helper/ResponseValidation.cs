using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICICIMerchant.Helper
{
    public class ValidationHelper
    {
        public static string Login(string tid, string tpin)
        {
            if (tid != string.Empty)
            {
                if (tpin != string.Empty)
                    return "";
                else
                    return "Please Enter TPIN";
            }
            else
                return "Plesae Enter TID";
        }
    }
}
