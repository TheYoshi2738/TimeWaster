using TimeWaster.Core.Models;
using TimeWaster.Web.Controllers.Users.Dto;

namespace TimeWaster.Web.Controllers.Users.Extensions;

public static class UserExtension
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto()
        {
            Id = user.Id,
            Name = user.Name,
            Login = user.Login,
        };
    }
}