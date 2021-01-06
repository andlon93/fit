using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FitnessTracker.TCX
{
    public static class EndomondoJsonReader
    {
        public static IEnumerable<EndomondoWorkout> ReadWorkouts(IEnumerable<string> fileNames)
        {
                var response = new HashSet<EndomondoWorkout>();
                foreach (string file in fileNames)
                {
                    var seriaLized = ReadTrainingCenterDatabaseFromFile(file);
                    if (seriaLized != null)
                    {
                        response.Add(seriaLized);
                    }
                }


            return response;
        }

        public static EndomondoWorkout? ReadTrainingCenterDatabaseFromFile(string filename)
        {
            var response2 = new HashSet<EndomondoWorkout>();

            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            var jsonString = File.ReadAllText(filename);
            var workout = new EndomondoWorkout();
            var dynamic = JsonSerializer.Deserialize<List<DynamicEndomondoWorkout>>(jsonString, options);
            foreach (var d in dynamic)
            {
                workout.AltitudeMaxM = d.AltitudeMaxM ?? workout.AltitudeMaxM;
                workout.AltitudeMinM = d.AltitudeMinM ?? workout.AltitudeMinM;
                workout.AscendM = d.AscendM ?? workout.AscendM;
                workout.CadenceAvgRpm = d.CadenceAvgRpm ?? workout.CadenceAvgRpm;
                workout.CaloriesKcal = d.CaloriesKcal ?? workout.CaloriesKcal;
                workout.CreatedDate = d.CreatedDate == null ? workout.CreatedDate : DateTime.Parse(d.CreatedDate);
                workout.DescendM = d.DescendM ?? workout.DescendM;
                workout.DistanceKm = d.DistanceKm ?? workout.DistanceKm;
                workout.DurationS = d.DurationS ?? workout.DurationS;
                workout.EndTime = d.EndTime == null ? workout.EndTime : DateTime.Parse(d.EndTime);
                workout.HeartRateAvgBpm = d.HeartRateAvgBpm ?? workout.HeartRateAvgBpm;
                workout.HeartRateMaxBpm = d.HeartRateMaxBpm ?? workout.HeartRateMaxBpm;
                workout.Source = d.Source ?? workout.Source;
                workout.SpeedAvgKmh = d.SpeedAvgKmh ?? workout.SpeedAvgKmh;
                workout.SpeedMaxKmh = d.SpeedMaxKmh ?? workout.SpeedMaxKmh;
                workout.Sport = d.Sport ?? workout.Sport;
                workout.StartTime = d.StartTime == null ? workout.StartTime : DateTime.Parse(d.StartTime);
                if (d.Points != null)
                {
                    if (workout.Points == null)
                    {
                        workout.Points = new List<Point>();
                    }
                    foreach (var p in d.Points)
                    {
                        var point = new Point();
                        foreach (var dp in p)
                        {
                            point.Altitude = dp.Altitude ?? point.Altitude;
                            point.DistanceKm = dp.DistanceKm ?? point.DistanceKm;
                            point.HeartRateBpm = dp.HeartRateBpm ?? point.HeartRateBpm;
                            point.Timestamp = dp.Timestamp == null ? point.Timestamp : DateTime.ParseExact(dp.Timestamp.Replace(" UTC", ""), "ddd MMM dd HH:mm:ss yyyy", CultureInfo.InvariantCulture);
                            if (dp.Location != null)
                            {
                                if (point.Location == null)
                                {
                                    point.Location = new Location();
                                }
                                foreach (var dl in dp.Location[0])
                                {
                                    point.Location.Latitude = dl.Latitude ?? point.Location.Latitude;
                                    point.Location.Longitude = dl.Longitude ?? point.Location.Longitude;
                                }
                            }
                        }
                        workout.Points.Add(point);
                    }
                }
            }
            return workout;
        }
    }
}
