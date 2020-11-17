using FitnessTracker.DTO;
using FitnessTracker.TCX;
using GraphQL;
using GraphQL.Types;
using System;
using System.Linq;

namespace FitnessTracker.GraphQLTypes
{
    public class WorkoutQuery : ObjectGraphType
    {
        private const string _pagingArgumentName = "paging";
        public WorkoutQuery()
        {
            Field<ListGraphType<WorkoutType>>(
                name: "workouts",
                arguments: new QueryArguments(
                    new QueryArgument<PagingType> 
                    { 
                        Name = _pagingArgumentName, 
                        DefaultValue = new Paging { Rows = 10, Offset = 0 }                         
                    }
                ),
                resolve: context =>
                {
                    var paging = context.GetArgument<Paging>(_pagingArgumentName);

                    return TCXReader.ReadWorkouts()
                             .Where(w => w.Activities?.FirstOrDefault()?.Lap?.StartTime != null)
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                             .OrderByDescending(w => DateTime.Parse(w.Activities.FirstOrDefault().Lap.StartTime))
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8604 // Possible null reference argument.
                             .Skip(paging.Offset)
                             .Take(paging.Rows)
                             .Select(w => MapTcxToWorkout(w));
                             
                } 
            );
        }

        private static Workout MapTcxToWorkout(TrainingCenterDatabase w)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            var activity = w.Activities.FirstOrDefault();
#pragma warning restore CS8604 // Possible null reference argument.
            var lap = activity?.Lap;
            return new Workout
            {
                Sport = activity?.Sport,
                StartTime = DateTime.TryParse(activity?.Lap?.StartTime, out var startTime) ? startTime : null,
                TotalTimeSeconds = lap?.TotalTimeSeconds,
                Distance = lap?.DistanceMeters,
                Calories = lap?.Calories,
                Cadence = lap?.Cadence,
                AverageHeartRate = lap?.AverageHeartRateBpm?.Value,
                MaximumHeartRate = lap?.MaximumHeartRateBpm?.Value,
            };
        }
    }
}
