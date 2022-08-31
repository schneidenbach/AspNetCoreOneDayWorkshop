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
            var job = await WorkshopDbContext.Jobs.FindAsync(message.JobId);
            if (job == null) {
                return controller => controller.NotFound();
            }

            var ret = await WorkshopDbContext
                .JobPhases
                .Where(p => p.JobId == job.Id)
                .ProjectTo<GetJobPhasesForJobResponse>(Mapper.ConfigurationProvider)
                .ToArrayAsync(cancellationToken);

            return controller => controller.Ok(ret);
        }
    }
}