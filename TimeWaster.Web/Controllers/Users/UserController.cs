using Microsoft.AspNetCore.Mvc;
using TimeWaster.Core.Models;
using TimeWaster.Core.Services;
using TimeWaster.Web.Controllers.Users.Dto;
using TimeWaster.Web.Controllers.Users.Extensions;

namespace TimeWaster.Web.Controllers.Users;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetAll()
    {
        return Ok(_userService.GetAll().Select(user => user.ToDto()));
    }

    [HttpGet("{id:guid}")]
    public ActionResult<UserDto?> Get(Guid id)
    {
        var user = _userService.Get(id);

        if (user is null)
        {
            return NotFound("User not found");
        }

        return Ok(user.ToDto());
    }

    [HttpGet("login/{login}")]
    public ActionResult<UserDto?> GetByLogin(string login)
    {
        var user = _userService.GetByLogin(login);

        if (user is null)
        {
            return NotFound("User not found");
        }

        return Ok(user.ToDto());
    }

    [HttpPost("create")]
    public ActionResult<UserDto?> Create([FromBody] UserCreateDto userDto)
    {
        var newUser = Core.Models.User.Create(userDto.Login, userDto.Name);

        if (!_userService.CheckUniqueLogin(newUser.Login))
        {
            return Conflict("Login is not unique");
        }

        var createdUser = _userService.Create(newUser);

        if (createdUser is null)
        {
            return StatusCode(500, "User creation failed");
        }

        return CreatedAtAction(nameof(Get), new { id = createdUser.Id }, createdUser.ToDto());
    }

    [HttpPut("update/{id:guid}")]
    public ActionResult<UserDto?> Update(Guid id, [FromBody] UserUpdateDto userDto)
    {
        if (id != userDto.Id)
        {
            return BadRequest("User id mismatch");
        }

        var userToUpdate = _userService.Get(id);

        if (userToUpdate is null)
        {
            return NotFound("User not found");
        }

        var user = new User(userDto.Id, userDto.Login, userDto.Name, null);
        var updatedUser = _userService.Update(user);

        if (updatedUser is null)
        {
            return StatusCode(500, "User update failed");
        }

        return Ok(updatedUser.ToDto());
    }

    [HttpDelete("delete/{id:guid}")]
    public ActionResult<UserDto?> Delete(Guid id)
    {
        var userToDelete = _userService.Get(id);

        if (userToDelete is null)
        {
            return NotFound("User not found");
        }

        _userService.Delete(id);

        if (_userService.Get(id) is not null)
        {
            return StatusCode(500, "User delete failed");
        }

        return Ok();
    }
}