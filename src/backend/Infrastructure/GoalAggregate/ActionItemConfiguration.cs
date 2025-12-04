using InternetSafetyPlan.Domain.GoalAggregate;
using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Infrastructure.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternetSafetyPlan.Infrastructure.GoalAggregate;

public class ActionItemConfiguration : EntityConfiguration<ActionItem>
{
    public override void Configure(EntityTypeBuilder<ActionItem> builder)
    {
        base.Configure(builder);
        builder
            .HasOne<Goal>()
            .WithMany(g => g.ActionItems)
            .HasForeignKey(e => e.GoalId);
        builder
            .OwnsOne(e => e.Name)
            .Property(n => n.Value)
            .HasColumnName("Name")
            .HasMaxLength(ActionItemName.MaxLength);
        builder
            .OwnsOne(e => e.Description)
            .Property(n => n.Value)
            .HasColumnName("Description")
            .HasMaxLength(Description.MaxLength);
        builder
            .OwnsOne(e => e.DueDate)
            .Property(dd => dd.Value)
            .HasColumnName("DueDate");
        builder
            .Property(e => e.IsComplete)
            .HasDefaultValue(false);
    }
}
