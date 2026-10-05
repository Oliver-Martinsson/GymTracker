namespace GymTracker.Core.Models;

public class LoggedSet
{
    public int Id { get; set; }
    public int WorkoutSessionId { get; set; }
    public WorkoutSession WorkoutSession { get; set; } = null!;
    public string ExerciseId { get; set; }
    public int SetNumber { get; set; }
    public double Weight { get; set; }
    public int Reps { get; set; }
    public bool Completed { get; set; }

}