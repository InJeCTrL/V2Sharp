using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using V2Sharp.IRepository;
using V2Sharp.Models;

namespace V2Sharp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SysctlController : ControllerBase
    {
        private readonly IUserInfo _userInfo;
        private readonly IStatus _status;

        public SysctlController(IUserInfo userInfo, IStatus status)
        {
            _userInfo = userInfo;
            _status = status;
        }

        // GET: api/Sysctl/newPeriod
        [HttpGet("newPeriod")]
        public async Task NewPeriod()
        {
            await _userInfo.NewTrafficInfo();
        }

        // GET: api/Sysctl/v2restart
        [HttpGet("v2restart")]
        public async Task V2Restart()
        {
            _status.Restart();
            await _userInfo.RefreshUsers();
        }

        // GET: api/Sysctl/up/111-xxx
        [HttpGet("up/{GUID}")]
        public async Task Up(string GUID)
        {
            await _userInfo.Upgrade(GUID);
        }

        // GET: api/Sysctl/down/111-xxx
        [HttpGet("down/{GUID}")]
        public async Task Down(string GUID)
        {
            await _userInfo.Downgrade(GUID);
        }
    }
}
