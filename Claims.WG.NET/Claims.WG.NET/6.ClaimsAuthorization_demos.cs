using System;
using System.Dynamic;
using System.IdentityModel.Services;
using System.Security;
using System.Security.Claims;
using System.Security.Permissions;
using System.Threading;
using Xunit;
using System.Linq;

namespace Claims.WG.NET
{
    public class CustomClaimsAuthZManager : ClaimsAuthorizationManager
    {
        public override bool CheckAccess(AuthorizationContext context)
        {
            // investigate claim "breakfast tv" with values "appear" and "watch"
            
            return base.CheckAccess(context);
        }
    }

    public class ClaimsAuthorization_demos
    {
        #region preparing principals

        private dynamic _principals = new ExpandoObject();

        public ClaimsAuthorization_demos()
        {
            var kasia_tusk = new ClaimsPrincipal(new FacebookIdentity("Kasia Tusk", 10000000));
            var kasia_translated = FederatedAuthentication.FederationConfiguration
                                                          .IdentityConfiguration.ClaimsAuthenticationManager
                                                          .Authenticate(null, kasia_tusk);

            _principals.kasia = kasia_translated;

            var procent = new ClaimsPrincipal(new FacebookIdentity("Maciej Aniserowicz", 3));
            var procent_translated = FederatedAuthentication.FederationConfiguration
                                                            .IdentityConfiguration.ClaimsAuthenticationManager
                                                            .Authenticate(null, procent);

            _principals.procent = procent_translated;
        }

        #endregion

        [Fact]
        public void only_popular_people_can_appear_in_breakfast_tv()
        {
        }









        [Fact]
        public void everybody_can_watch_breakfast_tv()
        {
        }








        [Fact]
        public void imperative_authorization_checks()
        {
        }




        [ClaimsPrincipalPermission(
            SecurityAction.Demand, Operation = "appear", Resource = "breakfast tv"
        )]
        void appear_in_breakfast_tv()
        {
        }

        [ClaimsPrincipalPermission(
            SecurityAction.Demand, Operation = "watch", Resource = "breakfast tv"
        )]
        void watch_breakfast_tv()
        {
        }
    }
}