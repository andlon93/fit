using FitnessTracker.DTO;
using GraphQL.Types;
using System;

namespace FitnessTracker.GraphQLTypes
{
    public class TrackPointGraphType : ObjectGraphType<TrackPoint>
    {
        public TrackPointGraphType()
        {
            Field<DecimalGraphType, double>()
                .Name(nameof(TrackPoint.AltitudeMeters))
                .Description("");

            Field<ByteGraphType, byte>()
                .Name(nameof(TrackPoint.Cadence))
                .Description("");

            Field<DecimalGraphType, double>()
                .Name(nameof(TrackPoint.Distancemeters))
                .Description("");

            Field<IntGraphType, int>()
                .Name(nameof(TrackPoint.HeartRate))
                .Description("Heart rate in beats per minute (bpm)");

            Field<PositionGraphType, Position>()
                .Name(nameof(TrackPoint.Position))
                .Description("");

            Field<DateTimeGraphType, DateTime>()
                .Name(nameof(TrackPoint.Time))
                .Description("");
        }

        
    }
}