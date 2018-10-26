using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMZDM.Model
{
    public class VersionResult : ApiResult
    {
        /// <summary>
        /// 文件全名
        /// </summary>
        [JsonProperty("downLoadUrl")]
        public string DownLoadUrl { get; set; }
        /// <summary>
        /// 文件全名
        /// </summary>
        [JsonProperty("fullFileName")]
        public string FullFileName { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        [JsonProperty("versionNumber")]
        public string VersionNumber { get; set; }
    }
}
