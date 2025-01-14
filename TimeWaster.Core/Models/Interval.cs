using System.Dynamic;

namespace TimeWaster.Core.Models;

public class Interval
{
    public Guid Id { get; }
    public string? Name { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime? EndTime { get; private set; }
    public Guid UserId { get; private set; }

    public static Interval Create(Guid userId, DateTime startTime, string? name = null, DateTime? endTime = null)
    {
        var interval = new Interval(userId)
        {
            StartTime = startTime,
            Name = name
        };

        if (endTime.HasValue)
        {
            interval.SetEndTime(endTime.Value);
        }
        
        return interval;
    }
    
    private Interval(Guid userId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
    }
    
    private Interval(Guid id, Guid userId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
    }

    public Interval(Guid id, string? name, DateTime startTime, DateTime? endTime, Guid userId)
    {
        Id = id;
        Name = name;
        StartTime = startTime;
        UserId = userId;
        
        if (endTime.HasValue)
        {
            SetEndTime(endTime.Value);
        }
    }

    public void SetName(string name)
    {
        Name = name;
    }

    public void SetStartTime(DateTime startTime)
    {
        if (startTime > DateTime.Now)
            throw new ArgumentException("Start time cannot be later than now.");
        
        StartTime = startTime;
    }

    public void SetEndTime(DateTime endTime)
    {
        //от exception'ов надо избавляться, все-таки это тумач
        if (endTime < StartTime)
            throw new ArgumentException("End time cannot be earlier than start time.");
        
        EndTime = endTime;
    }
}