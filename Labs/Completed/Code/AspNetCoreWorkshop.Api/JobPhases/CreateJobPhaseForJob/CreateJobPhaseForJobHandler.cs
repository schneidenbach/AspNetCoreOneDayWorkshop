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
            var job = await WorkshopDbContext.Jobs.FindAsync(message.JobId);
            if (job == null) {
                return controller => controller.NotFound();
            }
            
            var phase = Mapper.Map<JobPhase>(message);
            WorkshopDbContext.Add(phase);
            await WorkshopDbContext.SaveChangesAsync(cancellationToken);

            return controller => controller.Ok(new CreateJobPhaseForJobResponse
            {
                JobId = job.Id,
                Id = phase.Id
            });
        }
    }
}