namespace LabCenter.Shared.Models
{
    public record SignInModel(string UserName, string Password, bool RememberMe);
    public record ChangePasswordModel(string OldPaassword, string NewPassword);
    public record UserInfo(string Id, string UserName, string Name, bool Admin);
}
