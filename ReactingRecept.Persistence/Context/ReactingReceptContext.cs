using Microsoft.EntityFrameworkCore;
using ReactingRecept.Domain.Entities;

namespace ReactingRecept.Persistence.Context;

public class ReactingReceptContext : DbContext
{
    public DbSet<Category> Category => Set<Category>();
    public DbSet<Ingredient> Ingredient => Set<Ingredient>();
    public DbSet<Recipe> Recipe => Set<Recipe>();
    public DbSet<DailyIntake> DailyIntake => Set<DailyIntake>();

    public ReactingReceptContext()
    {
        ChangeTracker.LazyLoadingEnabled = false;
    }

    public ReactingReceptContext(DbContextOptions options) : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}
