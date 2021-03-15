namespace LabCenter.Client.Models
{
    public record ResponseModel<T>(string? Message, T? Data, int Code);
}
