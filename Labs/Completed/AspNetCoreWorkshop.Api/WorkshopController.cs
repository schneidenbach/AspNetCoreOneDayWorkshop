using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreWorkshop.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public abstract class WorkshopController : ControllerBase
    {
        protected IMediator Mediator => HttpContext.RequestServices.GetRequiredService<IMediator>();

        protected async Task<IActionResult> HandleRequestAsync(IRequest<Func<ControllerBase, IActionResult>> request)
        {
            if (request == null)
            {
                var error = new ValidationProblemDetails
                {
                    Detail = "The body of the request contained no usable content.",
                    Status = StatusCodes.Status400BadRequest,
                    Instance = HttpContext.Request.Path,
                    Title = "A bad request was received."
                };

                return BadRequest(error);
            }

            var response = await Mediator.Send(request);
            return response(this);
        }
    }
}