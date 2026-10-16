using GymTracker.Core.Models;
using GymTracker.Infrastructure.Data;
using GymTracker.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Tests.TestHelpers;

public static class TestData
{
    public static async Task<int> SeedSessionAsync(IDbContextFactory<GymDbContext> factory)
    {
        int workoutId;
        await using (var context = await factory.CreateDbContextAsync())
        {
            var workout = new Workout
            {
                Name = "push",
                WorkoutExercises =
                {
                    new WorkoutExercise { ExerciseId = "bench", Sets = 2, Reps = 3, Weight = 40 }
                }
            };
            context.Workouts.Add(workout);
            await context.SaveChangesAsync();
            workoutId = workout.Id;
        }

        var session = await new WorkoutSessionRepository(factory).StartSessionAsync(workoutId);
        return session.Id;
    }
    public static async Task CompleteSetAsync(IDbContextFactory<GymDbContext> factory, int sessionId, int count = int.MaxValue)
    {
        var repo = new WorkoutSessionRepository(factory);
        var session = await repo.GetByIdAsync(sessionId);

        foreach (var set in session!.Sets.Take(count))
        {
            set.Completed = true;
            await repo.UpdateSetAsync(set);
        }
    }
}