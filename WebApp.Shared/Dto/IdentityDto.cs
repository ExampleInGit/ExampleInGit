using System.Security.Claims;

namespace WebApp.Shared.Dto
{
    public class IdentityDto
    {
        public ClaimsIdentity Identity { get; set; }
        public string Email { get; set; }
    }
}
