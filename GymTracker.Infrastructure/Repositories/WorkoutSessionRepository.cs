using GymTracker.Core.Interfaces;
using GymTracker.Core.Models;
using GymTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Infrastructure.Repositories;

public class WorkoutSessionRepository : IWorkoutSessionRepository
{
    private readonly IDbContextFactory<GymDbContext> _contextFactory;

    public WorkoutSessionRepository(IDbContextFactory<GymDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<WorkoutSession> StartSessionAsync(int workoutId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var workout = await context.Workouts
            .Include(w => w.WorkoutExercises)
            .FirstAsync(w => w.Id == workoutId);

        var session = new WorkoutSession
        {
            WorkoutId = workoutId,
            Date = DateTime.Now
        };

        foreach (var we in workout.WorkoutExercises)
        {
            for (var i = 1; i <= we.Sets; i++)
            {
                session.Sets.Add(new LoggedSet
                {
                    ExerciseId = we.ExerciseId,
                    SetNumber = i,
                    Weight = we.Weight,
                    Reps = we.Reps
                });
            }
        }

        context.WorkoutSessions.Add(session);
        await context.SaveChangesAsync();
        return session;
    }

    public async Task<WorkoutSession?> GetByIdAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.WorkoutSessions
            .Include(s => s.Sets)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<WorkoutSession>> GetByWorkoutAsync(int workoutId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.WorkoutSessions
            .Include(s => s.Sets)
            .Where(s => s.WorkoutId == workoutId)
            .OrderByDescending(s => s.Date)
            .ToListAsync();
    }
    public async Task<List<WorkoutSession>> GetCompletedAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.WorkoutSessions
            .Include(s => s.Workout)
            .Include(s => s.Sets)
            .Where(s => s.Sets.Any() && s.Sets.All(set => set.Completed ))
            .OrderByDescending(s => s.Date)
            .ToListAsync();
    }

    public async Task UpdateSetAsync(LoggedSet set)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var existing = await context.LoggedSets.FindAsync(set.Id);
        if (existing is null)
        {
            return;
        }
        
        existing.Weight = set.Weight;
        existing.Reps = set.Reps;
        existing.Completed  = set.Completed;
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var session = await context.WorkoutSessions.FindAsync(id);
        if (session is null)
        {
            return;
        }

        context.WorkoutSessions.Remove(session);
        await context.SaveChangesAsync();
    }
    
    
    
}