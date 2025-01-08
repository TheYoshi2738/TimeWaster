using Microsoft.EntityFrameworkCore;
using TimeWaster.Core;
using TimeWaster.Core.Models;

namespace TimeWaster.Data.Intervals.Repositories;

public class IntervalsInMemoryRepository : IIntervalsRepository
{
    private readonly TimeWasterDbContext _context;

    public IntervalsInMemoryRepository(TimeWasterDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Interval> GetAll()
    {
        return _context.Intervals
            .AsNoTracking()
            .Select(interval => new Interval(
                interval.Id,
                interval.Name,
                interval.StartTime,
                interval.EndTime,
                interval.UserId))
            .AsEnumerable();
    }

    public Interval? Get(Guid id)
    {
        var interval = _context.Intervals
            .AsNoTracking()
            .FirstOrDefault(interval => interval.Id == id);

        return interval is not null
            ? new Interval(interval.Id, interval.Name, interval.StartTime, interval.EndTime, interval.UserId)
            : null;
    }

    public IEnumerable<Interval> GetByUser(Guid userId)
    {
        return _context.Intervals
            .AsNoTracking()
            .Where(interval => interval.UserId == userId)
            .Select(interval => new Interval(
                interval.Id,
                interval.Name,
                interval.StartTime,
                interval.EndTime,
                interval.UserId));
    }

    public Interval? Create(Interval interval)
    {
        var intervalDbModel = new IntervalDbModel()
        {
            Id = interval.Id,
            Name = interval.Name,
            StartTime = interval.StartTime,
            EndTime = interval.EndTime,
            UserId = interval.UserId
        };

        _context.Add(intervalDbModel);
        _context.SaveChanges();
        return Get(interval.Id);
    }

    public Interval? Update(Interval interval)
    {
        var intervalToUpdate = _context.Intervals
                                   .FirstOrDefault(model => model.Id == interval.Id)
                               ?? throw new ArgumentException($"Interval {interval.Id} not found");

        intervalToUpdate.Name = interval.Name;
        intervalToUpdate.StartTime = interval.StartTime;
        intervalToUpdate.EndTime = interval.EndTime;
        intervalToUpdate.UserId = interval.UserId;
        
        _context.SaveChanges();
        return Get(intervalToUpdate.Id);
    }

    public void Delete(Guid id)
    {
        var intervalToDelete = _context.Intervals
                                   .AsNoTracking()
                                   .FirstOrDefault(interval => interval.Id == id)
                               ?? throw new ArgumentException($"Interval {id} does not exist");

        _context.Intervals.Remove(intervalToDelete);

        _context.SaveChanges();
    }
}