using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using V2Sharp.IRepository;
using V2Sharp.Models;
using static V2Sharp.IRepository.IUserInfo;

namespace V2Sharp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigInfoController : ControllerBase
    {
        private readonly IUserInfo _userInfo;

        public ConfigInfoController(IUserInfo userInfo)
        {
            _userInfo = userInfo;
        }
        // GET: api/ConfigInfo/111-xxx
        [HttpGet("{GUID}")]
        public async Task<ActionResult<ConfigInfo>> GetUserConfig(string GUID)
        {
            return await _userInfo.GetConfigInfo(GUID);
        }
    }
}
