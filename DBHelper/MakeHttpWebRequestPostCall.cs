using ICICIMerchant.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ICICIMerchant.DBHelper
{
    public static class MakeHttpWebRequestPostCall
    {
        /// <summary>
        /// Post method for request and responce for every method 
        /// </summary>
        /// <param name="model">Class objcet</param>
        /// <param name="methodName">method name for e.g essayFeedbackRequest</param>
        /// <returns></returns>
        public static async Task<string> Login(object model, string methodName)
        {
        callAgain:
            string responseResutl = string.Empty;
            try
            {
                var webRequest = (HttpWebRequest)WebRequest.Create(DBHandler.url + methodName);
                webRequest.Method = "POST";
                
                webRequest.ContentType = "application/x-www-form-urlencoded";
               // webRequest.ContentType = "application/json; charset=utf-8";
                var Serialized = SerializeDeserialize.Serialize(model);
                string finalSerialized = model.ToString();//Serialized;
                using (StreamWriter sw = new StreamWriter(await webRequest.GetRequestStreamAsync()))
                {
                    sw.Write(finalSerialized);
                }

                HttpWebResponse httpWebResponse = await webRequest.GetResponseAsync() as HttpWebResponse;
                using (StreamReader sr = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    if (httpWebResponse.StatusCode != System.Net.HttpStatusCode.Accepted)
                    {
                        string messageresult = String.Format("POST failed. Received HTTP {0}", httpWebResponse.StatusCode);
                    }
                    responseResutl = sr.ReadToEnd();
                }
                return responseResutl;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The request was aborted: Could not create SSL/TLS secure channel."))
                    goto callAgain;
                throw ex;
            }
        }

    }
}
