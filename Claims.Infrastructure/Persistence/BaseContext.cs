using Microsoft.EntityFrameworkCore;

namespace Claims.Infrastructure.Persistence;

public class BaseContext<TContext> : DbContext where TContext : DbContext
{
    protected BaseContext(DbContextOptions<TContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TContext).Assembly);
    }
}