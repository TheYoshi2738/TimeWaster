using Microsoft.AspNetCore.Mvc;
using TimeWaster.Core.Models;
using TimeWaster.Core.Services.UserProcessing;
using TimeWaster.Web.Controllers.Users.Dto;
using TimeWaster.Web.Controllers.Users.Extensions;

namespace TimeWaster.Web.Controllers.Users;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetAll()
    {
        var users = _userService.GetAll().Value?.Select(user => user.ToDto());
        return users is null ? NotFound() : Ok(users);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<UserDto?> Get(Guid id)
    {
        var user = _userService.Get(id).Value;

        if (user is null)
        {
            return NotFound("User not found");
        }

        return Ok(user.ToDto());
    }

    [HttpGet("login/{login}")]
    public ActionResult<UserDto?> GetByLogin(string login)
    {
        var user = _userService.GetByLogin(login).Value;

        if (user is null)
        {
            return NotFound("User not found");
        }

        return Ok(user.ToDto());
    }

    [HttpPost("create")]
    public ActionResult<UserDto?> Create([FromBody] UserCreateDto userDto)
    {
        var user = Core.Models.User.Create(userDto.Login, userDto.Name);

        var userCreateResult = _userService.Create(user);

        if (userCreateResult is { IsSuccess: true, Value: { } createdUser })
        {
            return CreatedAtAction(nameof(Get), new { id = createdUser.Id }, createdUser.ToDto());
        }

        switch (userCreateResult.ErrorMessage)
        {
            case "Login is not unique":
                return BadRequest(userCreateResult.ErrorMessage);
            default:
                return StatusCode(500, userCreateResult.ErrorMessage); 
        }
    }

    [HttpPut("update/{id:guid}")]
    public ActionResult<UserDto?> Update(Guid id, [FromBody] UserUpdateDto userDto)
    {
        if (id != userDto.Id)
        {
            return BadRequest("User id mismatch");
        }

        var user = new User(userDto.Id, userDto.Login, userDto.Name, null);
        var userUpdateResult = _userService.Update(user);
        
        if (userUpdateResult is { IsSuccess: true, Value: { } updatedUser })
        {
            return Ok(updatedUser.ToDto());
        }

        switch (userUpdateResult.ErrorMessage)
        {
            case "User not found":
                return NotFound("User not found");
            default: 
                return StatusCode(500, userUpdateResult.ErrorMessage);
        }
    }

    [HttpDelete("delete/{id:guid}")]
    public ActionResult<UserDto?> Delete(Guid id)
    {
        var userDeleteResult = _userService.Delete(id);

        if (userDeleteResult is { IsSuccess: true })
        {
            return NoContent();
        }

        switch (userDeleteResult.ErrorMessage)
        {
            case "User not found":
                return NotFound(userDeleteResult.ErrorMessage);
            case "User delete failed":
                return BadRequest(userDeleteResult.ErrorMessage);
            default:
                return StatusCode(500, userDeleteResult.ErrorMessage);
        }
    }
}