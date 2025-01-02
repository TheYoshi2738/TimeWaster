namespace TimeWaster.Core;

public interface IIntervalRepository
{
    IEnumerable<Interval> GetAll();
    Interval? Get(Guid id);
    void Create(Interval interval);
    Interval? Delete(Guid id);
}