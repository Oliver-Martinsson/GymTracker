using GymTracker.Core.Models;
namespace GymTracker.Core.Interfaces;

public interface IWorkoutRepository
{
    Task<List<Workout>> GetAllAsync();
    Task<Workout?> GetByIdAsync(int id);
    Task AddAsync(Workout workout);
    Task UpdateAsync(Workout workout);
    Task DeleteAsync(int id);
}