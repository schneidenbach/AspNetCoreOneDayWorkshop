using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AspNetCoreWorkshop.Api.Jobs.GetJob;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreWorkshop.Api.Jobs.UpdateJob
{
    public class UpdateJobRequestHandler : IRequestHandler<UpdateJobRequest, Func<ControllerBase, IActionResult>>
    {
        private readonly WorkshopDbContext _workshopDbContext;

        private readonly IMapper _mapper;

        public UpdateJobRequestHandler(
            IEnumerable<IValidator<UpdateJobRequest>> validators,
            WorkshopDbContext workshopDbContext,
            IMapper mapper)
        {
            _workshopDbContext = workshopDbContext ?? throw new ArgumentNullException(nameof(workshopDbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Func<ControllerBase, IActionResult>> Handle(UpdateJobRequest message, CancellationToken cancellationToken)
        {
            var theJob = await _workshopDbContext.Jobs
                .FindAsync(new object[] {message.Id}, cancellationToken);

            if (theJob == null)
            {
                return controller => controller.NotFound();
            }

            var properties = typeof(Job).GetProperties();

            foreach (var prop in message.Data.EnumerateObject()) {
                var name = prop.Name;
                var value = prop.Value;

                var property = properties.SingleOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                if (property == null) {
                    continue;
                }

                property.SetValue(theJob, value.Deserialize(property.PropertyType));
            }

            await _workshopDbContext.SaveChangesAsync(cancellationToken);
            var ret = _mapper.Map<UpdateJobResponse>(theJob);
            return controller => controller.Ok(ret);
        }
    }
}