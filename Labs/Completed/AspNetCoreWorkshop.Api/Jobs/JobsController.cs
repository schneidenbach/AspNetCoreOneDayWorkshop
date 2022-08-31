using System;
using System.Text.Json;
using System.Threading.Tasks;
using AspNetCoreWorkshop.Api.Jobs.CreateJob;
using AspNetCoreWorkshop.Api.Jobs.DeleteJob;
using AspNetCoreWorkshop.Api.Jobs.GetJob;
using AspNetCoreWorkshop.Api.Jobs.GetJobs;
using AspNetCoreWorkshop.Api.Jobs.UpdateJob;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Options;

namespace AspNetCoreWorkshop.Api.Jobs
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : WorkshopController
    {
        private readonly ApiBehaviorOptions _options;

        public JobsController(IOptions<ApiBehaviorOptions> options)
        {
            _options = options.Value;
        }
        
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

        [HttpPost]
        public Task<IActionResult> CreateJob(CreateJobRequest request)
        {
            return HandleRequestAsync(request);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJob(int id, JsonElement body)
        {
            var updateJobRequest = new UpdateJobRequest(id, body);

            if (!TryValidateModel(updateJobRequest))
            {
                return BadRequest(ModelState);
            }
            
            return await HandleRequestAsync(updateJobRequest);
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> DeleteJob(int id)
        {
            return HandleRequestAsync(new DeleteJobRequest(id));
        }
    }
}

