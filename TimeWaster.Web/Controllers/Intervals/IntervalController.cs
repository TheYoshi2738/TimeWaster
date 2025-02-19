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

    public IntervalController(IIntervalService intervalService)
    {
        _intervalService = intervalService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<IntervalDto>> GetAll()
    {
        return _intervalService.GetAll().Value is { } intervals
            ? Ok(intervals.Select(interval => interval.ToDto()))
            : NotFound();
    }

    [HttpGet("{id:guid}")]
    public ActionResult<IntervalDto?> Get(Guid id)
    {
        return _intervalService.Get(id).Value is { } interval
            ? Ok(interval.ToDto())
            : NotFound();
    }

    [HttpGet("user/{userId:guid}")]
    public ActionResult<IEnumerable<IntervalDto>> GetByUserId(Guid userId)
    {
        var result = _intervalService.GetByUserId(userId);

        return result is { IsSuccess: true, Value: { } intervals }
            ? Ok(intervals.Select(i => i.ToDto()))
            : BadRequest(result.ErrorMessage);
    }

    [HttpPost("create")]
    public ActionResult<IntervalDto?> Create([FromBody] IntervalCreateDto intervalDto)
    {
        var intervalToCreate = Interval.Create(
            intervalDto.UserId,
            intervalDto.StartTime,
            intervalDto.Name,
            intervalDto.EndTime);

        var createResult = _intervalService.Create(intervalToCreate);

        return createResult is { IsSuccess: true, Value: { } interval }
            ? CreatedAtAction(nameof(Get), new { id = interval.Id }, interval.ToDto())
            : BadRequest(createResult.ErrorMessage);
    }

    [HttpPut("update/{id:guid}")]
    public ActionResult<IntervalDto?> Update(Guid id, [FromBody] IntervalUpdateDto intervalDto)
    {
        if (id != intervalDto.Id)
        {
            return BadRequest("Interval id mismatch");
        }

        var interval = new Interval(id, intervalDto.Name, intervalDto.StartTime, intervalDto.EndTime,
            intervalDto.UserId);

        var updateResult = _intervalService.Update(interval);

        return updateResult is { IsSuccess: true, Value: { } updatedInterval }
            ? Ok(updatedInterval.ToDto())
            : BadRequest(updateResult.ErrorMessage);
    }

    [HttpDelete("delete/{id:guid}")]
    public ActionResult<IntervalDto?> Delete(Guid id)
    {
        var deleteResult = _intervalService.Delete(id);
        
        return deleteResult is {IsSuccess: true}
            ? NoContent()
            : StatusCode(500, deleteResult.ErrorMessage);
    }

    [HttpPost("open")]
    public ActionResult<IntervalDto?> Open(Guid userId)
    {
        var openResult = _intervalService.Open(userId);

        return openResult is { IsSuccess: true, Value: { } interval }
            ? CreatedAtAction(nameof(Get), new { id = interval.Id }, interval.ToDto())
            : BadRequest(openResult.ErrorMessage);
    }

    [HttpPost("close")]
    public ActionResult<IntervalDto?> Close(Guid userId, string name)
    {
        var closeResult = _intervalService.Close(userId, name);
        
        return closeResult is { IsSuccess: true, Value: { } interval }
            ? Ok(interval.ToDto())
            : BadRequest(closeResult.ErrorMessage);
    }
}