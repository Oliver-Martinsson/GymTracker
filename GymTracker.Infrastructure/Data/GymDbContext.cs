using System;
using System.Collections.Generic;
using System.Text;
using GymTracker.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Infrastructure.Data
{
    
    
    public class GymDbContext : DbContext
    {
        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options){ }

        public DbSet<Workout> Workouts { get; set; }
        public DbSet<WorkoutExercise> WorkoutExercises { get; set; }

        public DbSet<WorkoutSession> WorkoutSessions { get; set; }
        public DbSet<LoggedSet> LoggedSets { get; set; }
        
        
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<WorkoutSession>()
                .HasOne(s => s.Workout)
                .WithMany()
                .HasForeignKey(s => s.WorkoutId);
            
            modelBuilder.Entity<LoggedSet>()
                .HasOne(l => l.WorkoutSession)
                .WithMany(s => s.Sets)
                .HasForeignKey(l => l.WorkoutSessionId);
            
            modelBuilder.Entity<WorkoutExercise>()
                .HasOne(we => we.Workout)
                .WithMany(w => w.WorkoutExercises)
                .HasForeignKey(we => we.WorkoutId);
        }
    }
}
