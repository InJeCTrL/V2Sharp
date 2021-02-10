using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace V2Sharp.IRepository
{
    public interface IStatus
    {
        public string ServerIP { get; }
        public int Port { get; }
        /// <summary>
        /// 获取config中clients列表
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<object>> GetConfig();
        /// <summary>
        /// 重启V2ray服务（可使新配置生效）
        /// </summary>
        /// <returns></returns>
        public void Restart();
    }
}
