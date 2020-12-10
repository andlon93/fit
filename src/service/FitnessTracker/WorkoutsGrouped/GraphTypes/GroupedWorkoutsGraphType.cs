using FitnessTracker.DTO;
using FitnessTracker.Workouts;
using FitnessTracker.Workouts.DTOs;
using FitnessTracker.Workouts.GraphTypes;
using FitnessTracker.WorkoutsGrouped.DTOs;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessTracker.WorkoutsGrouped.GraphTypes
{
    public class GroupedWorkoutsGraphType : ObjectGraphType<GroupedWorkout>
    {
        private readonly WorkoutQueryService _workoutQueryService;
        private readonly IDataLoaderContextAccessor _dataLoaderContextAccessor;

        public GroupedWorkoutsGraphType(WorkoutQueryService workoutQueryService, IDataLoaderContextAccessor dataLoaderContextAccessor)
        {
            _workoutQueryService = workoutQueryService;
            _dataLoaderContextAccessor = dataLoaderContextAccessor;

            Field<StringGraphType>().Name(nameof(GroupedWorkout.Title)).Description("Title");
            Field<FloatGraphType>().Name(nameof(GroupedWorkout.DurationInSeconds)).Description("DurationInSeconds");
            Field<FloatGraphType>().Name(nameof(GroupedWorkout.DistanceInMeters)).Description("DistanceInMeters");
            Field<LongGraphType>().Name(nameof(GroupedWorkout.Calories)).Description("Calories");
            Field<IntGraphType>().Name(nameof(GroupedWorkout.NumberOfWorkouts)).Description("Total number of workouts within group");
            Field<ListGraphType<WorkoutGraphType>>()
                .Name("workouts")
                .Resolve(ResolveWorkouts);
        }

        private IDataLoaderResult<Workout[]> ResolveWorkouts(IResolveFieldContext<GroupedWorkout> arg)
        {
            if (arg?.Source?.WorkoutIds?.Any() != true)
            {
                return new DataLoaderResult<Workout[]>(Array.Empty<Workout>());
            }

            var loader = _dataLoaderContextAccessor.Context.GetOrAddBatchLoader<Guid, Workout>(
                Guid.NewGuid().ToString(), // All dataloaders need a unique identifier to access the same loader each time.
                async context =>
                {
                    var workouts = _workoutQueryService.GetWorkouts(new Paging { Rows = context.Count() }, new Filter
                    {
                        Ids = context
                    });
                    return await Task.FromResult(workouts.ToDictionary(w => w.Id)); // TODO: remove async hack when dataaccess layer truly is async.
                });
            return loader.LoadAsync(arg.Source.WorkoutIds);
        }
    }
}
