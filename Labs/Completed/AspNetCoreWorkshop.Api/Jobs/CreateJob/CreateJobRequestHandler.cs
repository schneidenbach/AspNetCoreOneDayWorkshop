using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreWorkshop.Api.Jobs.CreateJob
{
    public class CreateJobRequestHandler : IRequestHandler<CreateJobRequest, Func<ControllerBase, IActionResult>>
    {
        public WorkshopDbContext WorkshopDbContext { get; }
        public IMapper Mapper { get; }

        public CreateJobRequestHandler(
            IEnumerable<IValidator<CreateJobRequest>> validators,
            WorkshopDbContext workshopDbContext,
            IMapper mapper)
        {
            WorkshopDbContext = workshopDbContext ?? throw new ArgumentNullException(nameof(workshopDbContext));
            Mapper = mapper;
        }

        public async Task<Func<ControllerBase, IActionResult>> Handle(CreateJobRequest message, CancellationToken cancellationToken)
        {
            var newJob = Mapper.Map<Job>(message);
            WorkshopDbContext.Add(newJob);
            await WorkshopDbContext.SaveChangesAsync(cancellationToken);
            return controller => controller.Ok(new CreateJobResponse(newJob.Id));
        }
    }
}