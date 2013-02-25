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
        }







        [Fact]
        public void use_id()
        {
        }








        [Fact]
        public void manual_if_authorization()
        {
        }






        [Fact]
        public void manual_demand_authorization()
        {
        }








        [Fact]
        public void declarative_authorization()
        {
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