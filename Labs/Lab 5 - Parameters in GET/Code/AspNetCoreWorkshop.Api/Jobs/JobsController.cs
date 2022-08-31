using System.Threading.Tasks;
using AspNetCoreWorkshop.Api.Jobs.GetJob;
using AspNetCoreWorkshop.Api.Jobs.GetJobs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreWorkshop.Api.Jobs
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : WorkshopController
    {
        [HttpGet("{id}")]
        public Task<IActionResult> GetJob([FromRoute] GetJobRequest request)
        {
            return HandleRequestAsync(request);
        }

        [HttpGet]
        public Task<IActionResult> GetJobs([FromQuery] GetJobsRequest request)
        {
            return HandleRequestAsync(request);
        }
    }
}

