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
            // compose new principal
            return new ClaimsPrincipal();
        }

        private static bool is_popular(ClaimsPrincipal incomingPrincipal)
        {
            // people with >500 facebook friends are popular
            return false;
        }
    }

    public class ClaimsTransformation_demos
    {
        [Fact]
        public void custom_claims_tranformation()
        {

        }
    }
}