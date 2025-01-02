using Microsoft.AspNetCore.Mvc;
using TimeWaster.Core;
using TimeWaster.Web.Controllers.Intervals.Dto;

namespace TimeWaster.Web.Controllers.Intervals;

[ApiController]
[Route("intervals")]
public class IntervalController : ControllerBase
{
    private IIntervalRepository _repository;
    
    public IntervalController(IIntervalRepository repository)
    {
        _repository = repository;
    }
    
    [HttpGet("")]
    public IEnumerable<Interval> GetAll()
    {
        return _repository.GetAll();
    }
    
    [HttpGet("{id:guid}")]
    public Interval? Get(Guid id)
    {
        return _repository.Get(id);
    }
    
    [HttpPost("create")]
    public Interval? Create(IntervalDto intervalDto)
    {
        var interval = new Interval();
        interval.SetName(intervalDto.Name);
        _repository.Create(interval);
        return interval;
    }
    
    [HttpDelete("delete/{id:guid}")]
    public Interval? Delete(Guid id)
    {
        var interval = _repository.Delete(id);
        return interval;
    }
}