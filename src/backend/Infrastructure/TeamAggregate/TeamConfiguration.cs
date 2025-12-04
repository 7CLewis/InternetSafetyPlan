using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Domain.TeamAggregate;
using InternetSafetyPlan.Infrastructure.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternetSafetyPlan.Infrastructure.TeamAggregate;

public class TeamConfiguration : EntityConfiguration<Team>
{
    public override void Configure(EntityTypeBuilder<Team> builder)
    {
        base.Configure(builder);
        builder
            .OwnsOne(e => e.Name)
            .Property(n => n.Value)
            .HasColumnName("Name")
            .HasMaxLength(TeamName.MaxLength);
        builder
            .OwnsOne(e => e.Description)
            .Property(n => n.Value)
            .HasColumnName("Description")
            .HasMaxLength(Description.MaxLength);
    }
}
