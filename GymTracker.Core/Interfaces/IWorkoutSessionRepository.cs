using GymTracker.Core.Models;

namespace GymTracker.Core.Interfaces;

public interface IWorkoutSessionRepository
{
    Task<WorkoutSession> StartSessionAsync(int workoutId);
    Task<WorkoutSession?> GetByIdAsync(int id);
    Task<List<WorkoutSession>> GetByWorkoutAsync(int workoutId);
    Task UpdateSetAsync(LoggedSet set);
    Task DeleteAsync(int id);
}