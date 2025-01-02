namespace TimeWaster.Core;

public class Interval
{
    public Guid Id { get; }
    public string? Name { get; private set; }
    public DateTime? StartTime { get; private set; }
    public DateTime? EndTime { get; private set; }

    public Interval()
    {
        Id = Guid.NewGuid();
    }

    public Interval(Guid id, string name, DateTime startTime, DateTime endTime)
    {
        Id = id;
        Name = name;
        StartTime = startTime;
        EndTime = endTime;
    }

    public void SetName(string name)
    {
        Name = name;
    }

    public void SetStartTime(DateTime startTime)
    {
        //должна быть валидация
        StartTime = startTime;
    }

    public void SetEndTime(DateTime endTime)
    {
        //должна быть валидация
        EndTime = endTime;
    }
}