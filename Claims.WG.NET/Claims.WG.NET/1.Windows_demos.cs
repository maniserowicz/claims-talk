using System.Security.Principal;
using Xunit;

public class Windows_demos
{
    private dynamic get_current_user()
    {
        var id = WindowsIdentity.GetCurrent();
        var principal = new WindowsPrincipal(id);

        return new
        {
            id,
            principal
        };
    }


    [Fact]
    public void current_windows_user()
    {
    }







    [Fact]
    public void is_in_role_users_string()
    {
    }






    [Fact]
    public void is_in_role_users_sid()
    {
    }
}