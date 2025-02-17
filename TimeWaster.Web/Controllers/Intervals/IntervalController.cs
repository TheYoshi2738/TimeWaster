using Microsoft.AspNetCore.Mvc;
using TimeWaster.Core.Models;
using TimeWaster.Core.Services.IntervalProcessing;
using TimeWaster.Core.Services.UserProcessing;
using TimeWaster.Web.Controllers.Intervals.Dto;
using TimeWaster.Web.Controllers.Intervals.Extensions;

namespace TimeWaster.Web.Controllers.Intervals;

[ApiController]
[Route("intervals")]
public class IntervalController : ControllerBase
{
    private readonly IIntervalService _intervalService;
    private readonly UserService _userService;

    public IntervalController(IIntervalService intervalService, UserService userService)
    {
        _intervalService = intervalService;
        _userService = userService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<IntervalDto>> GetAll()
    {
        var intervals = _intervalService.GetAll();
        return Ok(intervals);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<IntervalDto?> Get(Guid id)
    {
        var interval = _intervalService.Get(id);
        if (interval is null)
        {
            return NotFound("Interval not found");
        }

        return Ok(interval.ToDto());
    }

    [HttpGet("user/{userId:guid}")]
    public ActionResult<IEnumerable<IntervalDto>> GetByUserId(Guid userId)
    {
        var interval = _intervalService.GetByUserId(userId);
        return Ok(interval.Select(i => i.ToDto()));
    }

    [HttpPost("create")]
    public ActionResult<IntervalDto?> Create([FromBody] IntervalCreateDto intervalDto, Guid userId)
    {
        if (_userService.Get(userId) is null)
        {
            return BadRequest("User not found");
        }

        var interval = Interval.Create(userId, intervalDto.StartTime, intervalDto.Name, intervalDto.EndTime);

        var createdInterval = _intervalService.Create(interval);

        if (createdInterval is null)
        {
            return StatusCode(500, "Interval creation failed");
        }

        return CreatedAtAction(nameof(Get), new { id = createdInterval.Id }, createdInterval.ToDto());
    }

    [HttpPut("update/{id:guid}")]
    public ActionResult<IntervalDto?> Update(Guid id, Guid userId, [FromBody] IntervalUpdateDto intervalDto)
    {
        if (id != intervalDto.Id)
        {
            return BadRequest("Interval id mismatch");
        }

        var intervalToUpdate = _intervalService.Get(id);

        if (intervalToUpdate is null)
        {
            return NotFound("Interval not found");
        }

        if (intervalToUpdate.UserId != userId)
        {
            return BadRequest("User id mismatch");
        }

        var interval = new Interval(id, intervalDto.Name, intervalDto.StartTime, intervalDto.EndTime, userId);
        var updatedInterval = _intervalService.Update(interval);

        if (updatedInterval is null)
        {
            return StatusCode(500, "Interval creation failed");
        }

        return Ok(updatedInterval.ToDto());
    }

    [HttpDelete("delete/{id:guid}")]
    public ActionResult<IntervalDto?> Delete(Guid id)
    {
        var intervalToDelete = _intervalService.Get(id);

        if (intervalToDelete is null)
        {
            return NotFound("Interval not found");
        }

        return Ok(intervalToDelete.ToDto());
    }

    [HttpPost("open")]
    public ActionResult<IntervalDto?> Open(Guid userId)
    {
        if (_userService.Get(userId) is null)
        {
            return BadRequest("User not found");
        }

        var interval = _intervalService.Open(userId);

        if (interval is null)
        {
            return StatusCode(500, "Interval open failed");
        }

        return CreatedAtAction(nameof(Get), new { id = interval.Id }, interval.ToDto());
    }

    [HttpPost("close")]
    public ActionResult<IntervalDto?> Close(Guid userId, string name)
    {
        var closedInterval = _intervalService.Close(userId, name);

        if (closedInterval is null)
        {
            return StatusCode(500, "Interval closed failed");
        }

        return Ok(closedInterval.ToDto());
    }
}