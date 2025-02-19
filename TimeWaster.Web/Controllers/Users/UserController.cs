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
        if (_userService.GetAll().Value is { } users)
        {
            return Ok(users.Select(u => u.ToDto()));
        }

        return NotFound();
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

        var createResult = _userService.Create(user);

        if (createResult is { IsSuccess: true, Value: { } createdUser })
        {
            return CreatedAtAction(nameof(Get), new { id = createdUser.Id }, createdUser.ToDto());
        }

        switch (createResult.ErrorMessage)
        {
            case "Login is not unique":
                return BadRequest(createResult.ErrorMessage);
            default:
                return StatusCode(500, createResult.ErrorMessage); 
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
        var updateResult = _userService.Update(user);
        
        if (updateResult is { IsSuccess: true, Value: { } updatedUser })
        {
            return Ok(updatedUser.ToDto());
        }

        switch (updateResult.ErrorMessage)
        {
            case "User not found":
                return NotFound("User not found");
            default: 
                return StatusCode(500, updateResult.ErrorMessage);
        }
    }

    [HttpDelete("delete/{id:guid}")]
    public ActionResult<UserDto?> Delete(Guid id)
    {
        var deleteResult = _userService.Delete(id);

        if (deleteResult is { IsSuccess: true })
        {
            return NoContent();
        }

        switch (deleteResult.ErrorMessage)
        {
            case "User not found":
                return NotFound(deleteResult.ErrorMessage);
            case "User delete failed":
                return BadRequest(deleteResult.ErrorMessage);
            default:
                return StatusCode(500, deleteResult.ErrorMessage);
        }
    }
}