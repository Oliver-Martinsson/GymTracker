using GymTracker.Infrastructure.Repositories;
using GymTracker.Core.Models;
using Microsoft.EntityFrameworkCore;
using GymTracker.Tests.TestHelpers;

namespace GymTracker.Tests;

public class GymTrackerTests : IDisposable
{
    private readonly TestDbContextFactory _factory = new();
    private readonly WorkoutRepository _workoutRepo;
    private readonly WorkoutSessionRepository _sessionRepo;

    public GymTrackerTests()
    {
        _workoutRepo = new WorkoutRepository(_factory);
        _sessionRepo = new WorkoutSessionRepository(_factory);
    }
    #region InfrastructureTest

    [Fact]
    public async Task CheckThatDatabaseCanAddAndRetriveData()
    {

        //Arrange
        using var context = _factory.CreateDbContext();

        var workout = new Workout()
        {
            Name = "test",
        };

        //Act
        context.Workouts.Add(workout);
        await context.SaveChangesAsync();

        var savedWorkout = await context.Workouts.FirstOrDefaultAsync(w => w.Id == workout.Id);

        //Assert
        Assert.NotNull(savedWorkout);
        Assert.Equal("test", savedWorkout.Name);

    }

    [Fact]
    public async Task AddWorkout_WithoutName_ThrowsDbUpdateException()
    {
        //Arrange
        using var context = _factory.CreateDbContext();
        context.Workouts.Add(new Workout { Name = null! });

        //Act
        var exception = await Record.ExceptionAsync(() => context.SaveChangesAsync());

        //Assert
        Assert.IsType<DbUpdateException>(exception);
    }

    [Fact]
    public async Task StartSession_CreatesOneLoggedSetPerPlannedSet()
    {
        // Arrange
        int workoutId;
        using (var context = _factory.CreateDbContext())
        {
            var workout = new Workout
            {
                Name = "push",
                WorkoutExercises =
                {
                    new WorkoutExercise {ExerciseId = "bench", Sets = 2, Reps = 3, Weight = 40  },
                    new WorkoutExercise {ExerciseId = "press", Sets = 4, Reps = 8, Weight = 180  }
                }
            };
            context.Workouts.Add(workout);
            await context.SaveChangesAsync();
            workoutId = workout.Id;
        }

        var repo = new WorkoutSessionRepository(_factory);

        // Act
        var session = await repo.StartSessionAsync(workoutId);
        var saved = await repo.GetByIdAsync(session.Id);

        // Assert
        Assert.NotNull(saved);
        Assert.Equal(workoutId, saved.WorkoutId);
        Assert.Equal(2 + 4, saved.Sets.Count);

        var bench = saved.Sets.Where(s => s.ExerciseId == "bench").OrderBy(s => s.SetNumber).ToList();
        Assert.Equal(new[] { 1, 2 }, bench.Select(s => s.SetNumber));
        Assert.All(bench, s => { Assert.Equal(40, s.Weight); Assert.Equal(3, s.Reps); });

        var press = saved.Sets.Where(s => s.ExerciseId == "press").OrderBy(s => s.SetNumber).ToList();
        Assert.Equal(new[] { 1, 2, 3, 4 }, press.Select(s => s.SetNumber));
        Assert.All(press, s => { Assert.Equal(180, s.Weight); Assert.Equal(8, s.Reps); });
    }

    [Fact]
    public async Task UpdateSet_ChangesWeightRepsAndCompleted_ButNotSetNumberOrExercise()
    {
        //Arrange
        var sessionId = await TestData.SeedSessionAsync(_factory);
        var session = await _sessionRepo.GetByIdAsync(sessionId);
        var original = session!.Sets.First(s => s.SetNumber == 1);

        var update = new LoggedSet
        {
            Id = original.Id,
            ExerciseId = "changed",
            SetNumber = 99,
            Weight = 50,
            Reps = 5,
            Completed = true,
        };

        //Act
        await _sessionRepo.UpdateSetAsync(update);

        //Assert
        var saved = await _sessionRepo.GetByIdAsync(sessionId);
        var changed = saved!.Sets.Single(s => s.Id == original.Id);
        Assert.Equal(50, changed.Weight);
        Assert.Equal(5, changed.Reps);
        Assert.True(changed.Completed);
        Assert.Equal(1, changed.SetNumber);
        Assert.Equal("bench", changed.ExerciseId);
    }

    [Fact]
    public async Task UpdateSet_DoesNotTouchOtherSets()
    {
        //Arrange
        var sessionId = await TestData.SeedSessionAsync(_factory);
        var seession = await _sessionRepo.GetByIdAsync(sessionId);
        var first = seession!.Sets.First(s => s.SetNumber == 1);
        var second = seession!.Sets.First(s => s.SetNumber == 2);

        //Act
        await _sessionRepo.UpdateSetAsync(new LoggedSet { Id = first.Id, Weight = 50, Reps = 5, Completed = true });

        //Assert
        var saved = await _sessionRepo.GetByIdAsync(sessionId);
        var untouched = saved!.Sets.Single(s => s.Id == second.Id);
        Assert.Equal(40, untouched.Weight);
        Assert.Equal(3, untouched.Reps);
        Assert.False(untouched.Completed);
    }
    [Fact]
    public async Task UpdateSet_WithUnknownId_DoesNotThrow()
    {
        //Act
        var exception = await Record.ExceptionAsync(() => _sessionRepo.UpdateSetAsync(new LoggedSet { Id = 999 }));

        //Assert
        Assert.Null(exception);
    }

    [Fact]
    public async Task GetCompleted_ReturnsOnlySessionWhereAllSetsAreCompleted()
    {
        var completedId = await TestData.SeedSessionAsync(_factory);
        await TestData.CompleteSetAsync(_factory, completedId);

        var partialId = await TestData.SeedSessionAsync(_factory);
        await TestData.CompleteSetAsync(_factory, partialId, count: 1);

        await TestData.SeedSessionAsync(_factory);

        //Act
        var result = await _sessionRepo.GetCompletedAsync();

        //Assert
        var session = Assert.Single(result);
        Assert.Equal(completedId, session.Id);
        Assert.NotNull(session.Workout);
        Assert.Equal(2, session.Sets.Count);
    }

    [Fact]
    public async Task GetCompleted_ExcludeSessionsWithoutSets()
    {
        //Arrange
        int workoutId;
        using (var context = _factory.CreateDbContext())
        {
            var workout = new Workout { Name = "empty" };
            context.Workouts.Add(workout);
            await context.SaveChangesAsync();
            workoutId = workout.Id;
        }
        await _sessionRepo.StartSessionAsync(workoutId);

        //Act
        var result = await _sessionRepo.GetCompletedAsync();

        //Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetCompleted_ReternNewestFirst()
    {
        //Arrange
        var olderId = await TestData.SeedSessionAsync(_factory);
        var newerId = await TestData.SeedSessionAsync(_factory);
        await TestData.CompleteSetAsync(_factory, olderId);
        await TestData.CompleteSetAsync(_factory, newerId);

        using (var context = _factory.CreateDbContext())
        {
            context.WorkoutSessions.Find(olderId)!.Date = DateTime.Now.AddDays(-2);
            context.WorkoutSessions.Find(newerId)!.Date = DateTime.Now;
            await context.SaveChangesAsync();
        }
        //Act
        var result = await _sessionRepo.GetCompletedAsync();

        //Assert
        Assert.Equal(new[] { newerId, olderId }, result.Select(s => s.Id).ToArray());
    }
    [Fact]
    public async Task Archive_HideWorkoutFromGetAllAndGetById_ButKeepRow()
    {
        //Arrange
        await _workoutRepo.AddAsync(new Workout { Name = "kept" });
        await _workoutRepo.AddAsync(new Workout { Name = "archived" });
        var all = await _workoutRepo.GetAllAsync();
        var archive = all.Single(w => w.Name == "archived");

        //Act
        await _workoutRepo.ArchiveAsync(archive.Id);

        //Assert
        var remaining = await _workoutRepo.GetAllAsync();
        Assert.Equal("kept", Assert.Single(remaining).Name);
        Assert.Null(await _workoutRepo.GetByIdAsync(archive.Id));

        using var context = _factory.CreateDbContext();
        var row = await context.Workouts.SingleAsync(w => w.Id == archive.Id);
        Assert.True(row.IsArchived);
    }

    [Fact]
    public async Task Archive_WithUnknownId_DoesnNotThrow()
    {
        //act
        var exception = await Record.ExceptionAsync(() => _workoutRepo.ArchiveAsync(999));

        //Assert
        Assert.Null(exception);
    }
    #endregion

    #region ApplicationTest
    //ApplicationsTest
    #endregion

    #region CoreTest
    //CoreTest
    #endregion
    public void Dispose() => _factory.Dispose();
}