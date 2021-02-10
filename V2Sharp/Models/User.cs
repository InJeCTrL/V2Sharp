using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace V2Sharp.Models
{
    public class User
    {
        public int ID { get; set; }
        /// <summary>
        /// 用户GUID
        /// </summary>
        public string GUID { get; set; }
        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 用户是否是管理员
        /// </summary>
        public bool IsAdmin { get; set; } = false;
        /// <summary>
        /// 流量记录开始时间
        /// </summary>
        public DateTime StartTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 上行流量字节数
        /// </summary>
        public long UpTraffic { get; set; } = 0;
        /// <summary>
        /// 下行流量字节数
        /// </summary>
        public long DownTraffic { get; set; } = 0;
    }
}
