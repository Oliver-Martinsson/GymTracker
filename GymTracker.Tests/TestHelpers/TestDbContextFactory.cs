using GymTracker.Infrastructure.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Tests.TestHelpers
{
    public class TestDbContextFactory : IDisposable, IDbContextFactory<GymDbContext>
    {
        private readonly SqliteConnection _connection;

    public TestDbContextFactory()
    {
        _connection = new SqliteConnection("DataSource=:memory:;Foreign Keys=True");
        _connection.Open();

        using var context = CreateDbContext();
        context.Database.EnsureCreated();
    }

    public GymDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<GymDbContext>()
            .UseSqlite(_connection)
            .Options;

        return new GymDbContext(options);
    }

    public void Dispose() => _connection.Dispose();
}
}
