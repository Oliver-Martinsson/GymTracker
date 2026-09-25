using System;
using System.Collections.Generic;
using System.Text;

namespace GymTracker.Core.Models
{
    public class WorkoutExercise
    {
        public int Id { get; set; }
        public int WorkoutId { get; set; }
        public Workout Workout { get; set; } = null!;

        public string ExerciseId { get; set; } = string.Empty;
        public Exercise Exercise { get; set; } = null!;

        public int Sets { get; set; }
        public int Reps { get; set; }
        public double Weight { get; set; }
    }
}