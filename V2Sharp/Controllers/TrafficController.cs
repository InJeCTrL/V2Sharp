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
    public class TrafficController : ControllerBase
    {
        private readonly IUserInfo _userInfo;

        public TrafficController(IUserInfo userInfo)
        {
            _userInfo = userInfo;
        }
        // GET: api/Traffic
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrafficInfo>>> GetAllTraffic()
        {
            return new JsonResult(await _userInfo.GetAllTraffic());
        }
        // GET: api/Traffic/111-xxx
        [HttpGet("{GUID}")]
        public async Task<ActionResult<IEnumerable<TrafficInfo>>> GetTrafficByGUID(string GUID)
        {
            return new JsonResult(await _userInfo.GetTrafficByGUID(GUID));
        }
    }
}
