using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace YCNBot
{
    public class AgreedToTermsClaimTransformation : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            ClaimsIdentity claimsIdentity = new ();

            var claimType = "AgreedToTerms";

            if (!principal.HasClaim(claim => claim.Type == claimType))
            {
                claimsIdentity.AddClaim(new Claim(claimType, "true"));
            }

            principal.AddIdentity(claimsIdentity);

            return Task.FromResult(principal);
        }
    }
}
