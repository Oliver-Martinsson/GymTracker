using GymTracker.Core.Models;

namespace GymTracker.Core.Factories;

public class WorkoutSessionFactory
{
    public static WorkoutSession CreateFrom(Workout workout, DateTime date)
    {
        return new WorkoutSession();
    }
}