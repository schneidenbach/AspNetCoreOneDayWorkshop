using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreWorkshop.Api.JobPhases.GetJobPhasesForJob
{
    public class GetJobsPhasesForJobRequest : IRequest<Func<ControllerBase, IActionResult>>
    {
        public int JobId { get; set; }
    }
}