using Microsoft.AspNetCore.Mvc;
using TimeWaster.Core.Services;
using TimeWaster.Core.Models;
using TimeWaster.Web.Controllers.Intervals.Dto;

namespace TimeWaster.Web.Controllers.Intervals;

[ApiController]
[Route("intervals")]
public class IntervalController : ControllerBase
{
    private readonly IntervalService _intervalService;
    private readonly UserService _userService;

    public IntervalController(IntervalService intervalService, UserService userService)
    {
        _intervalService = intervalService;
        _userService = userService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Interval>> GetAll()
    {
        var intervals = _intervalService.GetAll();
        return Ok(intervals);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<Interval?> Get(Guid id)
    {
        var interval = _intervalService.Get(id);
        if (interval is null)
        {
            return NotFound("Interval not found");
        }

        return Ok(interval);
    }

    [HttpPost("create")]
    public ActionResult<Interval?> Create([FromBody] IntervalCreateDto intervalDto, Guid userId)
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

        return CreatedAtAction(nameof(Get), new { id = createdInterval.Id }, createdInterval);
    }

    [HttpPut("update/{id:guid}")]
    public ActionResult<Interval?> Update(Guid id, Guid userId, [FromBody] IntervalUpdateDto intervalDto)
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

        return Ok(updatedInterval);
    }

    [HttpDelete("delete/{id:guid}")]
    public ActionResult<Interval?> Delete(Guid id)
    {
        var intervalToDelete = _intervalService.Get(id);

        if (intervalToDelete is null)
        {
            return NotFound("Interval not found");
        }

        return Ok(intervalToDelete);
    }

    [HttpPost("open")]
    public ActionResult<Interval?> Open(Guid userId)
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

        return CreatedAtAction(nameof(Get), new { id = interval.Id }, interval);
    }

    [HttpPost("close")]
    public ActionResult<Interval?> Close(Guid userId, string name)
    {
        var closedInterval = _intervalService.Close(userId, name);

        if (closedInterval is null)
        {
            return StatusCode(500, "Interval closed failed");
        }

        return Ok(closedInterval);
    }
}