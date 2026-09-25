using System;
using System.Collections.Generic;
using System.Text;

namespace GymTracker.Core.Models
{
    public class Workout
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<WorkoutExercise> WorkoutExercise { get; set; } = new();
    }
}