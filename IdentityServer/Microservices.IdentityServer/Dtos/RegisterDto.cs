using System.ComponentModel.DataAnnotations;

namespace Microservices.IdentityServer.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string City { get; set; }

    }
}