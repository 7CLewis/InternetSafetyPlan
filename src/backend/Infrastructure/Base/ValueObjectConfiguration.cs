using InternetSafetyPlan.Domain.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternetSafetyPlan.Infrastructure.Base;

public class ValueObjectConfiguration<T> : IEntityTypeConfiguration<T> where T : ValueObject
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
    }
}
