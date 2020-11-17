using FitnessTracker.GraphQLTypes;
using GraphQL.Types;
using GraphQL.Utilities;
using System;

namespace FitnessTracker
{
    public class FitnessTrackerSchema : Schema
    {
        public FitnessTrackerSchema(IServiceProvider provider)
            : base(provider)
        {
            Query = provider.GetRequiredService<WorkoutQuery>();
        }
    }
}