using System.Net;
using GymTracker.Infrastructure.Services;
using GymTracker.Tests.TestHelpers;

namespace GymTracker.Tests.Infrastructure.Services;

public class ExerciseServiceTests
{
    private const string BaseUrl = "https://test.local";

    private static ExerciseService CreateService(StubHttpMessageHandler handler)
       => new(new HttpClient(handler) { BaseAddress = new Uri(BaseUrl) });

    [Fact]
    public async Task GetAllAsync_Should_ReturnExercises_When_ApiReturnsData()
    {
        // Arrange
        var handler = new StubHttpMessageHandler(HttpStatusCode.OK, """
                                                                  { "success": true, "data": [
                                                                  { "exerciseId": "1", "name": "bench press" },
                                                                  { "exerciseId": "2", "name": "squat" } ] }
                                                                  """);
        var service = CreateService(handler);

        // Act
        var exercises = await service.GetAllAsync();

        // Assert
        Assert.Equal(["bench press", "squat"], exercises.Select(exercise => exercise.Name));
    }

    [Fact]
    public async Task GetAllAsync_Should_ReturnEmptyList_When_DataIsNull()
    {
        // Arrange
        var handler = new StubHttpMessageHandler(HttpStatusCode.OK, """{ "success": true, "data": null }""");
        var service = CreateService(handler);

        // Act
        var exercises = await service.GetAllAsync();

        // Assert
        Assert.Empty(exercises);
    }

    [Fact]
    public async Task GetByIdAsync_Should_ReturnExercise_When_Found()
    {
        // Arrange
        var handler = new StubHttpMessageHandler(HttpStatusCode.OK, """{ "data": { "exerciseId": "42", "name": "deadlift" } }""");
        var service = CreateService(handler);

        // Act
        var exercise = await service.GetByIdAsync("42");

        // Assert
        Assert.Equal("deadlift", exercise?.Name);
    }

    [Fact]
    public async Task FilterAsync_Should_PutBodyPartAndEquipmentInQuery()
    {
        // Arrange
        var handler = new StubHttpMessageHandler(HttpStatusCode.OK, """{ "data": [] }""");
        var service = CreateService(handler);

        // Act
        await service.FilterAsync("upper legs", "barbell");

        // Assert
        Assert.Equal("/api/v1/exercises/filter?bodyParts=upper%20legs&equipment=barbell&limit=100",
           handler.LastRequestUri?.PathAndQuery);
    }

    [Fact]
    public async Task GetByIdAsync_Should_ReturnNull_When_ApiReturnsNotFound()
    {
        // Arrange
        var handler = new StubHttpMessageHandler(HttpStatusCode.NotFound);
        var service = CreateService(handler);

        // Act
        var exercise = await service.GetByIdAsync("does-not-exist");

        // Assert
        Assert.Null(exercise);
    }
}