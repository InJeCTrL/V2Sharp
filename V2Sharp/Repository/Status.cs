using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using V2Sharp.IRepository;

namespace V2Sharp.Repository
{
    public class Status : IStatus
    {
        public string ServerIP { get; }
        public int Port { get; private set; }
        public Status()
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("http://ip-api.com/json");
            using (WebResponse webResponse = webRequest.GetResponse())
            {
                using (Stream respstream = webResponse.GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(respstream))
                    {
                        string ret = streamReader.ReadToEnd();
                        var IPContent = JsonSerializer.Deserialize<Dictionary<string, object>>(ret);
                        ServerIP = IPContent["query"].ToString();
                    }
                }
            }
        }
        public async Task<IEnumerable<object>> GetConfig()
        {
            var ConfigStream = File.OpenRead("/usr/local/etc/v2ray/config.json");
            var Config = await JsonSerializer.DeserializeAsync<Dictionary<string, object>>(ConfigStream);
            var inbounds = Config["inbounds"];
            var inboundFirst = ((Dictionary<int, object>)inbounds)[0];
            Port = (int)((Dictionary<string, object>)inboundFirst)["port"];
            var settings = ((Dictionary<string, object>)inboundFirst)["settings"];
            var clients = (List<object>)((Dictionary<string, object>)settings)["clients"];
            return clients;
        }

        public void Restart()
        {
            Process.Start("systemctl restart v2ray.service");
        }
    }
}
