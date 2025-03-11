using System.Collections;
using TimeWaster.Core.Models;
using TimeWaster.Core.Services.UserProcessing;

namespace TimeWaster.Core.Services.IntervalProcessing;

public class IntervalService : IIntervalService
{
    private readonly IIntervalsRepository _intervalsRepository;
    private readonly IUserService _userService;

    public IntervalService(IIntervalsRepository intervalsRepository, IUserService userService)
    {
        _intervalsRepository = intervalsRepository;
        _userService = userService;
    }

    public Result<IEnumerable<Interval>> GetAll()
    {
        return Result<IEnumerable<Interval>>.Success(_intervalsRepository.GetAll());
    }

    public Result<Interval?> Get(Guid id)
    {
        return Result<Interval?>.Success(_intervalsRepository.Get(id));
    }

    public Result<IEnumerable<Interval>> GetByUserId(Guid userId)
    {
        if (_userService.Get(userId).Value is null)
        {
            return Result<IEnumerable<Interval>>.Failure("User not found");
        }

        return Result<IEnumerable<Interval>>.Success(_intervalsRepository.GetByUser(userId));
    }

    public Result<Interval?> Create(Interval interval)
    {
        if (_intervalsRepository.Get(interval.Id) is not null)
        {
            throw new InvalidOperationException("Interval already exists");
        }

        if (_userService.Get(interval.UserId).Value is null)
        {
            return Result<Interval?>.Failure("User not found");
        }

        if (!interval.IsValidTimeBounds())
        {
            return Result<Interval?>.Failure("Invalid time bounds is not valid. Start time later than end time or time is in the future.");
        }

        var intervalsByDate = _intervalsRepository
            .GetByUsersInDate(interval.UserId, DateOnly.FromDateTime(interval.StartTime))
            .ToList();

        if (intervalsByDate.Count == 0)
        {
            return Result<Interval?>.Success(_intervalsRepository.Create(interval));
        }

        return ValidateTimeBounds(intervalsByDate, interval) is { IsValid: false, ErrorMessage: { } errorMessage }
            ? Result<Interval?>.Failure(errorMessage)
            : Result<Interval?>.Success(_intervalsRepository.Create(interval));
    }

    public Result<Interval?> Update(Interval interval)
    {
        if (_userService.Get(interval.UserId).Value is null)
        {
            return Result<Interval?>.Failure("User not found");
        }

        if (_intervalsRepository.Get(interval.Id) is null)
        {
            return Result<Interval?>.Failure("Interval not found");
        }

        var intervalsByDate = _intervalsRepository
            .GetByUsersInDate(interval.UserId, DateOnly.FromDateTime(interval.StartTime))
            .ToList();

        if (intervalsByDate.Count == 1)
        {
            return Result<Interval?>.Success(_intervalsRepository.Update(interval));
        }

        return ValidateTimeBounds(intervalsByDate, interval) is { IsValid: false, ErrorMessage: { } errorMessage }
            ? Result<Interval?>.Failure(errorMessage)
            : Result<Interval?>.Success(_intervalsRepository.Update(interval));
    }

    public Result<Interval?> Open(Guid userId)
    {
        if (_userService.Get(userId).Value is null)
        {
            return Result<Interval?>.Failure("User not found");
        }
        
        return _intervalsRepository.GetOpen(userId) is not null 
            ? Result<Interval?>.Failure("Cannot open interval when other interval is open") 
            : Create(Interval.Create(userId, DateTime.UtcNow));
    }

    public Result<Interval?> Close(Guid userId, string name)
    {
        var intervalToClose = _intervalsRepository.GetOpen(userId);

        if (intervalToClose is null)
        {
            return Result<Interval?>.Failure("Cannot close interval when no open interval");
        }

        intervalToClose.SetEndTime(DateTime.UtcNow);
        intervalToClose.SetName(name);

        return Update(intervalToClose);
    }

    public Result<Interval?> Delete(Guid id)
    {
        if (_intervalsRepository.Get(id) is null)
        {
            return Result<Interval?>.Failure("Interval not found");
        }
        
        _intervalsRepository.Delete(id);
        
        return _intervalsRepository.Get(id) is null 
            ? Result<Interval?>.Success(null) 
            : Result<Interval?>.Failure("Interval delete failed");
    }

    private static TimeBoundsValidateResult ValidateTimeBounds(List<Interval> userIntervals, Interval interval)
    {
        if (userIntervals.FirstOrDefault(existInterval =>
                existInterval.StartTime <= interval.StartTime
                && interval.EndTime <= existInterval.EndTime) is not null)
        {
            return new TimeBoundsValidateResult()
            {
                IsValid = false,
                ErrorMessage = "Interval cannot be inside other interval"
            };
        }

        if (userIntervals.FirstOrDefault(existInterval =>
                interval.StartTime < existInterval.StartTime
                && existInterval.EndTime < interval.EndTime) is not null)
        {
            return new TimeBoundsValidateResult()
            {
                IsValid = false,
                ErrorMessage = "Interval cannot include other interval"
            };
        }

        if (userIntervals.FirstOrDefault(existInterval =>
                existInterval.StartTime < interval.EndTime
                && interval.EndTime < existInterval.EndTime) is not null)
        {
            return new TimeBoundsValidateResult()
            {
                IsValid = false,
                ErrorMessage = "Interval end time cannot be inside other interval"
            };
        }

        if (userIntervals.FirstOrDefault(existInterval =>
                existInterval.StartTime < interval.StartTime
                && interval.StartTime < existInterval.EndTime) is not null)
        {
            return new TimeBoundsValidateResult()
            {
                IsValid = false,
                ErrorMessage = "Interval start time cannot be inside other interval"
            };
        }

        return new TimeBoundsValidateResult() { IsValid = true };
    }

    private class TimeBoundsValidateResult
    {
        public bool IsValid { get; init; }
        public string? ErrorMessage { get; init; }
    }
}