using AspNetCoreWorkshop.Api.Jobs.GetJob;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreWorkshop.Api.Security
{
    [ApiController]
    [Route("security")]
    public class SecurityController : ControllerBase
    {
        /// <summary>
        /// Generates a token for your use in the application.
        /// </summary>
        [HttpPost("generateToken")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        public IActionResult GetToken([FromBody] GetTokenRequestBody request)
        {
            return Ok(HttpContext.GenerateJwt(request.Role));
        }
    }
}