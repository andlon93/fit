using FitnessTracker.DTO;
using GraphQL.Types;
using System;

namespace FitnessTracker.GraphQLTypes
{
    public class WorkoutType : ObjectGraphType<Workout>
    {
        public WorkoutType()
        {
            Field<IntGraphType, string>()
                .Name(nameof(Workout.AverageHeartRate))
                .Description("Average heart rate in beats per minute (bpm)");

            Field<IntGraphType, string>()
                .Name(nameof(Workout.Cadence))
                .Description("Steps per minute");

            Field<IntGraphType, string>()
                .Name(nameof(Workout.Calories))
                .Description("Calories in kcal");

            Field<DecimalGraphType, string>()
                .Name(nameof(Workout.Distance))
                .Description("Distance in meters");

            Field<IntGraphType, string>()
                .Name(nameof(Workout.MaximumHeartRate))
                .Description("Maximum heart rate in beats per minute (bpm)");

            Field<StringGraphType, string>()
                .Name(nameof(Workout.Sport))
                .Description("");

            Field<DateTimeGraphType, DateTime>()
                .Name(nameof(Workout.StartTime))
                .Description("Start time of the workout");

            Field<DecimalGraphType, string>()
                .Name(nameof(Workout.TotalTimeSeconds))
                .Description("Total time in seconds");
        }
    }
}
