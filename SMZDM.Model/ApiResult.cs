using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMZDM.Model
{
    public class ApiResult
    {
        /// <summary>
        /// 错误码（0：成功，其他失败）
        /// </summary>
        [JsonProperty("errcode")]
        public int ErrCode { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        [JsonProperty("errmsg")]
        public string ErrMsg { get; set; }
    }
}