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
        public string Name { get; set; }
        public List<Exercise> Routines { get; set; } = new();
    }
}