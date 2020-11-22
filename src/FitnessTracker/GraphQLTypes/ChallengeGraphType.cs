//using FitnessTracker.DTO;
//using GraphQL.Types;
//using System;
//using System.Collections.Generic;

//namespace FitnessTracker.GraphQLTypes
//{
//    public class ChallengeGraphType : ObjectGraphType<Challenge>
//    {
//        public ChallengeGraphType()
//        {
//            Field<StringGraphType, string>()
//                .Name(nameof(Challenge.Name))
//                .Description("");

//            Field<ChallengeTypeGraphType, ChallengeType>()
//                .Name(nameof(Challenge.Type))
//                .Description("");

//            Field<DateTimeGraphType, DateTime>()
//                .Name(nameof(Challenge.StartTime))
//                .Description("");

//            Field<DateTimeGraphType, DateTime>()
//                .Name(nameof(Challenge.EndTime))
//                .Description("");
//        }
//    }
//}
