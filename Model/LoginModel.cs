﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ICICIMerchant.Model
{
    [DataContract]
    class LoginModel
    {
        [DataMember]    
        public string access_token { get; set; }
        [DataMember]
        public string token_type { get; set; }
        [DataMember]
        public int expires_in { get; set; }
        [DataMember]
        public string TID { get; set; }
        [DataMember]
        public string TPIN { get; set; }
    }
}
