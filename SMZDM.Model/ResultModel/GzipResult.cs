using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace SMZDM.Model.Result
{
    public class GzipResult
    {
        /// <summary>
        /// 错误码（0：成功，其他失败）
        /// </summary>
        [JsonProperty("msg")]
        public string msg { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        [JsonProperty("code")]
        public string code { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        [JsonProperty("data")]
        public Data data { get; set; }
    }
    public class Data
    {
        /// <summary>
        /// 错误码（0：成功，其他失败）
        /// </summary>
        [JsonProperty("cnt")]
        public string cnt { get; set; }

    }
}