using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using V2Sharp.IRepository;
using V2Sharp.Models;
using static V2Sharp.IRepository.IUserInfo;

namespace V2Sharp.Repository
{
    public class UserInfo : IUserInfo
    {
        private readonly V2SharpContext _context;
        private readonly IStatus _status;
        public UserInfo(V2SharpContext context, IStatus status)
        {
            _context = context;
            _status = status;
        }
        public async Task<TrafficInfo> GetTrafficByGUID(string GUID)
        {
            await RefreshTraffic();
            var TrafficInfoSet = from user in _context.User
                                 where user.GUID == GUID
                                 select new TrafficInfo
                                 {
                                     GUID = user.GUID,
                                     StartTime = user.StartTime,
                                     DownTraffic = user.DownTraffic,
                                     UpTraffic = user.UpTraffic,
                                     TotalTraffic = user.DownTraffic + user.UpTraffic,
                                     Rate = (user.DownTraffic + user.UpTraffic) == 0 ? 0 : ((user.DownTraffic + user.UpTraffic) /
                                     (
                                     from user1 in _context.User     
                                     group user1 by user1.StartTime into guser1                   
                                     select guser1.Sum(u => u.UpTraffic + u.DownTraffic)
                                     ).FirstOrDefault())
                                 };
            return await Task.FromResult(TrafficInfoSet.FirstOrDefault());
        }
        public async Task<IEnumerable<TrafficInfo>> GetAllTraffic()
        {
            await RefreshTraffic();
            var TrafficInfoSet = from user in _context.User
                                 select new TrafficInfo
                                 {
                                     GUID = user.GUID,
                                     StartTime = user.StartTime,
                                     DownTraffic = user.DownTraffic,
                                     UpTraffic = user.UpTraffic,
                                     TotalTraffic = user.DownTraffic + user.UpTraffic,
                                     Rate = (user.DownTraffic + user.UpTraffic) == 0 ? 0 : ((user.DownTraffic + user.UpTraffic) /
                                     (
                                     from user1 in _context.User
                                     group user1 by user1.StartTime into guser1
                                     select guser1.Sum(u => u.UpTraffic + u.DownTraffic)
                                     ).FirstOrDefault())
                                 };
            return await Task.FromResult(TrafficInfoSet);
        }

        public async Task<ConfigInfo> GetConfigInfo(string GUID)
        {
            var Config = from user in _context.User
                         where user.GUID == GUID
                         select new ConfigInfo
                         {
                             GUID = user.GUID,
                             ServerIP = _status.ServerIP,
                             Port = _status.Port
                         };
            return await Task.FromResult(Config.FirstOrDefault());
        }
        public async Task RefreshUsers()
        {
            var Clients = await _status.GetConfig() as List<Dictionary<string, string>>;
            var linqGUID = from user in _context.User
                           select new { user.ID, user.GUID };
            var GUIDsInDB = await Task.FromResult(linqGUID.ToHashSet());
            // 删除数据库中多余的User / 更新数据库中的User
            foreach (var GUIDInDB in GUIDsInDB)
            {
                bool Found = false;
                foreach (var Client in Clients)
                {
                    if (Client["id"] == GUIDInDB.GUID)
                    {
                        _context.User.Update(new User { ID = GUIDInDB.ID, GUID = Client["id"], Email = Client["email"] });
                        Found = true;
                        break;
                    }
                }
                if (!Found)
                {
                    _context.User.Remove(new User { ID = GUIDInDB.ID });
                }
            }
            await _context.SaveChangesAsync();
            // 将配置文件中新User插入数据库
            foreach (var client in Clients)
            {
                var ID = client["id"];
                var Email = client["email"];
                bool Found = false;
                foreach (var GUIDInDB in GUIDsInDB)
                {
                    if (GUIDInDB.GUID == ID)
                    {
                        Found = true;
                        break;
                    }
                }
                if (!Found)
                {
                    _context.User.Add(new User
                    {
                        Email = Email,
                        GUID = ID,
                    });
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task NewTrafficInfo()
        {
            var now = DateTime.Now;
            var linqID = from user in _context.User
                         select user.ID;
            var IDs = await Task.FromResult(linqID.ToHashSet());
            foreach (var ID in IDs)
            {
                _context.User.Update(new User
                {
                    ID = ID,
                    StartTime = now,
                    DownTraffic = 0,
                    UpTraffic = 0,
                });
            }
            await _context.SaveChangesAsync();
            Process.Start("v2ctl api --server=127.0.0.1:1090 StatsService.QueryStats 'reset: true'");
        }

        public async Task<string> Upgrade(string GUID)
        {
            var tID = (from user in _context.User
                         where user.GUID == GUID
                         select user.ID).FirstOrDefault();
            _context.User.Update(new User
            {
                ID = tID,
                IsAdmin = true
            });
            int AffectedRows = await _context.SaveChangesAsync();
            if (AffectedRows > 0)
            {
                return "OK";
            }
            else
            {
                return "Fail";
            }
        }

        public async Task<string> Downgrade(string GUID)
        {
            var tID = (from user in _context.User
                       where user.GUID == GUID
                       select user.ID).FirstOrDefault();
            _context.User.Update(new User
            {
                ID = tID,
                IsAdmin = false
            });
            int AffectedRows = await _context.SaveChangesAsync();
            if (AffectedRows > 0)
            {
                return "OK";
            }
            else
            {
                return "Fail";
            }
        }

        public async Task RefreshTraffic()
        {
            var psi = new ProcessStartInfo("v2ctl api --server=127.0.0.1:1090 StatsService.QueryStats 'reset: true'")
            {
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                UseShellExecute = false
            };

            using (Process process = Process.Start(psi))
            {
                var output = process.StandardOutput.ReadToEnd();
                // TODO: 解析traffic数据，写入数据库
            }
        }
    }
}
