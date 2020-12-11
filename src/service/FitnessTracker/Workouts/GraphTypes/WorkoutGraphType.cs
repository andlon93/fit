﻿using FitnessTracker.Workouts.DTOs;
using GraphQL.Types;
using System;
using System.Collections.Generic;

namespace FitnessTracker.Workouts.GraphTypes
{
    public class WorkoutGraphType : ObjectGraphType<Workout>
    {
        public WorkoutGraphType()
        {
            Field<StringGraphType, string>()
                .Name(nameof(Workout.Id))
                .Description("");

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

            Field<ListGraphType<TrackPointGraphType>, IEnumerable<TrackPoint>>()
                .Name(nameof(Workout.Positions))
                .Description("");

            Field<DecimalGraphType, string>()
                .Name(nameof(Workout.MaxAltitudeMeters))
                .Description("Maximum altitude in meters");

            Field<DecimalGraphType, string>()
                .Name(nameof(Workout.MinAltitudeMeters))
                .Description("Minimum altitude in meters");

            Field<DecimalGraphType, string>()
                .Name(nameof(Workout.MinMinutesPerKm))
                .Description("Minumum minutes per kilometer");

            Field<DecimalGraphType, string>()
                .Name(nameof(Workout.AvgMinutesPerKm))
                .Description("Average minutes per kilometer");
        }
    }
}
