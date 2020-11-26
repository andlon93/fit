using FitnessTracker.DTO;
using FitnessTracker.GraphQLTypes;
using FitnessTracker.Users.DTOs;
using FitnessTracker.Workouts;
using FitnessTracker.Workouts.GraphTypes;
using GraphQL;
using GraphQL.Types;
using System;

namespace FitnessTracker.Users.GraphTypes
{
    public class UserType : ObjectGraphType<User>
    {
        private readonly WorkoutService _workoutservice;
        public UserType(WorkoutService workoutservice)
        {
            _workoutservice = workoutservice;

            Field<GuidGraphType, Guid>().Name(nameof(User.Id)).Description("Id of the user");
            Field<StringGraphType, string?>().Name(nameof(User.Email));
            Field<StringGraphType, string?>().Name(nameof(User.FirstName));
            Field<StringGraphType, string?>().Name(nameof(User.LastName));
            Field<StringGraphType, string?>().Name(nameof(User.Country));
            Field<IntGraphType, int?>().Name(nameof(User.Height));
            Field<ListGraphType<WorkoutGraphType>>(
                "workouts",
                arguments: new QueryArguments(PagingArgument.GetArgument()),
                resolve: context =>
                {
                    var paging = context.GetArgument<Paging>(PagingArgument.GetArgumentName());

                    return _workoutservice.GetWorkouts(paging, new Filter { Ids = context.Source.WorkoutIds });
                });           
        }
    }
}
