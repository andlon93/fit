using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace FitnessTracker.TCX
{
    public static class TCXReader
    {
        private const string path = "/app/Data/";
        private const string filename = "endomondo-2020-11-14.zip";

        private static IEnumerable<TrainingCenterDatabase>? workoutDatabase;

        public static IEnumerable<TrainingCenterDatabase> ReadWorkouts()
        {
            if (workoutDatabase == null)
            {
                var response = new HashSet<TrainingCenterDatabase>();

                foreach (string file in Directory.EnumerateFiles($"Data/{Path.GetFileNameWithoutExtension(filename)}/Workouts", "*.tcx"))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(TrainingCenterDatabase));

                    using (StreamReader sr = new StreamReader(file))
                    {
                        var seriaLized = ser.Deserialize(sr);
                        if (seriaLized != null)
                        {
                            response.Add((TrainingCenterDatabase)seriaLized);
                        }                        
                    }
                }

                //var response2 = new HashSet<EndomondoWorkout>();

                //var options = new JsonSerializerOptions
                //{
                //    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                //};
                //foreach (string filename in Directory.EnumerateFiles($"Data/{Path.GetFileNameWithoutExtension(filename)}/Workouts", "*.json"))
                //{
                //    var jsonString = File.ReadAllText(filename);
                //    var workout = JsonSerializer.Deserialize<List<EndomondoWorkout>>(jsonString, options);
                //    response2.Add(workout.First());
                //}
                workoutDatabase = response;
            }

            return workoutDatabase;
        }        
    }
}
