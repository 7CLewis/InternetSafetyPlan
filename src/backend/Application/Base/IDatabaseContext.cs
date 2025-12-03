using Microsoft.EntityFrameworkCore;

namespace InternetSafetyPlan.Application.Base;

public interface IDatabaseContext
{
    public DbSet<T> Set<T>() where T : class;
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
}
