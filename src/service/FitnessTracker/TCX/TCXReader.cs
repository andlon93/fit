using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace FitnessTracker.TCX
{
    public static class TCXReader
    {
        public static IEnumerable<TrainingCenterDatabase_t> ReadWorkouts(IEnumerable<string> fileNames)
        {
                var response = new HashSet<TrainingCenterDatabase_t>();
                foreach (string file in fileNames)
                {
                    var seriaLized = ReadTrainingCenterDatabaseFromFile(file);
                    if (seriaLized != null)
                    {
                        response.Add(seriaLized);
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

            return response;
        }

        public static TrainingCenterDatabase_t? ReadTrainingCenterDatabaseFromFile(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TrainingCenterDatabase_t));
            using (StreamReader sr = new StreamReader(filename))
            {
                var seriaLized = serializer.Deserialize(sr);
                return (TrainingCenterDatabase_t?)seriaLized;
            }
        }
    }
}
