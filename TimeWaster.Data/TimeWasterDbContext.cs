using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TimeWaster.Data.Users;
using TimeWaster.Data.Intervals;

namespace TimeWaster.Data;

public class TimeWasterDbContext : DbContext
{
    public DbSet<IntervalDbModel> Intervals { get; set; }
    public DbSet<UserDbModel> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserDbModelConfiguration());
        modelBuilder.ApplyConfiguration(new IntervalDbModelConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appconfig.json");
        
        var connectionString = configBuilder.Build().GetConnectionString("DefaultConnection");

        base.OnConfiguring(optionsBuilder
            .UseNpgsql(connectionString));
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