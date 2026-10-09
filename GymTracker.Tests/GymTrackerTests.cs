using GymTracker.Infrastructure.Data;
using GymTracker.Infrastructure.Repositories;
using GymTracker.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Linq;
using System.Reflection;

namespace GymTracker.Test
{
    public class GymTrackerTests : IDisposable, IDbContextFactory<GymDbContext>
    {
        private readonly SqliteConnection _connection;

        public GymTrackerTests()
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
        [Fact]
        public async Task StartSession_CreateOneLoggedSetPerPlannedSet()
        {
            // Arrange
            int workoutId;
            using var context = CreateDbContext();
            {
                var workout = new Workout
                {
                    Name = "push",
                    WorkoutExercises =
                    {
                        new WorkoutExercise {ExerciseId = "bench", Sets = 2, Reps = 3, Weight = 40  },
                        new WorkoutExercise {ExerciseId = "press", Sets = 4, Reps = 8, Weight = 180  }
                    }
                };
                context.Workouts.Add(workout);
                await context.SaveChangesAsync();
                workoutId = workout.Id;
            }

            var repo = new WorkoutSessionRepository(this);

            // Act
            var session = await repo.StartSessionAsync(workoutId);
            var saved = await repo.GetByIdAsync(session.Id);

            // Assert
            Assert.NotNull(saved);
            Assert.Equal(workoutId, saved.WorkoutId);
            Assert.Equal(6, saved.Sets.Count);

            var bench = saved.Sets.Where(s => s.ExerciseId == "bench").OrderBy(s => s.SetNumber).ToList();
            Assert.Equal(new[] { 1, 2 }, bench.Select(s => s.SetNumber));
            Assert.All(bench, s => { Assert.Equal(40, s.Weight); Assert.Equal(3, s.Reps); });

            var press = saved.Sets.Where(s => s.ExerciseId == "press").OrderBy(s =>s.SetNumber).ToList();
            Assert.Equal(new[] { 1, 2, 3, 4 }, press.Select(s => s.SetNumber));
            Assert.All(press, s => { Assert.Equal(180, s.Weight); Assert.Equal(8, s.Reps); });
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
