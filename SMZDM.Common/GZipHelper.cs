using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Data;

namespace SMZDM.Common
{
    public class GZipHelper
    {
        #region 压缩
        /// <summary>
        /// 将传入字符串以GZip算法压缩后，返回Base64编码字符
        /// </summary>
        /// <param name="compressString">需要压缩的字符串</param>
        /// <returns>压缩后的Base64编码的字符串</returns>
        public static string GZipCompressString(string compressString)
        {
            if (string.IsNullOrEmpty(compressString) || compressString.Length == 0)
            {
                return "";
            }
            else
            {
                byte[] rawData = System.Text.Encoding.UTF8.GetBytes(compressString.ToString());
                byte[] zippedData = Compress(rawData);
                return (string)(Convert.ToBase64String(zippedData));
            }
        }
        /// <summary>
        /// GZip压缩
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] rawData)
        {
            MemoryStream ms = new MemoryStream();
            using (var compressedzipStream = new GZipStream(ms, CompressionMode.Compress, true))
            {
                compressedzipStream.Write(rawData, 0, rawData.Length);
            }
            return ms.ToArray();
        }
        #endregion

        #region 解压
        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static DataSet GetDatasetByString(string Value)
        {
            DataSet ds = new DataSet();
            string CC = GZipDecompressString(Value);
            System.IO.StringReader Sr = new StringReader(CC);
            ds.ReadXml(Sr);
            return ds;
        }
        /// <summary>
        /// 将传入的二进制字符串资料以GZip算法解压缩
        /// </summary>
        /// <param name="zippedString">经GZip压缩后的二进制字符串</param>
        /// <returns>原始未压缩字符串</returns>
        public static string GZipDecompressString(string zippedString)
        {
            if (string.IsNullOrEmpty(zippedString) || zippedString.Length == 0)
            {
                return "";
            }
            else
            {
                byte[] zippedData = Convert.FromBase64String(zippedString.ToString());
                return (string)(System.Text.Encoding.UTF8.GetString(Decompress(zippedData)));
            }
        }
        public static string GZipDecompressbyte(byte[] str)
        {
            return (string)(System.Text.Encoding.UTF8.GetString(Decompress(str)));
        }
        /// <summary>
        /// GZIP解压
        /// </summary>
        /// <param name="zippedData"></param>
        /// <returns></returns>
        private static byte[] Decompress(byte[] zippedData)
        {
            MemoryStream ms = new MemoryStream(zippedData);
            GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Decompress);
            MemoryStream outBuffer = new MemoryStream();
            byte[] block = new byte[1024];
            while (true)
            {
                int bytesRead = compressedzipStream.Read(block, 0, block.Length);
                if (bytesRead <= 0)
                    break;
                else
                    outBuffer.Write(block, 0, bytesRead);
            }
            compressedzipStream.Close();
            return outBuffer.ToArray();
        }
        #endregion

        #region 余留
        //public static Stream GetGzipStream(List<Person> person)
        //{
        //    var ms = new MemoryStream();
        //    string json = JsonConvert.SerializeObject(person);
        //    byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
        //    // byte[] jsonBytes = Encoding.GetEncoding("iso-8859-1").GetBytes(json);
        //    using (var gzip = new GZipStream(ms, CompressionMode.Compress, true))
        //    {
        //        gzip.Write(jsonBytes, 0, jsonBytes.Length);
        //    }
        //    ms.Position = 0;
        //    return ms;
        //}
        /// <summary>
        /// Read  Bytes in Stream 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(bytes, 0, bytes.Length);
            return bytes;
        }
        /// <summary>
        /// SringToBase64
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        public string Base64Code(string Message)
        {
            byte[] bytes = Encoding.Default.GetBytes(Message);
            return Convert.ToBase64String(bytes);
        }
        #endregion

    }
}

