using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Domain.TeamAggregate;
using InternetSafetyPlan.Domain.UltimateGoalAggregate;
using InternetSafetyPlan.Infrastructure.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternetSafetyPlan.Infrastructure.UltimateGoalAggregate;

public class UltimateGoalsConfiguration : EntityConfiguration<UltimateGoal>
{
    public override void Configure(EntityTypeBuilder<UltimateGoal> builder)
    {
        base.Configure(builder);
        builder
            .HasOne<Team>()
            .WithMany(t => t.UltimateGoals)
            .HasForeignKey(e => e.TeamId);
        builder
            .OwnsOne(e => e.Name)
            .Property(n => n.Value)
            .HasColumnName("Name")
            .HasMaxLength(UltimateGoalName.MaxLength);
        builder
            .OwnsOne(e => e.Description)
            .Property(n => n.Value)
            .HasColumnName("Description")
            .HasMaxLength(Description.MaxLength);
        builder
            .HasMany(e => e.Goals)
            .WithOne()
            .HasForeignKey(g => g.UltimateGoalId);
    }
}

