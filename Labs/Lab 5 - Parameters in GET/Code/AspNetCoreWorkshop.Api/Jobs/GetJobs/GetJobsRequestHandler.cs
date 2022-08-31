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

namespace AspNetCoreWorkshop.Api.Jobs.GetJobs
{
    public class GetJobsRequestHandler : IRequestHandler<GetJobsRequest, Func<ControllerBase, IActionResult>>
    {
        private readonly WorkshopDbContext _workshopDbContext;
        private readonly IMapper _mapper;

        public GetJobsRequestHandler(
            IEnumerable<IValidator<GetJobsRequest>> validators,
            WorkshopDbContext workshopDbContext,
            IMapper mapper)
        {
            _workshopDbContext = workshopDbContext ?? throw new ArgumentNullException(nameof(workshopDbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Func<ControllerBase, IActionResult>> Handle(GetJobsRequest message, CancellationToken cancellationToken)
        {
            var ret = await _workshopDbContext.Jobs
                .ProjectTo<GetJobsResponse>(_mapper.ConfigurationProvider)
                .ToArrayAsync(cancellationToken);
            return controller => controller.Ok(ret);
        }
    }
}