using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessTracker.Workouts.GraphTypes
{
    public class WorkoutInputType : InputObjectGraphType
    {


        public WorkoutInputType()
        {
            Name = "WorkoutInput";
            Field<NonNullGraphType<DateTimeGraphType>>("startTime");
            Field<NonNullGraphType<StringGraphType>>("sport");
            Field<NonNullGraphType<FloatGraphType>>("totalTimeSeconds");
        }  
    }
}
