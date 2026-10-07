using GymTracker.Core.Interfaces;
using GymTracker.Core.Models;
using GymTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Infrastructure.Repositories;

public class WorkoutRepository : IWorkoutRepository
{
    private readonly IDbContextFactory<GymDbContext> _contextFactory;
    
    public WorkoutRepository(IDbContextFactory<GymDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<List<Workout>> GetAllAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Workouts
            .Where(w => !w.IsArchived)
            .Include(w => w.WorkoutExercises)
            .ToListAsync();
    }

    public async Task<Workout?> GetByIdAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Workouts
            .Include(w => w.WorkoutExercises)
            .FirstOrDefaultAsync(w => w.Id == id && !w.IsArchived);
    }

    public async Task AddAsync(Workout workout)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        context.Workouts.Add(workout);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Workout workout)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        context.Workouts.Update(workout);
        await context.SaveChangesAsync();
    }

    public async Task ArchiveAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var workout = await context.Workouts.FindAsync(id);
        if (workout is null)
        {
            return;
        }

        workout.IsArchived = true;
        await context.SaveChangesAsync();
    }

    public async Task RemoveExerciseAsync(int workoutExerciseId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var workoutExercise = await context.WorkoutExercises.FindAsync(workoutExerciseId);
        if (workoutExercise is null)
        {
            return;
        }
        
        context.WorkoutExercises.Remove(workoutExercise);
        await context.SaveChangesAsync();
    }
}