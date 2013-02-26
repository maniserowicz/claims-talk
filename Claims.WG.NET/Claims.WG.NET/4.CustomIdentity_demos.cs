using System;
using System.Security.Claims;
using System.Security.Principal;
using Xunit;

namespace Claims.WG.NET
{

    // "http://facebook.com/claims/friendsCount"

    public class FacebookIdentity : GenericIdentity
    {
        public FacebookIdentity(string name, int friendsCount)
            : base(name, "facebook auth")
        {
            // add custom claim
        }

        public int FriendsCount
        {
            get
            {
                return 0;
            }
        }
    }

    public class CustomIdentity_demos
    {
        [Fact]
        public void uses_custom_property_to_access_domain_specific_claims()
        {
        }
    }
}