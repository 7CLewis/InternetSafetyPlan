using System.Text.RegularExpressions;
using InternetSafetyPlan.Domain.DeviceAggregate;
using InternetSafetyPlan.Domain.GoalAggregate;
using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Domain.TeamAggregate;
using InternetSafetyPlan.Domain.UltimateGoalAggregate;
using InternetSafetyPlan.Infrastructure.Base;
using InternetSafetyPlan.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternetSafetyPlan.Infrastructure.GoalAggregate;

public class GoalConfiguration : EntityConfiguration<Goal>
{
    public override void Configure(EntityTypeBuilder<Goal> builder)
    {
        base.Configure(builder);
        builder
            .HasOne<UltimateGoal>()
            .WithMany(ug => ug.Goals)
            .HasForeignKey(e => e.UltimateGoalId);
        builder
            .OwnsOne(e => e.Name)
            .Property(n => n.Value)
            .HasColumnName("Name")
            .HasMaxLength(GoalName.MaxLength);
        builder
            .Property(e => e.Category)
            .HasConversion(
                v => StringUtils.AddSpacesBetweenCapitals(v.ToString(), true),
                v => (GoalCategory)Enum.Parse(typeof(GoalCategory), Regex.Replace(v, @"\s+", "")));
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
            .HasMany(e => e.ActionItems)
            .WithOne()
            .HasForeignKey(ai => ai.GoalId);
        builder
            .HasMany(e => e.AffectedDevices)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "GoalDevice",
                j => j
                    .HasOne<Device>()
                    .WithMany()
                    .HasForeignKey("DeviceId")
                    .HasConstraintName("FK_GoalDevice_Device_DeviceId")
                    .OnDelete(DeleteBehavior.NoAction),
                j => j
                    .HasOne<Goal>()
                    .WithMany()
                    .HasForeignKey("GoalId")
                    .HasConstraintName("FK_GoalDevice_Goal_GoalId")
                    .OnDelete(DeleteBehavior.NoAction));
        builder
            .HasMany(e => e.AffectedTeammates)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "GoalTeammate",
                j => j
                    .HasOne<Teammate>()
                    .WithMany()
                    .HasForeignKey("TeammateId")
                    .HasConstraintName("FK_GoalTeammate_Teammates_TeammateId")
                    .OnDelete(DeleteBehavior.NoAction),
                j => j
                    .HasOne<Goal>()
                    .WithMany()
                    .HasForeignKey("GoalId")
                    .HasConstraintName("FK_GoalTeammate_Goal_GoalId")
                    .OnDelete(DeleteBehavior.NoAction));
        builder
            .HasMany(e => e.Tags)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "GoalTag",
                j => j
                    .HasOne<Tag>()
                    .WithMany()
                    .HasForeignKey("TagId")
                    .HasConstraintName("FK_GoalTag_Tags_TagId")
                    .OnDelete(DeleteBehavior.NoAction),
                j => j
                    .HasOne<Goal>()
                    .WithMany()
                    .HasForeignKey("GoalId")
                    .HasConstraintName("FK_GoalTag_Goal_GoalId")
                    .OnDelete(DeleteBehavior.NoAction));
    }
}
