using System.ComponentModel.DataAnnotations;

namespace AspNetCoreWorkshop.Api.Security
{
    public class GetTokenRequestBody
    {
        [Required]
        public string Role { get; set; }
    }
}