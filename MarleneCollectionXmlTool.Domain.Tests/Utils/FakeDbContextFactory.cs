using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace MarleneCollectionXmlTool.Domain.Tests.Utils;

internal static class FakeDbContextFactory
{
    internal static TDbContext CreateMockDbContext<TDbContext>() where TDbContext : DbContext
    {
        var databaseName = Guid.NewGuid().ToString();
        DbContextOptions<TDbContext> options = new DbContextOptionsBuilder<TDbContext>().UseInMemoryDatabase(databaseName, delegate (InMemoryDbContextOptionsBuilder b)
        {
            b.EnableNullChecks(nullChecksEnabled: false);
        }).ConfigureWarnings(delegate (WarningsConfigurationBuilder x)
        {
            x.Ignore(InMemoryEventId.TransactionIgnoredWarning);
        }).Options;
        if (Activator.CreateInstance(typeof(TDbContext), options) is not TDbContext val)
        {
            throw new NullReferenceException("Could not create InMemory DbContext.");
        }

        val.Database.EnsureDeleted();
        val.Database.EnsureCreated();

        return val;
    }

    internal static void Seed(this DbContext ctx, params object[] entities)
    {
        ctx.AddRange(entities);
        ctx.SaveChanges();
    }

    internal static void SeedRange<TEntity>(this DbContext ctx, IEnumerable<TEntity> entities) where TEntity : class
    {
        ctx.AddRange(entities);
        ctx.SaveChanges();
    }
}
