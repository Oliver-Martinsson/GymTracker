using GymTracker.Core.Models;

namespace GymTracker.Core.Factories;

public class WorkoutSessionFactory
{
    public static WorkoutSession CreateFrom(Workout workout, DateTime date)
    {
        var session = new WorkoutSession
        {
            WorkoutId = workout.Id,
            Date = date
        };

        foreach (var workoutExercise in workout.WorkoutExercises)
        {
            for (var setNumber = 1; setNumber <= workoutExercise.Sets; setNumber++)
            {
                session.Sets.Add(new LoggedSet
                {
                    ExerciseId = workoutExercise.ExerciseId,
                    SetNumber = setNumber,
                    Weight = workoutExercise.Weight,
                    Reps = workoutExercise.Reps
                });
            }
        }
        return session;
    }
}