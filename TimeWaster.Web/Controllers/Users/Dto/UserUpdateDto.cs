namespace TimeWaster.Web.Controllers.Users.Dto;

public class UserUpdateDto
{
    public required Guid Id { get; set; }
    public required string Login { get; set; }
    public required string Name { get; set; }
}