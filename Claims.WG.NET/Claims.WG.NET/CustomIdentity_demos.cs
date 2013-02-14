using System.Security.Claims;
using System.Security.Principal;
using Xunit;

namespace Claims.WG.NET
{
    public class FacebookIdentity : GenericIdentity
    {
        public FacebookIdentity(string name, int friendsCount)
            : base(name, "facebook auth")
        {
            var friendsCountClaim = new Claim(
                "http://schemas.facebook.com/claims/friendsCount"
                , friendsCount.ToString()
            );
            AddClaim(friendsCountClaim);
        }

        public int FriendsCount
        {
            get
            {
                return int.Parse(
                    FindFirst("http://schemas.facebook.com/claims/friendsCount").Value
                );
            }
        }
    }

    public class CustomIdentity_demos
    {
        [Fact]
        public void uses_custom_property_to_access_domain_specific_claims()
        {
            var id = new FacebookIdentity("Maciej Aniserowicz", 44);

            Assert.Equal(44, id.FriendsCount);
        }
    }
}