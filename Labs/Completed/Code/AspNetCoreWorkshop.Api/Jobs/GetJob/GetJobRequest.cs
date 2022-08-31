﻿using System;
using System.Collections.Generic;
using AspNetCoreWorkshop.Api.Jobs.GetJobs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreWorkshop.Api.Jobs.GetJob
{
    public class GetJobRequest : IRequest<Func<ControllerBase, IActionResult>>
    {
        public int Id { get; set; }
    }
}
