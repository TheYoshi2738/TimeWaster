namespace TimeWaster.Web.Controllers.Intervals.Dto;

public class IntervalCreateDto
{
    public string? Name { get; set; }
    public required DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}