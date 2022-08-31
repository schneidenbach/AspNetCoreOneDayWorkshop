using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreWorkshop.Api.JobPhases.GetJobPhasesForJob
{
    public class GetJobPhasesForJobHandler : IRequestHandler<GetJobsPhasesForJobRequest, Func<ControllerBase, IActionResult>>
    {
        public GetJobPhasesForJobHandler(
            WorkshopDbContext workshopDbContext,
            IMapper mapper
        )
        {
            WorkshopDbContext = workshopDbContext ?? throw new System.ArgumentNullException(nameof(workshopDbContext));
            Mapper = mapper ?? throw new System.ArgumentNullException(nameof(mapper));
        }

        public WorkshopDbContext WorkshopDbContext { get; }
        public IMapper Mapper { get; }

        public async Task<Func<ControllerBase, IActionResult>> Handle(GetJobsPhasesForJobRequest message, CancellationToken cancellationToken)
        {
            //TODO: make sure Job exists in database - if not, return NotFound
            //TODO: query WorkshopDbContext.JobPhases where JobId == message.JobId, then project to GetJobPhasesForJobResponse

            throw new NotImplementedException();
        }
    }
}