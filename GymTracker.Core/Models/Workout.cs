using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymTracker.Core.Models
{
    public class Workout
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public List<WorkoutExercise> WorkoutExercises { get; set; } = new();
        public bool IsArchived { get; set; }
    }
}