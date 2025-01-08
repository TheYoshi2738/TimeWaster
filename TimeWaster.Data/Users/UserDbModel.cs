using System.ComponentModel.Design;
using TimeWaster.Data.Intervals;

namespace TimeWaster.Data.Users;

public class UserDbModel
{
    public required Guid Id { get; init; }
    public required string Login { get; set; }
    public required string Name { get; set; }
    public List<IntervalDbModel>? Intervals { get; }
}