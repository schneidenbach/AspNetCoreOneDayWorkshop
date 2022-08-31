using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AspNetCoreWorkshop.Api.Jobs.GetJob;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreWorkshop.Api.Jobs.DeleteJob
{
    public class DeleteJobRequestHandler : IRequestHandler<DeleteJobRequest, Func<ControllerBase, IActionResult>>
    {
        private readonly WorkshopDbContext _workshopDbContext;

        private readonly IMapper _mapper;

        public DeleteJobRequestHandler(
            IEnumerable<IValidator<DeleteJobRequest>> validators,
            WorkshopDbContext workshopDbContext,
            IMapper mapper)
        {
            _workshopDbContext = workshopDbContext ?? throw new ArgumentNullException(nameof(workshopDbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Func<ControllerBase, IActionResult>> Handle(DeleteJobRequest message, CancellationToken cancellationToken)
        {
            var theJob = await _workshopDbContext.Jobs
                .FindAsync(message.Id);

            if (theJob == null)
            {
                return controller => controller.NotFound();
            }

            _workshopDbContext.Remove(theJob);
            await _workshopDbContext.SaveChangesAsync(cancellationToken);

            return controller => controller.NoContent();
        }
    }
}