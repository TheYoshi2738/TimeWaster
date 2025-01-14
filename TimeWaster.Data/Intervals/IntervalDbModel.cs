using System.Reflection.Emit;
using TimeWaster.Core.Models;
using TimeWaster.Data.Users;

namespace TimeWaster.Data.Intervals;

public class IntervalDbModel
{
    public required Guid Id { get; init; }
    public string? Name { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public UserDbModel User { get; set; } = null!;
    public Guid UserId { get; set; }

    public Interval CreateCoreModel()
    {
        return new Interval(
            Id,
            Name,
            StartTime,
            EndTime,
            UserId);
    }
}