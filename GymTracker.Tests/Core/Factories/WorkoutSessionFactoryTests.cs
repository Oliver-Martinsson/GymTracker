using GymTracker.Core.Factories;
using GymTracker.Core.Models;

namespace GymTracker.Test.Core.Factories;

public class WorkoutSessionFactoryTests
{
   private const int WorkoutId = 7;
   private static readonly DateTime SessionDate = new(2026, 10, 12);

   private readonly WorkoutSession _session;

   public WorkoutSessionFactoryTests()
   {
      var workout = new Workout
      {
         Id = WorkoutId,
         WorkoutExercises = 
         [
            new WorkoutExercise { ExerciseId = "bench", Sets = 3, Reps = 8, Weight = 60 },
            new WorkoutExercise { ExerciseId = "squat", Sets = 2, Reps = 5, Weight = 100 }
         ]
      };
      
      _session = WorkoutSessionFactory.CreateFrom(workout, SessionDate);
   }

   [Fact]
   public void CreateFrom_Should_CreateOneLoggedSetPerPlannedSet()
   {
      Assert.Equal(5, _session.Sets.Count);
   }

   [Fact]
   public void CreateFrom_Should_RestartSetNumber_When_NextExerciseStarts()
   {
      Assert.Equal([1, 2, 3, 1, 2], _session.Sets.Select(set => set.SetNumber));
   }

   [Fact]
   public void CreateFrom_Should_CopyExerciseWeightAndReps()
   {
      var lastSet = _session.Sets.Last();
      
      Assert.Equal(("squat", 100.0, 5), (lastSet.ExerciseId, lastSet.Weight, lastSet.Reps));
   }
   
   [Fact]
   public void CreateFrom_Should_MarkAllSetsAsNotCompleted()
   {
      Assert.All(_session.Sets, set => Assert.False(set.Completed));
   }
   
   [Fact]
   public void CreateFrom_Should_SetWorkoutIdAndDate()
   {
      Assert.Equal(WorkoutId, _session.WorkoutId);
      Assert.Equal(SessionDate, _session.Date);
   }

   [Fact]
   public void CreateFrom_Should_ReturnNoSets_When_WorkoutHasNoExercises()
   {
      var session = WorkoutSessionFactory.CreateFrom(new Workout(), SessionDate);
      
      Assert.Empty(session.Sets);
   }
}