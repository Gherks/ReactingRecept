 using Microsoft.EntityFrameworkCore;
using ReactingRecept.Persistence.Context;
using System;

namespace ReactingRecept.Persistence.IntegrationTests;

public static class TestDatabaseCreator
{
    public static ReactingReceptContext Create()
    {
        ReactingReceptContext context = CreateContext();
        SetupDatabase(context);

        return context;
    }

    private static ReactingReceptContext CreateContext()
    {
        string connectionString = $"Server=localhost;Database=ReactingReceptTest_{Guid.NewGuid()};Integrated Security=true;Encrypt=false;";

        ReactingReceptContext context = new(
            new DbContextOptionsBuilder<ReactingReceptContext>()
                .UseSqlServer(connectionString)
                .Options);

        context.Database.EnsureCreated();

        return context;
    }

    private static void SetupDatabase(ReactingReceptContext context)
    {
        context.Database.ExecuteSqlRaw(
            "INSERT [dbo].[Category] ([Id], [CategoryType], [SortOrder], [Name]) VALUES (N'cfc84d3d-f056-4191-a489-05fb2f11326b', 0, 1, N'Dairy')" +
            "INSERT [dbo].[Category] ([Id], [CategoryType], [SortOrder], [Name]) VALUES (N'06be00d5-4e13-4152-acbf-18eb1791cc36', 0, 3, N'Pantry')" +
            "INSERT [dbo].[Category] ([Id], [CategoryType], [SortOrder], [Name]) VALUES (N'2f3ec683-d693-450f-b46e-3273be0b60e8', 0, 0, N'Vegetables')" +
            "INSERT [dbo].[Category] ([Id], [CategoryType], [SortOrder], [Name]) VALUES (N'efcf64e4-10a4-4f18-a500-65f12dab95e8', 0, 2, N'Meat')" +
            "INSERT [dbo].[Category] ([Id], [CategoryType], [SortOrder], [Name]) VALUES (N'b6252e61-5e89-462c-b983-8ad6925bd5da', 0, 4, N'Other')" +
            "INSERT [dbo].[Category] ([Id], [CategoryType], [SortOrder], [Name]) VALUES (N'381b13a6-c709-4fb5-8af6-de209ccdaef5', 1, 1, N'Dessert')" +
            "INSERT [dbo].[Category] ([Id], [CategoryType], [SortOrder], [Name]) VALUES (N'6ab6f2a4-9b08-4b52-8b82-bdc02e366897', 1, 0, N'Meal')" +
            "INSERT [dbo].[Category] ([Id], [CategoryType], [SortOrder], [Name]) VALUES (N'8b7e39dc-866d-4a17-8a56-729f35a3aef8', 1, 2, N'Snacks')"
        );
    }
}
