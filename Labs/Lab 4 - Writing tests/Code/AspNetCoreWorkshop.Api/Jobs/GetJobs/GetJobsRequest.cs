using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreWorkshop.Api.Jobs.GetJobs
{
    public class GetJobsRequest : IRequest<Func<ControllerBase, IActionResult>>
    {
    }
}
