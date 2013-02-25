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
        var user = get_current_user();

        Assert.Equal("p-win8\\procent", user.id.Name);
    }

    [Fact]
    public void is_in_role_users_string()
    {
        var user = get_current_user();

        // culture-specific
        bool isUser = user.principal.IsInRole("Builtin\\Users");

        Assert.True(isUser);
    }

    [Fact]
    public void is_in_role_users_sid()
    {
        var user = get_current_user();

        var builtinUsers = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);

        bool isUser = user.principal.IsInRole(builtinUsers);

        Assert.True(isUser);
    }
}