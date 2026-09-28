namespace GymTracker.Infrastructure.Services;
using GymTracker.Core.Models;
using GymTracker.Core.Interfaces;

public class FakeExerciseService : IExerciseService
{
    private readonly List<Exercise> _exercises = new()
    {
        new Exercise { Id = "1", Name = "Barbell Squat", BodyPart = "Upper Legs", Target = "Quads", Equipment = "Barbell" },
        new Exercise {Id = "2", Name = "Bench Press", BodyPart = "Chest", Target = "Pectorals", Equipment = "Barbell"},
        new Exercise {Id = "3", Name = "Deadlift", BodyPart = "Back", Target = "Glutes", Equipment = "Barbell"},
        new Exercise {Id = "4", Name = "Pull Up", BodyPart = "Back", Target = "Lats", Equipment = "Body Weight"},
        new Exercise {Id = "5", Name = "Overhead Press", BodyPart = "Shoulders", Target = "Delts", Equipment = "Barbell"},
    };

    public Task<List<Exercise>> GetAllAsync()
        => Task.FromResult(_exercises);
    public Task<Exercise?> GetByIdAsync(string id) 
        => Task.FromResult(_exercises.FirstOrDefault(e => e.Id == id));

    }