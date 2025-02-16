using TimeWaster.Core.Models;
using TimeWaster.Web.Controllers.Intervals.Dto;

namespace TimeWaster.Web.Controllers.Intervals.Extensions;

public static class IntervalExtension
{
    public static IntervalDto ToDto(this Interval interval)
    {
        return new IntervalDto()
        {
            Id = interval.Id,
            Name = interval.Name,
            StartTime = interval.StartTime,
            EndTime = interval.EndTime,
        };
    }
}