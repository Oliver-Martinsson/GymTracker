using System;
using System.Collections.Generic;
using System.Text;

namespace GymTracker.Infrastructure.Interfaces
{
    internal interface IWorkoutUrlBuilder
    {
        string BuildExerciseSearchUrl(string? bodyPart, string? target, string? equipment, string? name);
    }
}
