using LabCheckin.Server.Models;
using LabCheckin.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LabCheckin.Server.Controllers
{
    [ApiController]
    [Route("/api/checkin")]
    public class CheckinController : ControllerBase
    {
        [Route(""), HttpPost]
        public Task<ResponseModel<bool>> CheckinAsync([FromBody] CheckinModel model)
        {
            throw new NotImplementedException();
            // return new JsonResultModel<bool>(true);
        }
    }
}
