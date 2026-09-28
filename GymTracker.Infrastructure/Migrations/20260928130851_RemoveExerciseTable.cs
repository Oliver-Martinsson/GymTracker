using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveExerciseTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutExercises_Exercises_ExerciseId",
                table: "WorkoutExercises");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutExercises_ExerciseId",
                table: "WorkoutExercises");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Apparatus = table.Column<string>(type: "TEXT", nullable: false),
                    BodyPart = table.Column<string>(type: "TEXT", nullable: false),
                    CaloriesPerMinute = table.Column<double>(type: "REAL", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Difficulty = table.Column<string>(type: "TEXT", nullable: false),
                    Equipment = table.Column<string>(type: "TEXT", nullable: false),
                    Force = table.Column<string>(type: "TEXT", nullable: false),
                    GifUrl = table.Column<string>(type: "TEXT", nullable: false),
                    Instructions = table.Column<string>(type: "TEXT", nullable: false),
                    IntensityLevel = table.Column<string>(type: "TEXT", nullable: false),
                    IsUnilateral = table.Column<bool>(type: "INTEGER", nullable: false),
                    JointFocus = table.Column<string>(type: "TEXT", nullable: false),
                    Mechanic = table.Column<string>(type: "TEXT", nullable: false),
                    Met = table.Column<double>(type: "REAL", nullable: false),
                    MovementTags = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PopularityRank = table.Column<int>(type: "INTEGER", nullable: false),
                    RecommendedReps = table.Column<string>(type: "TEXT", nullable: false),
                    RecommendedSets = table.Column<string>(type: "TEXT", nullable: false),
                    SecondaryMuscles = table.Column<string>(type: "TEXT", nullable: false),
                    Target = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutExercises_ExerciseId",
                table: "WorkoutExercises",
                column: "ExerciseId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutExercises_Exercises_ExerciseId",
                table: "WorkoutExercises",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
