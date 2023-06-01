using System.Security.Claims;

namespace YCNBot.UnitTest
{
    public class AgreedToTermsClaimTransformationTests
    {
        [Fact]
        public async Task TransformAsync_AlreadyAgreed_ReturnsClaimsWithOneAgreedTermClaim()
        {
            var claimsPrincipal = new ClaimsPrincipal();

            claimsPrincipal.AddIdentity(new ClaimsIdentity(new Claim[]
            {
                new Claim("AgreedToTerms", "true")
            }));

            var result = await new AgreedToTermsClaimTransformation().TransformAsync(claimsPrincipal);

            Assert.True(result.Claims.Count(x => x.Type == "AgreedToTerms") == 1);
        }

        [Fact]
        public async Task TransformAsync_NotAgreed_ReturnsClaimsWithOneAgreedTermClaim()
        {
            var claimsPrincipal = new ClaimsPrincipal();

            claimsPrincipal.AddIdentity(new ClaimsIdentity());

            var result = await new AgreedToTermsClaimTransformation().TransformAsync(claimsPrincipal);

            Assert.Contains(result.Claims, item => item.Type == "AgreedToTerms");
        }
    }
}
