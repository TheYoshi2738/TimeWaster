using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TimeWaster.Data.Intervals;

public class IntervalDbModelConfiguration : IEntityTypeConfiguration<IntervalDbModel>
{
    public void Configure(EntityTypeBuilder<IntervalDbModel> builder)
    {
        builder.HasKey(interval => interval.Id);

        builder
            .HasOne(interval => interval.User)
            .WithMany(user => user.Intervals)
            .HasForeignKey(interval => interval.UserId);

        builder.Property(interval => interval.UserId).IsRequired();
        builder.Property(interval => interval.StartTime);
        builder.Property(interval => interval.EndTime);
    }
}