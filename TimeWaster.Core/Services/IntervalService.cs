using TimeWaster.Core.Models;

namespace TimeWaster.Core.Services;

public class IntervalService
{
    private readonly IIntervalsRepository _intervalsRepository;

    public IntervalService(IIntervalsRepository intervalsRepository)
    {
        _intervalsRepository = intervalsRepository;
    }

    public IEnumerable<Interval> GetAll()
    {
        return _intervalsRepository.GetAll();
    }

    public Interval? Get(Guid id)
    {
        return _intervalsRepository.Get(id);
    }

    public IEnumerable<Interval>? GetByUserId(Guid userId)
    {
        return _intervalsRepository.GetByUser(userId);
    }

    public Interval? Create(Interval interval)
    {
        if (_intervalsRepository.Get(interval.Id) != null)
        {
            throw new InvalidOperationException("Interval already exists");
        }
        
        return _intervalsRepository.Create(interval);
    }

    public Interval? Update(Interval interval)
    {
        var intervalToUpdate = _intervalsRepository.Get(interval.Id);

        return intervalToUpdate is null ? null : _intervalsRepository.Update(interval);
    }

    public void Delete(Guid id)
    {
        _intervalsRepository.Delete(id);
    }
}