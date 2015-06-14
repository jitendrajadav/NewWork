using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ICICIMerchant.Helper
{
    public static class SerializeDeserialize
    {
        #region Serialize Deserialize

        /// <summary>
        /// Deserialize any string value
        /// </summary>
        /// <typeparam name="T">generic type</typeparam>
        /// <param name="json">string value.</param>
        /// <returns></returns>
        public static T Deserialize<T>(string json)
        {
            try
            {
                var _Bytes = Encoding.Unicode.GetBytes(json);
                using (MemoryStream _Stream = new MemoryStream(_Bytes))
                {
                    var _Serializer = new DataContractJsonSerializer(typeof(T));
                    _Stream.Position = 0;
                    return (T)_Serializer.ReadObject(_Stream);
                }
            }
            catch (System.Exception)
            {
                object a = null;
                return (T)a;
            }
        }

        /// <summary>
        /// Serialize any object.
        /// </summary>
        /// <param name="instance">object type</param>
        /// <returns></returns>
        public static string Serialize(object instance)
        {
            try
            {
                using (MemoryStream _Stream = new MemoryStream())
                {
                    var _Serializer = new DataContractJsonSerializer(instance.GetType());
                    _Serializer.WriteObject(_Stream, instance);
                    _Stream.Position = 0;
                    using (StreamReader _Reader = new StreamReader(_Stream))
                    { return _Reader.ReadToEnd(); }
                }
            }
            catch (System.Exception)
            {
                return string.Empty;
            }
        }

        #endregion

    }
}
