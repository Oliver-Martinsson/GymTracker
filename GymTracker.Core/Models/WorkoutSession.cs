namespace GymTracker.Core.Models;

public class WorkoutSession
{
    public int Id { get; set; }
    public int WorkoutId { get; set; }
    public Workout Workout { get; set; } = null!;
    public DateTime Date { get; set; }
    public List<LoggedSet> Sets { get; set; } = new();
}