using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using V2Sharp.Models;

namespace V2Sharp.IRepository
{
    public interface IUserInfo
    {
        /// <summary>
        /// 用于连接的配置信息
        /// </summary>
        public class ConfigInfo
        {
            /// <summary>
            /// 用户GUID
            /// </summary>
            public string GUID { get; set; }
            /// <summary>
            /// 服务器IP
            /// </summary>
            public string ServerIP { get; set; }
            /// <summary>
            /// 服务器端口号
            /// </summary>
            public int Port { get; set; }
            /// <summary>
            /// 是否是管理员
            /// </summary>
            public bool IsAdmin { get; set; }
        }
        /// <summary>
        /// 流量使用情况
        /// </summary>
        public class TrafficInfo
        {
            /// <summary>
            /// 用户GUID
            /// </summary>
            public string GUID { get; set; }
            /// <summary>
            /// 流量记录开始时间
            /// </summary>
            public DateTime StartTime { get; set; }
            /// <summary>
            /// 上行流量字节数
            /// </summary>
            public long UpTraffic { get; set; }
            /// <summary>
            /// 下行流量字节数
            /// </summary>
            public long DownTraffic { get; set; }
            /// <summary>
            /// 总流量字节数
            /// </summary>
            public long TotalTraffic { get; set; }
            /// <summary>
            /// 总流量占比
            /// </summary>
            public double Rate { get; set; }
        }
        /// <summary>
        /// 获取某用户的连接配置信息
        /// </summary>
        /// <param name="GUID">用户GUID</param>
        /// <returns></returns>
        public Task<ConfigInfo> GetConfigInfo(string GUID);
        /// <summary>
        /// 获取某用户的流量使用情况
        /// </summary>
        /// <param name="GUID">用户GUID</param>
        /// <returns></returns>
        public Task<TrafficInfo> GetTrafficByGUID(string GUID);
        /// <summary>
        /// 获取所有用户的流量使用情况
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<TrafficInfo>> GetAllTraffic();
        /// <summary>
        /// 刷新数据库中用户信息
        /// </summary>
        /// <returns></returns>
        public Task RefreshUsers();
        /// <summary>
        /// 新建一个统计周期
        /// </summary>
        /// <returns></returns>
        public Task NewTrafficInfo();
        /// <summary>
        /// 将指定GUID用户提升为Admin
        /// </summary>
        /// <param name="GUID">用户GUID</param>
        /// <returns></returns>
        public Task<string> Upgrade(string GUID);
        /// <summary>
        /// 将指定GUID用户降低为普通用户
        /// </summary>
        /// <param name="GUID">用户GUID</param>
        /// <returns></returns>
        public Task<string> Downgrade(string GUID);
        /// <summary>
        /// 刷新数据库中流量信息
        /// </summary>
        /// <returns></returns>
        public Task RefreshTraffic();
    }
}
