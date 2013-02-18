using System.Collections.Generic;
using System.IdentityModel.Services;
using System.Security.Claims;
using Xunit;

namespace Claims.WG.NET
{
    public static class CustomClaims
    {
        public const string IsPopular = "http://my/claims/isPopular";
    }

    public class CustomClaimsAuthManager :
        // requires reference to System.IdentityModel
        ClaimsAuthenticationManager
    {
        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            bool isPopular = is_popular(incomingPrincipal);

            var claims = new List<Claim>
                {
                    incomingPrincipal.FindFirst(ClaimTypes.Name),
                    new Claim(CustomClaims.IsPopular, isPopular.ToString()),
                };

            return new ClaimsPrincipal(new ClaimsIdentity(claims));
        }

        private static bool is_popular(ClaimsPrincipal incomingPrincipal)
        {
            var fb_friends_count_claim = incomingPrincipal.FindFirst("http://schemas.facebook.com/claims/friendsCount");
            if (fb_friends_count_claim == null)
            {
                return false;
            }

            int friendsCount;
            int.TryParse(fb_friends_count_claim.Value, out friendsCount);
            return friendsCount > 50;
        }
    }

    public class ClaimsTransformation_demos
    {
        [Fact]
        public void custom_claims_tranformation()
        {
            var popularId = new FacebookIdentity("Kasia Tusk", 1000000);
            var popularPrincipal = new ClaimsPrincipal(popularId);

            // requires references to
            // * System.Web
            // * System.IdentityModel.services
            var transformedPopularPrincipal = FederatedAuthentication.FederationConfiguration
                .IdentityConfiguration.ClaimsAuthenticationManager
                .Authenticate("resource", popularPrincipal);

            Assert.Equal("True", transformedPopularPrincipal.FindFirst(CustomClaims.IsPopular).Value);

            var unpopularId = new FacebookIdentity("Maciej Aniserowicz", 3);
            var unpopularPrincipal = new ClaimsPrincipal(unpopularId);

            var transformedUnpopularPrincipal = FederatedAuthentication.FederationConfiguration
                .IdentityConfiguration.ClaimsAuthenticationManager
                .Authenticate("resource", unpopularPrincipal);

            Assert.Equal("False", transformedUnpopularPrincipal.FindFirst(CustomClaims.IsPopular).Value);
        }
    }
}