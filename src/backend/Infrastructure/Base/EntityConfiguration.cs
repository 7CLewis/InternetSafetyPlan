using InternetSafetyPlan.Domain.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternetSafetyPlan.Infrastructure.Base;

public class EntityConfiguration<T> : IEntityTypeConfiguration<T> where T : Entity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder
            .HasKey(e => e.Id);
        builder
            .Property(e => e.Created)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");
        builder
            .Property(e => e.LastUpdated)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
