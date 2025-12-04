using InternetSafetyPlan.Domain.TeamAggregate;
using InternetSafetyPlan.Domain.UserAggregate;
using InternetSafetyPlan.Infrastructure.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternetSafetyPlan.Infrastructure.TeamAggregate;

public class TeammateConfiguration : EntityConfiguration<Teammate>
{
    public override void Configure(EntityTypeBuilder<Teammate> builder)
    {
        base.Configure(builder);
        builder
            .HasOne<Team>()
            .WithMany(t => t.Teammates)
            .HasForeignKey(e => e.TeamId);
        builder
            .HasOne<User>()
            .WithOne()
            .HasForeignKey<Teammate>(e => e.UserId)
            .IsRequired(false);
        builder
            .OwnsOne(e => e.Name)
            .Property(n => n.Value)
            .HasColumnName("Name")
            .HasMaxLength(TeammateName.MaxLength);
        builder
            .OwnsOne(e => e.DateOfBirth)
            .Property(n => n.Value)
            .HasColumnName("DateOfBirth");
    }
}
