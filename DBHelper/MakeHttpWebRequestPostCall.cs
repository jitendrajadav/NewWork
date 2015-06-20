using ICICIMerchant.Common;
using ICICIMerchant.Helper;
using ICICIMerchant.Model;
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
        public static async Task<string> Login(string data,string url)
        {
            string responseResutl = string.Empty;
            try
            {
                var webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = "POST";
                
                webRequest.ContentType = "application/x-www-form-urlencoded";
                //webRequest.ContentType = "application/json; charset=utf-8";
                //var Serialized = SerializeDeserialize.Serialize(model);
                //string finalSerialized = model.ToString();//Serialized;
                using (StreamWriter sw = new StreamWriter(await webRequest.GetRequestStreamAsync()))
                {
                    sw.Write(data);
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
                return responseResutl;
            }
        }

        /// <summary>
        /// Post method for request and responce for every method 
        /// </summary>
        /// <param name="model">Class objcet</param>
        /// <param name="methodName">method name for e.g essayFeedbackRequest</param>
        /// <returns></returns>
        public static async Task<string> Generic_With_Token(string data, string url)
        {
            string responseResutl = string.Empty;
            try
            {
                var webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = "POST";
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.Headers["Authorization"] = "bearer " + ((LoginModel)SuspensionManager.SessionState["loginModel"]).access_token;
                //webRequest.ContentType = "application/json; charset=utf-8";
                //var Serialized = SerializeDeserialize.Serialize(model);
                //string finalSerialized = model.ToString();//Serialized;
                using (StreamWriter sw = new StreamWriter(await webRequest.GetRequestStreamAsync()))
                {
                    sw.Write(data);
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
                return responseResutl;
            }
        }

        public async static Task<string> Login(string data, string url,string key)
        {
            string rawServerResponse = null;
            WebRequest webRequest = WebRequest.Create(DBHandler.url + DBHandler.login_url_paddup);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";

            string body = data;
            byte[] writeBuffer = Encoding.UTF8.GetBytes(body);
            using (Stream stream = await webRequest.GetRequestStreamAsync())
            {
                stream.Write(writeBuffer, 0, writeBuffer.Length);
            }

            // response
            try
            {
                var respAsyncResult = await webRequest.GetResponseAsync();
                using (WebResponse response = await webRequest.GetResponseAsync())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            rawServerResponse = reader.ReadToEnd(); // read response data
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return rawServerResponse;
        }
    }
}
