using System;
using System.Collections.Generic;
using System.Text;

namespace PuppetCat.AspNetCore.Mvc.Log
{
    public class RequestLogEntity
    {
        /// <summary>
        /// 请求ID
        /// </summary>
        public string RequestId { get; set; }
        /// <summary>
        /// 响应时间
        /// </summary>
        public string RequestSourse { get; set; }
        /// <summary>
        /// 返回编号
        /// </summary>
        public string ResponseCode { get; set; }
        /// <summary>
        /// 响应时间(毫秒)
        /// </summary>
        public long ResponseTime { get; set; }
        /// <summary>
        /// 请求报文
        /// </summary>
        public string Request { get; set; }
        /// <summary>
        /// 接口内部日志
        /// </summary>
        public Dictionary<string, string> ApiTrace { get; set; } = new Dictionary<string, string>();
        /// <summary>
        /// 返回报文
        /// </summary>
        public string Response { get; set; }
    }
}
