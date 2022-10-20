using System.Security.Claims;
using System.Security.Principal;

namespace WebApp.Shared.Token
{
    public class ClaimsIdentityExtension : ClaimsIdentity
    {
        public ClaimsIdentityExtension(IEnumerable<Claim>? claims, string? authenticationType, string? nameType, string? roleType)
            : base((IIdentity?)null, claims, authenticationType, nameType, roleType)
        {
        }

        public const string DefaultEmailClaimType = ClaimTypes.Email;
    }
}
