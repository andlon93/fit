using FitnessTracker.DTO;
using FitnessTracker.Users;
using FitnessTracker.Users.GraphTypes;
using FitnessTracker.Workouts;
using FitnessTracker.Workouts.DTOs;
using FitnessTracker.Workouts.GraphTypes;
using GraphQL;
using GraphQL.Types;
using System.Collections.Generic;
using System.Linq;

namespace FitnessTracker.GraphQLTypes
{
    public class WorkoutQuery : ObjectGraphType
    {
        private readonly UserService _userService;
        private readonly WorkoutService _workoutService;
        private readonly EndomondeZipFileReader _endomondeZipFileReader;

        private const string _filterArgumentName = "filter";        

        public WorkoutQuery(UserService userService, WorkoutService workoutService, EndomondeZipFileReader endomondeZipFileReader)
        {
            _userService = userService;
            _workoutService = workoutService;
            _endomondeZipFileReader = endomondeZipFileReader;
            
            _endomondeZipFileReader.ReadZipFile();

            Field<ListGraphType<UserType>>(
                    name: "users",
                    arguments: new QueryArguments(PagingArgument.GetArgument()),
                    resolve: context => _userService.GetUsers().ToList()
                );

            Field<ListGraphType<WorkoutGraphType>>(
                name: "workouts",
                arguments: new QueryArguments(
                    PagingArgument.GetArgument(),
                    new QueryArgument<FilterGraphType>
                    {
                        Name = _filterArgumentName
                    }
                ),
                resolve: context =>
                {
                    var paging = context.GetArgument<Paging>(PagingArgument.GetArgumentName());
                    var filter = context.GetArgument<Filter>(_filterArgumentName);

                    return _workoutService.GetWorkouts(paging, filter);

                }
            );            
        }
    }
}
