namespace TimeWaster.Web.Controllers.Intervals.Dto;

public class IntervalDto
{
    public required Guid Id { get; set; }
    public string? Name { get; set; }
    public required DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}