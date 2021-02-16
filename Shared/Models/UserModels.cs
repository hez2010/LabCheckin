namespace LabCheckin.Shared.Models
{
    public record SignInModel(string UserName, string Password, bool RememberMe);
    public record UserInfo(string Id, string UserName, string Name);
}
