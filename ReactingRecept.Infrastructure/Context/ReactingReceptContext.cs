using Microsoft.EntityFrameworkCore;
using ReactingRecept.Server.Entities;

namespace ReactingRecept.Infrastructure.Context;

public class ReactingReceptContext : DbContext
{
    public ReactingReceptContext()
    {
        ChangeTracker.LazyLoadingEnabled = false;
    }

    public ReactingReceptContext(DbContextOptions options) : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
    }

    public DbSet<Category> Category => Set<Category>();
}
