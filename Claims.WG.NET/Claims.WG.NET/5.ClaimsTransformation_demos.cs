using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.Security.Claims;
using Xunit;


// "http://facebook.com/claims/friendsCount"




namespace Claims.WG.NET
{
    public static class CustomClaims
    {
        public const string IsPopular = "http://my/claims/isPopular";
    }

    public class ClaimsTransformation_demos
    {
        [Fact]
        public void custom_claims_tranformation()
        {
            var kasia_tusk = new FacebookIdentity("Kasia Tusk", 10000000);
            var original = new ClaimsPrincipal(kasia_tusk);


            Assert.Equal(10000000, kasia_tusk.FriendsCount);

            // transform/translate

            // ClaimsPrincipal translated = ;

            // Assert.True(translated.HasClaim(CustomClaims.IsPopular, bool.TrueString));
        }
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
}