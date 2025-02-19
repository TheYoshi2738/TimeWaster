using TimeWaster.Core.Models;

namespace TimeWaster.Core.Services.IntervalProcessing;

public interface IIntervalService
{
    public Result<IEnumerable<Interval>> GetAll();
    public Result<Interval?> Get(Guid id);
    public Result<IEnumerable<Interval>> GetByUserId(Guid id);
    public Result<Interval?> Create(Interval interval);
    public Result<Interval?> Update(Interval interval);
    public Result<Interval?> Open(Guid userId);
    public Result<Interval?> Close(Guid userId, string name);
    public Result<Interval?> Delete(Guid id);
    
}