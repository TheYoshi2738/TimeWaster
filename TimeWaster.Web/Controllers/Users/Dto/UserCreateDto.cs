using System.ComponentModel.DataAnnotations;

namespace TimeWaster.Web.Controllers.Users.Dto;

public class UserCreateDto
{
    [Required]
    public required string Login { get; set; }
    public required string Name { get; set; }
}