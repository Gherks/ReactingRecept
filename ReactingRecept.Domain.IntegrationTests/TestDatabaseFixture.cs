using Microsoft.EntityFrameworkCore;
using ReactingRecept.Contract;
using ReactingRecept.Infrastructure.Context;
using ReactingRecept.Server.Entities;
using System;
using static ReactingRecept.Shared.Enums;

namespace ReactingRecept.Infrastructure.IntegrationTests;

public class TestDatabaseFixture
{
    private const string _connectionString = @"Server=.;Database=ReactingReceptTest;Integrated Security=true";

    private static readonly object _lock = new();
    private static bool _databaseInitialized;

    public ReactingReceptContext? ReactingReceptContext { get; private set; }

    public TestDatabaseFixture()
    {
        lock (_lock)
        {
            if (!_databaseInitialized)
            {
                ReactingReceptContext = new ReactingReceptContext(
                    new DbContextOptionsBuilder<ReactingReceptContext>()
                        .UseSqlServer(_connectionString)
                        .Options);

                if (ReactingReceptContext == null)
                {
                    throw new InvalidOperationException();
                }

                ReactingReceptContext.Database.EnsureDeleted();
                ReactingReceptContext.Database.EnsureCreated();

                SetupDatabase();

                ReactingReceptContext.Database.BeginTransaction();
                _databaseInitialized = true;
            }
        }
    }

    private void SetupDatabase()
    {
        const string errorMessage = "Failed to setup database for infrastructure integration tests because DbContext has not been set.";
        Contracts.LogAndThrowWhenNull(ReactingReceptContext, errorMessage);

        ReactingReceptContext.AddRange(
            new Category
            {
                Id = Guid.Parse("CFC84D3D-F056-4191-A489-05FB2F11326B"),
                Name = "Dairy",
                CategoryType = CategoryType.Ingredient,
                SortOrder = 1
            },
            new Category
            {
                Id = Guid.Parse("06BE00D5-4E13-4152-ACBF-18EB1791CC36"),
                Name = "Pantry",
                CategoryType = CategoryType.Ingredient,
                SortOrder = 3
            },
            new Category
            {
                Id = Guid.Parse("2F3EC683-D693-450F-B46E-3273BE0B60E8"),
                Name = "Vegetables",
                CategoryType = CategoryType.Ingredient,
                SortOrder = 0
            },
            new Category
            {
                Id = Guid.Parse("EFCF64E4-10A4-4F18-A500-65F12DAB95E8"),
                Name = "Meat",
                CategoryType = CategoryType.Ingredient,
                SortOrder = 2
            },
            new Category
            {
                Id = Guid.Parse("8B7E39DC-866D-4A17-8A56-729F35A3AEF8"),
                Name = "Snacks",
                CategoryType = CategoryType.Recipe,
                SortOrder = 2
            },
            new Category
            {
                Id = Guid.Parse("B6252E61-5E89-462C-B983-8AD6925BD5DA"),
                Name = "Other",
                CategoryType = CategoryType.Ingredient,
                SortOrder = 4
            },
            new Category
            {
                Id = Guid.Parse("6AB6F2A4-9B08-4B52-8B82-BDC02E366897"),
                Name = "Meal",
                CategoryType = CategoryType.Recipe,
                SortOrder = 0
            },
            new Category
            {
                Id = Guid.Parse("381B13A6-C709-4FB5-8AF6-DE209CCDAEF5"),
                Name = "Dessert",
                CategoryType = CategoryType.Recipe,
                SortOrder = 1
            });

        ReactingReceptContext.SaveChanges();
    }
}
