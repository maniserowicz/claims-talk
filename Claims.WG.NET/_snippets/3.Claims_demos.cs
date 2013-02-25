using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using Xunit;
using System.Linq;

namespace Claims.WG.NET
{
    public class Claims_demos
    {
        private void set_generic_principal()
        {
            var id = new GenericIdentity("procent");
            var principal = new GenericPrincipal(id, new[] { "dev", "blogger" });

            Thread.CurrentPrincipal = principal;
        }

        public Claims_demos()
        {
            set_generic_principal();
        }

        [Fact]
        public void GenericIdentity_inherits_from_ClaimsIdentity()
        {
            IIdentity currentIdentity = Thread.CurrentPrincipal.Identity;

            Assert.True(
                currentIdentity is ClaimsIdentity
            );
        }

        [Fact]
        public void GenericPrincipal_inherits_from_ClaimsPrincipal()
        {
            IPrincipal currentPrincipal = Thread.CurrentPrincipal;

            Assert.True(
                currentPrincipal is ClaimsPrincipal
            );
        }

        [Fact]
        public void ClaimsPrincipal_Current_returns_generic_principal()
        {
            ClaimsPrincipal currentClaimsPrincipal = ClaimsPrincipal.Current;
            IPrincipal currentPrincipal = Thread.CurrentPrincipal;

            Assert.Equal(
                currentClaimsPrincipal, currentPrincipal
            );
        }

        [Fact]
        public void identity_is_mapped_to_claims_identity()
        {
            ClaimsPrincipal currentClaimsPrincipal = ClaimsPrincipal.Current;

            Assert.Equal(
                1, currentClaimsPrincipal.Identities.Count()
            );

            ClaimsIdentity claimsIdentity = currentClaimsPrincipal.Identities.Single();

            Assert.Equal(
                "procent", claimsIdentity.Name
            );
            Assert.Equal(
                "procent", claimsIdentity.FindFirst(ClaimTypes.Name).Value
            );
        }

        [Fact]
        public void generic_roles_are_translated_to_claims()
        {
            ClaimsPrincipal currentClaimsPrincipal = ClaimsPrincipal.Current;

            currentClaimsPrincipal.HasClaim(x => x.Type == ClaimTypes.Role);

            IEnumerable<Claim> role_claims = currentClaimsPrincipal.FindAll(ClaimTypes.Role);

            Assert.Equal(2, role_claims.Count());

            Assert.True(
                role_claims.Any(x => x.Value == "dev")
            );

            Assert.True(
                role_claims.Any(x => x.Value == "blogger")
            );
        }

        [Fact]
        public void new_role_claims_result_in_new_roles_added_to_generic_principal()
        {
            ClaimsPrincipal currentClaimsPrincipal = ClaimsPrincipal.Current;
            IPrincipal currentPrincipal = Thread.CurrentPrincipal;

            Assert.False(
                currentPrincipal.IsInRole("admin")
            );

            ClaimsIdentity claimsIdentity = currentClaimsPrincipal.Identities.Single();

            var admin_claim = new Claim(ClaimTypes.Role, "admin");

            claimsIdentity.AddClaim(admin_claim);

            Assert.True(
                currentPrincipal.IsInRole("admin")
            );
        }

        [Fact]
        public void generic_identity_with_name_implies_authentication()
        {
            Assert.True(
                Thread.CurrentPrincipal.Identity.IsAuthenticated
            );
        }

        [Fact]
        public void claims_provide_information_for_anonymous_identities()
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "procent"),
                    new Claim("content_reader", "news"),
                    new Claim("content_reader", "articles"),
                };
            var claimsIdentity = new ClaimsIdentity(claims);
            Assert.False(claimsIdentity.IsAuthenticated);
        }

        [Fact]
        public void setting_authentication_type_implies_authentication()
        {
            var claimsIdentity = new ClaimsIdentity(null, "auth type");

            Assert.True(claimsIdentity.IsAuthenticated);
        }

        [Fact]
        public void any_claim_can_be_mapped_to_name___or_roles()
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "procent"),
                    new Claim(ClaimTypes.Email, "procent@dev.com"),
                };

            var id_with_name_as_name = new ClaimsIdentity(claims);
            Assert.Equal("procent", id_with_name_as_name.Name);

            var id_with_email_as_name = new ClaimsIdentity(claims, "auth type", ClaimTypes.Email, ClaimTypes.Role);
            Assert.Equal("procent@dev.com", id_with_email_as_name.Name);
        }

        [Fact]
        public void ClaimsPrincipal_can_be_build_from_multiple_identities()
        {
            var googleClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "procent"),
                    new Claim(ClaimTypes.Email, "procent@gmail.com"),
                };
            var googleId = new ClaimsIdentity(googleClaims, "google auth");

            var facebookClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "Maciej Aniserowicz"),
                    new Claim(ClaimTypes.Email, "maciej.aniserowicz@facebook.com"),
                    new Claim("http://schemas.facebook.com/claims/friendsCount", "44"),
                };
            var facebookId = new ClaimsIdentity(facebookClaims, "facebook auth");

            var claimsPrincipal = new ClaimsPrincipal(new[] { googleId, facebookId });

            Assert.Equal(2, claimsPrincipal.Identities.Count());

            IEnumerable<Claim> emails = claimsPrincipal.FindAll(ClaimTypes.Email);

            Assert.Equal(2, emails.Count());
            Assert.True(
                emails.Any(x => x.Value == "procent@gmail.com")
            );
            Assert.True(
                emails.Any(x => x.Value == "maciej.aniserowicz@facebook.com")
            );

            Claim fb_friends_count = claimsPrincipal.FindFirst("http://schemas.facebook.com/claims/friendsCount");

            Assert.Equal("44", fb_friends_count.Value);
        }
    }
}