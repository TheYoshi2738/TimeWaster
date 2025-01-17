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

        var intervalsByDate =
            _intervalsRepository.GetByUsersInDate(interval.UserId, DateOnly.FromDateTime(interval.StartTime)).ToList();


        if (intervalsByDate.Count == 0) return _intervalsRepository.Create(interval);

        if (intervalsByDate.FirstOrDefault(oldInterval =>
                oldInterval.StartTime < interval.StartTime
                && oldInterval.EndTime > interval.StartTime) is not null)
        {
            return null;
        }

        if (intervalsByDate.FirstOrDefault(oldInterval =>
                oldInterval.StartTime < interval.EndTime
                && oldInterval.EndTime > interval.EndTime) is not null)
        {
            return null;
        }

        return _intervalsRepository.Create(interval);
    }

    public Interval? Update(Interval interval)
    {
        var intervalToUpdate = _intervalsRepository.Get(interval.Id);

        return intervalToUpdate is null ? null : _intervalsRepository.Update(interval);
    }

    public Interval? Open(Guid userId)
    {
        if (_intervalsRepository.GetOpen(userId) is not null)
        {
            return null;
        }
        
        return Create(Interval.Create(userId, DateTime.UtcNow));
    }

    public Interval? Close(Guid userId, string name)
    {
        var intervalToUpdate = _intervalsRepository.GetOpen(userId);

        if (intervalToUpdate is null)
        {
            return null;
        }

        intervalToUpdate.SetEndTime(DateTime.UtcNow);
        intervalToUpdate.SetName(name);

        return _intervalsRepository.Update(intervalToUpdate);
    }

    public void Delete(Guid id)
    {
        _intervalsRepository.Delete(id);
    }
}