﻿namespace TimeWaster.Web.Controllers.Intervals.Dto;

public class IntervalDto
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}