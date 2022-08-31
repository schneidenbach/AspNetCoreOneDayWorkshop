using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using AspNetCoreWorkshop.Api.JobPhases.CreateJobPhaseForJob;
using AspNetCoreWorkshop.Api.JobPhases.GetJobPhasesForJob;
using AspNetCoreWorkshop.Api.Jobs.CreateJob;
using AspNetCoreWorkshop.Api.Jobs.DeleteJob;
using AspNetCoreWorkshop.Api.Jobs.GetJob;
using AspNetCoreWorkshop.Api.Jobs.GetJobs;
using AspNetCoreWorkshop.Api.Jobs.UpdateJob;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreWorkshop.Api.Jobs
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JobsController : WorkshopController
    {
        /// <summary>
        /// Gets a job with a specific ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetJobResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public Task<IActionResult> GetJob([FromRoute] GetJobRequest request)
        {
            return HandleRequestAsync(request);
        }

        /// <summary>
        /// Gets a list of jobs.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetJobResponse>))]
        [HttpGet]
        public Task<IActionResult> GetJobs([FromQuery] GetJobsRequest request)
        {
            return HandleRequestAsync(request);
        }
        
        /// <summary>
        /// Creates a new job.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateJobResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [HttpPost]
        public Task<IActionResult> CreateJob(CreateJobRequest request)
        {
            return HandleRequestAsync(request);
        }

        /// <summary>
        /// Updates an existing job. You do not need to specify all of the 
        /// </summary>
        /// <param name="id">The ID of the job being updated.</param>
        /// <param name="body">The body of the request.</param>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateJobResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJob(int id, JsonElement body)
        {
            var updateJobRequest = new UpdateJobRequest(id, body);
            if (!TryValidateModel(updateJobRequest))
            {
                return BadRequestAsValidationProblemDetails();
            }
            
            return await HandleRequestAsync(updateJobRequest);
        }

        /// <summary>
        /// Deletes a specified job.
        /// </summary>
        /// <param name="id">The ID of the job.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Task<IActionResult> DeleteJob(int id)
        {
            return HandleRequestAsync(new DeleteJobRequest(id));
        }

        /// <summary>
        /// Gets the job phases for a specified jobs. Project managers only!
        /// </summary>
        [Authorize(Roles = "project manager")]
        [HttpGet("{jobId}/phases")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GetJobPhasesForJobResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Task<IActionResult> GetJobPhasesForJob([FromRoute] GetJobsPhasesForJobRequest request)
        {
            return HandleRequestAsync(request);
        }

        /// <summary>
        /// Creates a phase for a job. Project managers only!
        /// </summary>
        /// <param name="jobId">The ID of the job.</param>
        /// <param name="request">The body of the request.</param>
        [Authorize(Roles = "project manager")]
        [HttpPost("{jobId}/phases")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetJobPhasesForJobResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateJobPhasesForJob(int jobId, [FromBody] CreateJobPhaseForJobRequestBody request)
        {
            var body = new CreateJobPhaseForJobRequest(jobId, request?.Number, request?.Description);
            if (!TryValidateModel(body))
            {
                return BadRequestAsValidationProblemDetails();
            }
            return await HandleRequestAsync(body);
        }
    }
}

