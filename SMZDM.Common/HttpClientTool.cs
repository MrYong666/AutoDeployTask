using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading;
using System.Net;

namespace SMZDM.Common
{
    public class HttpClientTool
    {
        private static HttpClient Client;
        static HttpClientTool()
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Accept.
                Add(new MediaTypeWithQualityHeaderValue("application/json")
                { CharSet = "utf-8" });
        }
        public static string HttpGet(string url)
        {
            var responseMsg = Client.GetAsync(url).Result;
            var responseContent = responseMsg.Content.ReadAsStringAsync().Result;
            if (responseMsg.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpRequestException(string.Format("调用{0}接口出错！StatusCode：{1}，Msg：{2}",
                    url, responseMsg.StatusCode, responseContent));
            }
            return responseContent;
        }
        public static T HttpGet<T>(string url)
        {
            var responseMsg = Client.GetAsync(url).Result;
            var responseContent = responseMsg.Content.ReadAsStringAsync().Result;
            if (responseMsg.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new HttpRequestException(string.Format("调用{0}接口出错！StatusCode：{1}，Msg：{2}",
                    url, responseMsg.StatusCode, responseContent));
            }
            return JsonConvert.DeserializeObject<T>(responseContent);
        }

        public static string HttpPost(string url, object data)
        {
            var content = JsonConvert.SerializeObject(data);
            var httpContent = new StringContent(content);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var responseMsg = Client.PostAsync(url, httpContent).Result;
            var responseContent = responseMsg.Content.ReadAsStringAsync().Result;
            if (responseMsg.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new HttpRequestException(string.Format("调用{0}接口出错！StatusCode：{1}，Msg：{2}",
                    url, responseMsg.StatusCode, content));
            }
            return responseContent;
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <typeparam name="T">请求返回类型</typeparam>
        /// <param name="apiUrl">请求地址</param>
        /// <param name="obj">post数据对象</param>
        /// <returns></returns>
        public static T HttpPost<T>(string apiUrl, object obj)
        {
            T responseResult;
            try
            {
                responseResult = RetryDecorator(Post<T>, apiUrl, obj);
            }
            catch (Exception e)
            {
                throw new Exception("HttpPost请求异常.ExceptionUrl:" + apiUrl, e);
            }

            return responseResult;
        }

        /// <summary>
        ///HttpClient实现Post请求
        /// </summary>
        private static async Task<T> Post<T>(string url, object obj)
        {
            HttpClientHandler handler = new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip };
            using (HttpClient http = new HttpClient(handler))
            {
                string strBody = JsonConvert.SerializeObject(obj);
                StringContent bodyContent = new StringContent(strBody);
                bodyContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await http.PostAsync(url, bodyContent).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                object content = JsonConvert.DeserializeObject(result, typeof(T));
                return (T)content;
            }
        }
        public static string HttpPut(string url, object data)
        {
            var content = JsonConvert.SerializeObject(data);
            var httpContent = new StringContent(content);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var responseMsg = Client.PutAsync(url, httpContent).Result;
            var responseContent = responseMsg.Content.ReadAsStringAsync().Result;
            if (responseMsg.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new HttpRequestException(string.Format("调用{0}接口出错！StatusCode：{1}，Msg：{2}",
                    url, responseMsg.StatusCode, content));
            }
            return responseContent;
        }
        public static string HttpDelete(string url)
        {
            var responseMsg = Client.DeleteAsync(url).Result;
            var responseContent = responseMsg.Content.ReadAsStringAsync().Result;
            if (responseMsg.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new HttpRequestException(string.Format("调用{0}接口出错！StatusCode：{1}，Msg：{2}",
                    url, responseMsg.StatusCode, responseContent));
            }
            return responseContent;
        }

        private static T RetryDecorator<T>(Func<string, object, Task<T>> Webmethod, string url, object postObj, int retryTime = 1, int retrySecondSpan = 30)
        {
            bool IsSuccess = false;
            int currentRetryTime = 0;
            T webResult = default(T);
            while (!IsSuccess && currentRetryTime <= retryTime)
            {
                try
                {
                    currentRetryTime++;
                    webResult = Webmethod(url, postObj).GetAwaiter().GetResult();
                    IsSuccess = true;
                }
                catch
                {
                    Thread.Sleep(retrySecondSpan * 1000);
                }
            }
            return webResult;
        }
    }
}
