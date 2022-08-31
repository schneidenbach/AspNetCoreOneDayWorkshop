using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreWorkshop.Api.Jobs.CreateJob
{
    public class CreateJobRequest : IRequest<Func<ControllerBase, IActionResult>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Number { get; set; }
        public DateTime StartDate { get; set; }
        public int NumberOfProjectManagers { get; set; }
        public decimal TotalCost { get; set; }
    }
}
