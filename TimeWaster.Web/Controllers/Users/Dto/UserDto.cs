namespace TimeWaster.Web.Controllers.Users.Dto;

public class UserDto
{
    public required Guid Id { get; set; }
    public required string Login { get; set; }
    public required string Name { get;  set; }
}