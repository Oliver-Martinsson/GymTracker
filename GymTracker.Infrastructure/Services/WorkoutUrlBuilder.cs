using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace GymTracker.Infrastructure.Services
{
    internal class WorkoutUrlBuilder : IWorkoutUrlBuilder
    {
        private readonly string _baseUrl = "http://api.workoutxapp.com/v1";

        public string BuildExerciseSearchUrl(string? bodyPart, string? target, string? equipment, string? name)
        {
            var endpoint = $"{_baseUrl}/exercisees/search";
            var queryParams = new Dictionary<string, string?>();

            if(!string.IsNullOrWhiteSpace(bodyPart))
            {queryParams["body"] = bodyPart;}

            if(!string.IsNullOrWhiteSpace(target))
            { queryParams["target"] = target;}

            if(!string.IsNullOrWhiteSpace(equipment))
            { queryParams["equipment"] = equipment;}

            if(!string.IsNullOrWhiteSpace(name))
            { queryParams["name"] = name;}

            return QueryHelpers.AddQueryString(endpoint, queryParams);
    }
}
