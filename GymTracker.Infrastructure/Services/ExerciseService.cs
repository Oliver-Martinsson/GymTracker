using System.Net.Http.Json;
using GymTracker.Core.Interfaces;
using GymTracker.Core.Models;

namespace GymTracker.Infrastructure.Services;

public class ExerciseService : IExerciseService
{
    private readonly HttpClient _http;

    public ExerciseService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Exercise>> GetAllAsync()
    {
        var response = await _http.GetFromJsonAsync<ApiResponse>("api/v1/exercises?limit=100&offset=0");
        return response?.Data ?? new List<Exercise>();
    }

    public async Task<Exercise?> GetByIdAsync(string id)
    {
        var response = await _http.GetFromJsonAsync<ExerciseResponse>($"api/v1/exercises/{id}");
        return response?.Data;
    }
}
