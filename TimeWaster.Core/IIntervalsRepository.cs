using TimeWaster.Core.Models;

namespace TimeWaster.Core;

public interface IIntervalsRepository
{
    IEnumerable<Interval> GetAll();
    Interval? Get(Guid id);
    IEnumerable<Interval> GetByUser(Guid userId);
    Interval? Create(Interval interval);
    Interval? Update(Interval interval);
    void Delete(Guid id);
}