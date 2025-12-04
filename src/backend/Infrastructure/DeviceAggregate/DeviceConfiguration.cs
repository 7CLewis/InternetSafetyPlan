using System.Text.RegularExpressions;
using InternetSafetyPlan.Domain.DeviceAggregate;
using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Domain.TeamAggregate;
using InternetSafetyPlan.Infrastructure.Base;
using InternetSafetyPlan.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternetSafetyPlan.Infrastructure.DeviceAggregate;

public class DeviceConfiguration : EntityConfiguration<Device>
{
    public override void Configure(EntityTypeBuilder<Device> builder)
    {
        base.Configure(builder);
        builder
            .HasOne<Team>()
            .WithMany()
            .HasForeignKey(e => e.TeamId);
        builder
            .OwnsOne(e => e.Name)
            .Property(n => n.Value)
            .HasColumnName("Name")
            .HasMaxLength(ManufacturerName.MaxLength);
        builder
            .OwnsOne(e => e.Nickname)
            .Property(n => n.Value)
            .HasColumnName("Nickname")
            .HasMaxLength(Nickname.MaxLength);
        builder
            .Property(e => e.Type)
            .HasConversion(
                v => StringUtils.AddSpacesBetweenCapitals(v.ToString(), true),
                v => (DeviceType)Enum.Parse(typeof(DeviceType), Regex.Replace(v, @"\s+", "")));
        builder
            .HasMany(e => e.Tags)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "DeviceTag",
                j => j
                    .HasOne<Tag>()
                    .WithMany()
                    .HasForeignKey("TagId")
                    .HasConstraintName("FK_DeviceTag_Tags_TagId")
                    .OnDelete(DeleteBehavior.NoAction),
                j => j
                    .HasOne<Device>()
                    .WithMany()
                    .HasForeignKey("DeviceId")
                    .HasConstraintName("FK_DeviceTag_Device_DeviceId")
                    .OnDelete(DeleteBehavior.NoAction));
        builder
            .HasMany(e => e.Teammates)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "DeviceTeammate",
                j => j
                    .HasOne<Teammate>()
                    .WithMany()
                    .HasForeignKey("TeammateId")
                    .HasConstraintName("FK_DeviceTeammate_Teammate_TeammateId")
                    .OnDelete(DeleteBehavior.NoAction),
                j => j
                    .HasOne<Device>()
                    .WithMany()
                    .HasForeignKey("DeviceId")
                    .HasConstraintName("FK_DeviceTeammate_Device_DeviceId")
                    .OnDelete(DeleteBehavior.NoAction));
    }
}
