using TimeWaster.Core.Models;

namespace TimeWaster.Core.Services.IntervalProcessing;

public interface IIntervalService
{
    public IEnumerable<Interval> GetAll();
    public Interval? Get(Guid id);
    public IEnumerable<Interval> GetByUserId(Guid id);
    public Interval? Create(Interval interval);
    public Interval? Update(Interval interval);
    public Interval? Open(Guid userId);
    public Interval? Close(Guid userId, string name);
    public void Delete(Guid id);
    
}