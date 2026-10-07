using System.Runtime.CompilerServices;
using GymTracker.Infrastructure.Data;
using System.Data;
using GymTracker.Core.Models;
using Xunit;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Infrastructure.Test
{
    public class DBTests
    {
        
         private GymDbContext CreateDbContext()
         {
            var options = new DbContextOptionsBuilder<GymDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

                return new GymDbContext(options);
         }

        [Fact]
        public async Task CheckThatDatabaseCanAddAndRetriveData()
        {
            using var context = CreateDbContext();

            var workout = new Workout()
            {
                Id = 123,
                Name = "test",
            };

            context.Workouts.Add(workout);
            await context.SaveChangesAsync();

            var savedWorkout = await context.Workouts.FirstOrDefaultAsync(w => w.Id == workout.Id);

            Assert.Equal("123",savedWorkout.Name);
            Assert.NotNull(savedWorkout.Name);
            Assert.NotEqual("Fel", savedWorkout.Name);
            Assert.IsType<string>(savedWorkout.Name);
        }
    
    }
}
