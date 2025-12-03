using System.Text.RegularExpressions;
using InternetSafetyPlan.Domain.Shared;
using InternetSafetyPlan.Infrastructure.Base;
using InternetSafetyPlan.Shared;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternetSafetyPlan.Infrastructure.Shared;

public class TagConfiguration : ValueObjectConfiguration<Tag>
{
    public override void Configure(EntityTypeBuilder<Tag> builder)
    {
        base.Configure(builder);
        builder.HasKey(e => e.Id);
        builder
            .HasIndex(e => new { e.Name, e.Type });
        builder
            .Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(Tag.NameMaxLength);
        builder
            .Property(e => e.Type)
            .HasConversion(
                v => StringUtils.AddSpacesBetweenCapitals(v.ToString(), true),
                v => (TagType)Enum.Parse(typeof(TagType), Regex.Replace(v, @"\s+", "")))
            .HasMaxLength(Tag.TypeMaxLength);
    }
}
