using FitnessTracker.Challenges;
using FitnessTracker.DTO;
using FitnessTracker.Users;
using FitnessTracker.Users.GraphTypes;
using FitnessTracker.Workouts;
using FitnessTracker.Workouts.DTOs;
using FitnessTracker.Workouts.GraphTypes;
using FitnessTracker.WorkoutsGrouped.DTOs;
using FitnessTracker.WorkoutsGrouped.GraphTypes;
using GraphQL;
using GraphQL.Types;
using System.Linq;

namespace FitnessTracker.GraphQLTypes
{
    public class WorkoutQuery : ObjectGraphType
    {
        private readonly UserService _userService;
        private readonly WorkoutQueryService _workoutService;
        private readonly ChallengeService _challengeService;
        private readonly EndomondeZipFileReader _endomondeZipFileReader;

        private const string _filterArgumentName = "filter";

        public WorkoutQuery(
            UserService userService,
            WorkoutQueryService workoutService,
            ChallengeService challengeService,
            EndomondeZipFileReader endomondeZipFileReader)
        {
            _userService = userService;
            _workoutService = workoutService;
            _challengeService = challengeService;
            _endomondeZipFileReader = endomondeZipFileReader;
            
            _endomondeZipFileReader.ReadZipFile();

            Field<ListGraphType<UserType>>(
                    name: "users",
                    arguments: new QueryArguments(
                        new QueryArgument<Users.GraphTypes.FilterGraphType> { Name = _filterArgumentName }),
                    resolve: context =>
                    {
                        var filter = context.GetArgument<Users.DTOs.Filter>(_filterArgumentName);
                        return filter?.Ids.Any() != true
                            ? _userService.GetAllUsers().ToList()
                            : _userService.GetUsersByIds(filter.Ids).ToList();
                    }
                );

            Field<ListGraphType<WorkoutGraphType>>(
                    name: "workouts",
                    arguments: new QueryArguments(
                        PagingArgument.GetArgument(),
                        new QueryArgument<Workouts.GraphTypes.FilterGraphType> { Name = _filterArgumentName }),
                    resolve: context => _workoutService.GetWorkouts(
                            context.GetArgument<Paging>(PagingArgument.GetArgumentName()), 
                            context.GetArgument<Filter>(_filterArgumentName))
                );

            Field<ListGraphType<ChallengeGraphType>>(
                    name: "challenges",
                    resolve: context => _challengeService.GetChallenges().ToList()
                );

            Field<ListGraphType<GroupedWorkoutsGraphType>>(
                    name: "groupedWorkouts",
                    arguments: new QueryArguments(
                        PagingArgument.GetArgument(),
                        new QueryArgument<WorkoutGroupTypeGraphType> { Name = "groupBy", DefaultValue = WorkoutGroupType.Month }),
                    resolve: context => _workoutService.GetGroupedWorkouts(
                            context.GetArgument<WorkoutGroupType>("groupBy"), 
                            context.GetArgument<Paging>(PagingArgument.GetArgumentName())));
        }
    }
}
