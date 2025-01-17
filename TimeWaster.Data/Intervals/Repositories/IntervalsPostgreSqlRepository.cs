using Microsoft.EntityFrameworkCore;
using TimeWaster.Core;
using TimeWaster.Core.Models;

namespace TimeWaster.Data.Intervals.Repositories;

public class IntervalsPostgreSqlRepository : IIntervalsRepository
{
    private readonly TimeWasterDbContext _context;

    public IntervalsPostgreSqlRepository(TimeWasterDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Interval> GetAll()
    {
        return _context.Intervals
            .AsNoTracking()
            .Select(interval => interval.CreateCoreModel())
            .AsEnumerable();
    }

    public Interval? Get(Guid id)
    {
        return _context.Intervals
            .AsNoTracking()
            .FirstOrDefault(interval => interval.Id == id)
            ?.CreateCoreModel();
    }

    public IEnumerable<Interval> GetByUser(Guid userId)
    {
        return _context.Intervals
            .AsNoTracking()
            .Where(interval => interval.UserId == userId)
            .Select(interval => interval.CreateCoreModel());
    }

    public IEnumerable<Interval> GetByUsersInDate(Guid userId, DateOnly date)
    {
        return _context.Intervals
            .AsNoTracking()
            .Where(interval => interval.UserId == userId && DateOnly.FromDateTime(interval.StartTime) == date)
            .Select(interval => interval.CreateCoreModel());
    }

    public Interval? GetOpen(Guid userId)
    {
        return _context.Intervals
            .AsNoTracking()
            .FirstOrDefault(interval => interval.UserId == userId && !interval.EndTime.HasValue)
            ?.CreateCoreModel();
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