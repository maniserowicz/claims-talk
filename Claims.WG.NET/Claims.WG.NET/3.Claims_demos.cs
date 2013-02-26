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
        }












        [Fact]
        public void GenericPrincipal_inherits_from_ClaimsPrincipal()
        {
        }















        [Fact]
        public void ClaimsPrincipal_Current_returns_generic_principal()
        {
        }






        [Fact]
        public void identity_is_mapped_to_claims_identity()
        {
        }







        [Fact]
        public void generic_roles_are_translated_to_claims()
        {
        }






        [Fact]
        public void new_role_claims_result_in_new_roles_added_to_generic_principal()
        {
        }







        [Fact]
        public void generic_identity_with_name_implies_authentication()
        {
        }









        [Fact]
        public void claims_provide_information_for_anonymous_identities()
        {
        }







        [Fact]
        public void setting_authentication_type_implies_authentication()
        {
        }








        [Fact]
        public void any_claim_can_be_mapped_to_name___or_roles()
        {

        }









        [Fact]
        public void ClaimsPrincipal_can_be_built_from_multiple_identities()
        {
        }












    }
}