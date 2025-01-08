using TimeWaster.Core;
using TimeWaster.Core.Models;

namespace TimeWaster.Data.Intervals.Repositories;

public class IntervalsStaticRepository : IIntervalsRepository
{
    private static List<Interval> _intervals = new List<Interval>();
    // {
    //     new Interval(Guid.NewGuid(),
    //                 "Breakfast",
    //                 DateTime.Parse("2024-12-28T09:00:00Z"),
    //                 DateTime.Parse("2024-12-28T10:00:00Z")),
    //     new Interval(Guid.NewGuid(),
    //                 "homework",
    //                 DateTime.Parse("2024-12-28T10:00:00Z"),
    //                 DateTime.Parse("2024-12-28T12:00:00Z")),
    //     new Interval(Guid.NewGuid(),
    //                 "VideoGames",
    //                 DateTime.Parse("2024-12-28T12:00:00Z"),
    //                 DateTime.Parse("2024-12-28T13:30:00Z")),
    // };

    public IEnumerable<Interval> GetAll()
    {
        return _intervals;
    }

    public Interval? Get(Guid id)
    {
        return _intervals.SingleOrDefault(i => i.Id == id);
    }

    public IEnumerable<Interval> GetByUser(Guid userId)
    {
        return _intervals.Where(i => i.UserId == userId);
    }

    public Interval? Create(Interval interval)
    {
        _intervals.Add(interval);
        return Get(interval.Id);
    }

    public Interval? Update(Interval interval)
    {
        throw new NotImplementedException();
    }

    public void Delete(Guid id)
    {
        var interval = Get(id);

        if (interval is null)
        {
            throw new InvalidOperationException("Interval not found");
        }
        _intervals.Remove(interval);
    }
}