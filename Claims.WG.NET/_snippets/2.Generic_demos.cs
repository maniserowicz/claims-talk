using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;
using Xunit;

namespace Claims.WG.NET
{
    public class Generic_demos
    {
        private void set_principal()
        {
            var id = new GenericIdentity("procent");
            var principal = new GenericPrincipal(id, new [] { "dev", "blogger" });

            Thread.CurrentPrincipal = principal;
        }

        public Generic_demos()
        {
            set_principal();
        }

        [Fact]
        public void generic_identity_with_name_implies_authentication()
        {
            Assert.True(
                Thread.CurrentPrincipal.Identity.IsAuthenticated
            );
        }

        [Fact]
        public void use_id()
        {
            string name = Thread.CurrentPrincipal.Identity.Name;

            Assert.Equal("procent", name);
        }

        [Fact]
        public void manual_if_authorization()
        {
            var current = Thread.CurrentPrincipal;

            Assert.True(current.IsInRole("dev"));
            Assert.True(current.IsInRole("blogger"));

            Assert.False(current.IsInRole("admin"));
        }

        [Fact]
        public void manual_demand_authorization()
        {
            Assert.DoesNotThrow(
                () => new PrincipalPermission("procent", null).Demand()
            );

            Assert.DoesNotThrow(
                () => new PrincipalPermission(null, "dev").Demand()
            );

            Assert.DoesNotThrow(
                () => new PrincipalPermission(null, "blogger").Demand()
            );

            Assert.Throws<SecurityException>(
                () => new PrincipalPermission(null, "admin").Demand()
            );
        }

        [Fact]
        public void declarative_authorization()
        {
            Assert.DoesNotThrow(
                () => method_for_procent()
            );

            Assert.DoesNotThrow(
                () => method_for_dev()
            );

            Assert.Throws<SecurityException>(
                () => method_for_admin()
            );

            Assert.DoesNotThrow(
                () => method_for_dev_or_admin()
            );
        }

        [PrincipalPermission(SecurityAction.Demand, Name="procent")]
        private void method_for_procent() { }

        [PrincipalPermission(SecurityAction.Demand, Role= "dev")]
        private void method_for_dev() { }

        [PrincipalPermission(SecurityAction.Demand, Role= "admin")]
        private void method_for_admin() { }


        [PrincipalPermission(SecurityAction.Demand, Role = "dev")]
        [PrincipalPermission(SecurityAction.Demand, Role = "admin")]
        private void method_for_dev_or_admin() { }
    }
}