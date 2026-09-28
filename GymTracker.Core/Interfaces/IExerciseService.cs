namespace GymTracker.Core.Interfaces;
using GymTracker.Core.Models;

public interface IExerciseService
{
    Task<List<Exercise>> GetAllAsync();
    
    Task<Exercise?> GetByIdAsync(string id);
}