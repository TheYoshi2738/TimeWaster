using TimeWaster.Core.Models;

namespace TimeWaster.Core;

public interface IIntervalsRepository
{
    IEnumerable<Interval> GetAll();
    Interval? Get(Guid id);
    IEnumerable<Interval> GetByUser(Guid userId);
    IEnumerable<Interval> GetByUsersInDate(Guid userId, DateOnly date);
    Interval? GetOpen(Guid userId);
    Interval? Create(Interval interval);
    Interval? Update(Interval interval);
    void Delete(Guid id);
}