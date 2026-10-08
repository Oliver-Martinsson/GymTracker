using GymTracker.Core.Models;
using GymTracker.Core.Interfaces;

namespace GymTracker.Infrastructure.Services;

public class FakeExerciseService : IExerciseService
{
    private readonly List<Exercise> _exercises = new()
    {
        new Exercise { Id = "1", Name = "Barbell Squat",  BodyParts = new() { "Upper Legs" }, TargetMuscles = new() { "Quads" },     Equipments = new() { "Barbell" } },
        new Exercise { Id = "2", Name = "Bench Press",    BodyParts = new() { "Chest" },      TargetMuscles = new() { "Pectorals" }, Equipments = new() { "Barbell" } },
        new Exercise { Id = "3", Name = "Deadlift",       BodyParts = new() { "Back" },       TargetMuscles = new() { "Glutes" },    Equipments = new() { "Barbell" } },
        new Exercise { Id = "4", Name = "Pull Up",        BodyParts = new() { "Back" },       TargetMuscles = new() { "Lats" },      Equipments = new() { "Body Weight" } },
        new Exercise { Id = "5", Name = "Overhead Press", BodyParts = new() { "Shoulders" },  TargetMuscles = new() { "Delts" },     Equipments = new() { "Barbell" } },
    };

    public Task<List<Exercise>> SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return GetAllAsync();
        
        var result = _exercises
            .Where(e => e.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
            .ToList();
        return Task.FromResult(result);
        
    }


    public Task<List<Exercise>> GetAllAsync()
        => Task.FromResult(_exercises);

    public Task<Exercise?> GetByIdAsync(string id)
        => Task.FromResult(_exercises.FirstOrDefault(e => e.Id == id));
    
    public Task<List<Exercise>> FilterAsync(string? bodyPart, string? equipment)
    {
        var result = _exercises.AsEnumerable();
        if (!string.IsNullOrEmpty(bodyPart))
            result = result.Where(e => e.BodyParts.Contains(bodyPart));
        if (!string.IsNullOrEmpty(equipment))
            result = result.Where(e => e.Equipments.Contains(equipment));
        return Task.FromResult(result.ToList());
    }
    
    
}
