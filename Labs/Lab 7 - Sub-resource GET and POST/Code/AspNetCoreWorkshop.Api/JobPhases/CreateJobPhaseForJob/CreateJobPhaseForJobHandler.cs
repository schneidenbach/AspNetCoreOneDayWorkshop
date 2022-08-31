using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AspNetCoreWorkshop.Api.Jobs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreWorkshop.Api.JobPhases.CreateJobPhaseForJob
{
    public class CreateJobPhaseForJobHandler : IRequestHandler<CreateJobPhaseForJobRequest, Func<ControllerBase, IActionResult>>
    {
        public CreateJobPhaseForJobHandler(
            WorkshopDbContext workshopDbContext,
            IMapper mapper
        )
        {
            WorkshopDbContext = workshopDbContext ?? throw new System.ArgumentNullException(nameof(workshopDbContext));
            Mapper = mapper ?? throw new System.ArgumentNullException(nameof(mapper));
        }

        public WorkshopDbContext WorkshopDbContext { get; }
        public IMapper Mapper { get; }

        public async Task<Func<ControllerBase, IActionResult>> Handle(CreateJobPhaseForJobRequest message, CancellationToken cancellationToken)
        {
            //TODO: make sure job exists - if it doesn't return Not Found
            //TODO: return JobPhaseNumber/Description using Ok
            throw new NotImplementedException();
        }
    }
}