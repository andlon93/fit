using FitnessTracker.DTO;
using FitnessTracker.GraphQLTypes;
using FitnessTracker.Users.DTOs;
using FitnessTracker.Workouts;
using FitnessTracker.Workouts.DTOs;
using FitnessTracker.Workouts.GraphTypes;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;
using System;
using System.Linq;

namespace FitnessTracker.Users.GraphTypes
{
    public class UserType : ObjectGraphType<User>
    {
        private readonly WorkoutService _workoutservice;
        private readonly IDataLoaderContextAccessor _dataLoaderContextAccessor;
        public UserType(WorkoutService workoutservice, IDataLoaderContextAccessor dataLoaderContextAccessor)
        {
            _workoutservice = workoutservice;
            _dataLoaderContextAccessor = dataLoaderContextAccessor;

            Field<GuidGraphType, Guid>().Name(nameof(User.Id)).Description("Id of the user");
            Field<StringGraphType, string?>().Name(nameof(User.Email));
            Field<StringGraphType, string?>().Name(nameof(User.FirstName));
            Field<StringGraphType, string?>().Name(nameof(User.LastName));
            Field<StringGraphType, string?>().Name(nameof(User.Country));
            Field<IntGraphType, int?>().Name(nameof(User.Height));
            Field<ListGraphType<WorkoutGraphType>>()
                .Name("workouts")
                .Resolve(ResolveWorkouts);         
        }

        private IDataLoaderResult<Workout[]> ResolveWorkouts(IResolveFieldContext<User> arg)
        {
            if (arg?.Source?.WorkoutIds?.Any() != true)
            {
                return new DataLoaderResult<Workout[]>(new Workout[0]);
            }

            var loader = _dataLoaderContextAccessor.Context.GetOrAddBatchLoader<Guid, Workout>(
                Guid.NewGuid().ToString(), // All dataloaders need a unique identifier to access the same loader each time.
                async context =>
                {
                    var workouts = _workoutservice.GetWorkouts(new Paging { Rows = context.Count() }, new Filter
                    {
                        Ids = context
                    });
                    return workouts.ToDictionary(w => w.Id);
                });
            return loader.LoadAsync(arg.Source.WorkoutIds);
        }
    }
}
