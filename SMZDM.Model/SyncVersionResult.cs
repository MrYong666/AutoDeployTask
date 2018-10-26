using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMZDM.Model
{
    public class SyncVersionResult : ApiResult
    {
        /// <summary>
        /// 执行日志
        /// </summary>
        [JsonProperty("operationalLog")]
        public string OperationalLog { get; set; }
    }
}
