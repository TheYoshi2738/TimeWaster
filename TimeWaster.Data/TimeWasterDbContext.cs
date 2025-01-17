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
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserDbModelConfiguration());
        modelBuilder.ApplyConfiguration(new IntervalDbModelConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is not IntervalDbModel dbModel) continue;
            dbModel.StartTime = dbModel.StartTime.ToUniversalTime();
            dbModel.EndTime = dbModel.EndTime?.ToUniversalTime();
        }
            
        return base.SaveChanges();
    }
}