using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SMZDM.Common
{
    public class JsonTool
    {
        #region Singleton 
        static readonly JsonTool _Instance = new JsonTool();
        public static JsonTool Instance
        {
            get
            {
                return _Instance;
            }
        }

        #endregion
        /// <summary>
        /// 反序列化请求结果
        /// </summary>
        /// <typeparam name="T">继承自Result的实体</typeparam>
        /// <param name="requestContent">返回结果</param>
        /// <returns></returns>
        public T Deserialize<T>(string requestContent)
        {
            var jsonSerializer = new JsonSerializer();
            JsonReader reader = new JsonTextReader(new StringReader(requestContent));
            return jsonSerializer.Deserialize<T>(reader);
        }

        /// <summary>
        /// 序列化请求的body
        /// </summary>
        /// <typeparam name="T">请求的body的类型</typeparam>
        /// <param name="obj">请求的body</param>
        /// <returns>json序列化后的body字符串</returns>
        public string Serialize<T>(T obj)
        {
            var jsonSerializer = new JsonSerializer();
            var json = new StringBuilder();
            var write = new StringWriter(json);
            jsonSerializer.Serialize(write, obj);
            return json.ToString();
        }
        /// <summary>
        /// WebApi返回Json格式的数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public HttpResponseMessage toJson(Object obj)
        {
            String str;
            if (obj is String || obj is Char)
            {
                str = obj.ToString();
            }
            else
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                str = serializer.Serialize(obj);
            }
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }
    }
}
