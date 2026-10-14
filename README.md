# Gym Tracker 🏋️

![CI](https://github.com/Oliver-Martinsson/GymTracker/actions/workflows/ci.yml/badge.svg)

Gym Tracker is a web app for planning and logging strength training workouts, built with C#,
Blazor Server, Entity Framework Core and SQLite. It was made by **Team 404 Gains Not Found** as a
school project focused on testable architecture, SOLID principles and a DevOps workflow with
pull requests and GitHub Actions.

## Features

- **Exercise library**: search and filter exercises by body part and equipment (via the ExerciseDB API)
- **Routines**: create workout templates with exercises, sets, reps and weight, and archive old ones
- **Workout sessions**: start a session from a routine and log weight and reps set by set
- **Estimated 1RM**: calculated from your completed sets using the Epley formula

## Run the program

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```bash
dotnet run --project GymTracker.Web
```

Then open http://localhost:5261. The database is created automatically on first run.

## Team

| Name              | GitHub |
|-------------------|--------|
| Patrik Delen      | [@pdelen](https://github.com/pdelen) |
| Tobias Kickmaier  | [@Kickmaier](https://github.com/Kickmaier) |
| Oliver Martinsson | [@Oliver-Martinsson](https://github.com/Oliver-Martinsson) |
