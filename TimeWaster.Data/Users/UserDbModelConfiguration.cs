using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeWaster.Core.Models;
using TimeWaster.Data.Users.Repositories;

namespace TimeWaster.Data.Users;

public class UserDbModelConfiguration : IEntityTypeConfiguration<UserDbModel>
{
    public void Configure(EntityTypeBuilder<UserDbModel> builder)
    {
        builder.ToTable("users");
        
        builder.HasKey(user => user.Id);

        builder
            .HasMany(user => user.Intervals)
            .WithOne(interval => interval.User)
            .HasForeignKey(interval => interval.UserId);

        builder.Property(user => user.Id).HasColumnName("id");
        builder.Property(user => user.Login).HasColumnName("login");
        builder.Property(user => user.Name).HasColumnName("name");
    }
}