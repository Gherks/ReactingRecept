using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReactingRecept.Application.Interfaces.Persistence;
using ReactingRecept.Persistence.Context;
using ReactingRecept.Persistence.Repositories;

namespace ReactingRecept.Persistence.Extensions
{
    public static class ReactingReceptServiceExtensions
    {
        public static IServiceCollection AddReactingReceptDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ReactingReceptContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ReactingRecept"));
#if DEBUG
                options.EnableSensitiveDataLogging();
#endif
            });

            return services;
        }

        public static IServiceCollection AddReactingReceptRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            //services.AddScoped<IDailyIntakeEntryRepository, DailyIntakeEntryRepository>();
            //services.AddScoped<IIngredientRepository, IngredientRepository>();
            //services.AddScoped<IRecipeRepository, RecipeRepository>();

            //services.AddScoped<IDailyIntakeEntryLoaderFactory, DailyIntakeEntryLoaderFactory>();
            //services.AddTransient<DailyIntakeEntryFromRecipeLoader>();
            //services.AddTransient<DailyIntakeEntryFromIngredientLoader>();

            return services;
        }

        public static IServiceCollection AddReactingReceptServices(this IServiceCollection services)
        {
            //services.AddScoped<ICategoryService, CategoryService>();
            //services.AddScoped<IDailyIntakeEntryService, DailyIntakeEntryService>();
            //services.AddScoped<IIngredientService, IngredientService>();
            //services.AddScoped<IRecipeService, RecipeService>();

            return services;
        }
    }
}