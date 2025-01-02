using TimeWaster.Core;

namespace TimeWaster.Data;

public class IntervalsStaticRepository : IIntervalRepository
{
    private static List<Interval> _intervals = new List<Interval>()
    {
        new Interval(Guid.NewGuid(),
                    "Breakfast",
                    DateTime.Parse("2024-12-28T09:00:00Z"),
                    DateTime.Parse("2024-12-28T10:00:00Z")),
        new Interval(Guid.NewGuid(),
                    "homework",
                    DateTime.Parse("2024-12-28T10:00:00Z"),
                    DateTime.Parse("2024-12-28T12:00:00Z")),
        new Interval(Guid.NewGuid(),
                    "VideoGames",
                    DateTime.Parse("2024-12-28T12:00:00Z"),
                    DateTime.Parse("2024-12-28T13:30:00Z")),
    };

    public IEnumerable<Interval> GetAll()
    {
        return _intervals;
    }

    public Interval? Get(Guid id)
    {
        return _intervals.SingleOrDefault(i => i.Id == id);
    }

    public void Create(Interval interval)
    {
        _intervals.Add(interval);
    }

    public Interval? Delete(Guid id)
    {
        var interval = Get(id);
        
        if (interval is null)
            return null;
        
        return _intervals.Remove(interval) ? interval : null;
    }
}