using FitnessTracker.DTO;
using GraphQL;
using GraphQL.Types;
using System.Collections.Generic;
using System.Linq;

namespace FitnessTracker.GraphQLTypes
{
    public class WorkoutQuery : ObjectGraphType
    {
        private const string _pagingArgumentName = "paging";
        private const string _filterArgumentName = "filter";

        public WorkoutQuery()
        {
            Field<ListGraphType<WorkoutGraphType>>(
                name: "workouts",
                arguments: new QueryArguments(
                    new QueryArgument<PagingGraphType>
                    {
                        Name = _pagingArgumentName,
                        DefaultValue = new Paging { Rows = 10, Offset = 0 }
                    },
                    new QueryArgument<FilterGraphType>
                    {
                        Name = _filterArgumentName
                    }
                ),
                resolve: context =>
                {
                    var paging = context.GetArgument<Paging>(_pagingArgumentName);
                    var filter = context.GetArgument<Filter>(_filterArgumentName);

                    return Database.Database.FindAll()
                             .OrderByDescending(w => w.Activities.Activity[0].Lap[0].StartTime)
                             .Skip(paging.Offset)
                             .Take(paging.Rows)
                             .Select(w => MapTcxToWorkout(w))   
                             .Where(w => filter?.Id?.Any() != true || filter.Id.Contains(w.Id));
                             
                } 
            );
        }
          
        private static Workout MapTcxToWorkout(TrainingCenterDatabase_t w)
        {
            var activity = w.Activities.Activity[0];
            var lap = activity?.Lap?.FirstOrDefault();
            return new Workout
            {
                Id = activity?.Id.ToString(),
                Sport = activity?.Sport.ToString(),
                StartTime = lap?.StartTime,
                TotalTimeSeconds = lap?.TotalTimeSeconds,
                Distance = lap?.DistanceMeters,
                Calories = lap?.Calories,
                Cadence = lap?.Cadence,
                AverageHeartRate = lap?.AverageHeartRateBpm?.Value,
                MaximumHeartRate = lap?.MaximumHeartRateBpm?.Value,
                Positions = ConcatenateLaps(activity)
            };
        }

        private static IEnumerable<TrackPoint>? ConcatenateLaps(Activity_t? activity)
        {
            if (activity == null) return null;

            var result = new List<TrackPoint>();
            foreach (var lap in activity.Lap)
            {
                foreach (var track in lap.Track)
                {
                    result.Add(new TrackPoint 
                    {
                        AltitudeMeters = track.AltitudeMeters,
                        Cadence = track.Cadence,
                        Distancemeters = track.DistanceMeters,
                        HeartRate = track.HeartRateBpm?.Value,
                        Position = new Position { LatitudeDegrees = track.Position?.LatitudeDegrees, LongitudeDegrees = track.Position?.LongitudeDegrees },
                        Time = track.Time,
                    });
                }
            }
            return result;
        }
    }
}
