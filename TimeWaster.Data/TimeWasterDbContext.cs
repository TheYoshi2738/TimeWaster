using Microsoft.EntityFrameworkCore;
using TimeWaster.Data.Users;
using TimeWaster.Data.Intervals;

namespace TimeWaster.Data;

public class TimeWasterDbContext : DbContext
{
    public DbSet<IntervalDbModel> Intervals { get; set; }
    public DbSet<UserDbModel> Users { get; set; }

    public TimeWasterDbContext(DbContextOptions<TimeWasterDbContext> options) : base(options)
    {
    }
}