using GymTracker.Infrastructure.Data;
using GymTracker.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace GymTracker.Test
{
    public class GymTrackerTests : IDisposable
    {
        private readonly SqliteConnection _connection;

        public GymTrackerTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            using var context = CreateDbContext();
            context.Database.EnsureCreated();
        }

        private GymDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<GymDbContext>()
                .UseSqlite(_connection)
                .Options;

            return new GymDbContext(options);
        }

        #region InfrastructureTest

        [Fact]
        public async Task CheckThatDatabaseCanAddAndRetriveData()
        {

            //Arrange
            using var context = CreateDbContext();

            var workout = new Workout()
            {
                Name = "test",
            };
            
            //Act
            context.Workouts.Add(workout);
            await context.SaveChangesAsync();

            var savedWorkout = await context.Workouts.FirstOrDefaultAsync(w => w.Id == workout.Id);
            
            //Assert
            Assert.NotNull(savedWorkout);
            Assert.Equal("test", savedWorkout.Name);
        
        }

        [Fact]
        public async Task AddWorkout_WithoutName_ThrowsDbUpdateException()
        {
            //Arrange
            using var context = CreateDbContext();
            context.Workouts.Add(new Workout { Name = null });

            //Act
            var exception = await Record.ExceptionAsync(() => context.SaveChangesAsync());

            //Assert
            Assert.IsType<DbUpdateException>(exception);
        }


        #endregion

        #region ApplicationTest
        //ApplicationsTest
        #endregion

        #region CoreTest
        //CoreTest
        #endregion
        public void Dispose() => _connection.Dispose();
    }
}
