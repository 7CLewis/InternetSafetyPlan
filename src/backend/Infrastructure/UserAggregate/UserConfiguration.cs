using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Domain.TeamAggregate;
using InternetSafetyPlan.Domain.UserAggregate;
using InternetSafetyPlan.Infrastructure.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternetSafetyPlan.Infrastructure.UserAggregate;

public class UserConfiguration : EntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);
        builder
            .OwnsOne(e => e.Email)
            .Property(e => e.Value)
            .HasColumnName("Email")
            .HasMaxLength(Email.MaxLength);
        builder
            .HasOne<Team>()
            .WithMany(t => t.Users)
            .HasForeignKey(e => e.TeamId)
            .IsRequired(false);
    }
}
