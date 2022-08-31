using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreWorkshop.Api.Security
{
    [ApiController]
    [Route("security")]
    public class SecurityController : ControllerBase
    {
        [HttpPost("generateToken")]
        public IActionResult GetToken([FromBody] GetTokenRequestBody request)
        {
            return Ok(HttpContext.GenerateJwt(request.Role));
        }
    }
}