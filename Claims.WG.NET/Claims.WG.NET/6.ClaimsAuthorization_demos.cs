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
            if (context.Resource.Any(x => x.Type == ClaimTypes.Name && x.Value == "breakfast tv"))
            {
                if (context.Action.Any(x => x.Type == ClaimTypes.Name && x.Value == "appear"))
                {
                    bool isPopular = context.Principal.HasClaim(CustomClaims.IsPopular, bool.TrueString);

                    return isPopular;
                }
                if (context.Action.Any(x => x.Value == "watch"))
                {
                    return true;
                }
            }
            
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
            Thread.CurrentPrincipal = _principals.kasia;

            Assert.DoesNotThrow(
                () => appear_in_breakfast_tv()
            );

            Thread.CurrentPrincipal = _principals.procent;

            Assert.Throws<SecurityException>(
                () => appear_in_breakfast_tv()
            );
        }

        [Fact]
        public void everybody_can_watch_breakfast_tv()
        {
            Thread.CurrentPrincipal = _principals.kasia;

            Assert.DoesNotThrow(
                () => watch_breakfast_tv()
            );

            Thread.CurrentPrincipal = _principals.procent;

            Assert.DoesNotThrow(
                () => watch_breakfast_tv()
            );
        }

        [Fact]
        public void imperative_authorization_checks()
        {
            var authZManager = FederatedAuthentication.FederationConfiguration.IdentityConfiguration.ClaimsAuthorizationManager;
            var kasia_authZ_context = new AuthorizationContext(_principals.kasia, "breakfast tv", "appear");
            bool kasia_can_appear_in_breakfast_tv = authZManager.CheckAccess(kasia_authZ_context);

            Assert.True(kasia_can_appear_in_breakfast_tv);

            var procent_authZ_context = new AuthorizationContext(_principals.procent, "breakfast tv", "appear");
            bool procent_can_appear_in_breakfast_tv = authZManager.CheckAccess(procent_authZ_context);

            Assert.False(procent_can_appear_in_breakfast_tv);
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