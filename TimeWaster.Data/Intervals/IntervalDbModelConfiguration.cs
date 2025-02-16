using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TimeWaster.Data.Intervals;

public class IntervalDbModelConfiguration : IEntityTypeConfiguration<IntervalDbModel>
{
    public void Configure(EntityTypeBuilder<IntervalDbModel> builder)
    {
        builder.ToTable("intervals");
        
        builder.HasKey(interval => interval.Id);

        builder
            .HasOne(interval => interval.User)
            .WithMany(user => user.Intervals)
            .HasForeignKey(interval => interval.UserId);

        builder.Property(interval => interval.Id).HasColumnName("id");
        builder.Property(interval => interval.Name).HasColumnName("name");
        builder.Property(interval => interval.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(interval => interval.StartTime).HasColumnName("start_time");
        builder.Property(interval => interval.EndTime).HasColumnName("end_time");
    }
}