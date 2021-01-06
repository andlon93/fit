using FitnessTracker.TCX;
using FitnessTracker.Users;
using FitnessTracker.Workouts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FitnessTracker
{
    public class EndomondeZipFileReader
    {
        private const string _pathToZipFiles = "/app/Data/";
        private const string _filename = "endomondo-2020-11-14.zip";
        private readonly WorkoutCommandService _workoutService;
        private readonly UserCommandService _userCommandService;

        public EndomondeZipFileReader(WorkoutCommandService workoutService, UserCommandService userCommandService)
        {
            _workoutService = workoutService;
            _userCommandService = userCommandService;
        }        

        // TDOD: change to support n zip files.
        public void ReadZipFile()
        {
            TryUnzip(_filename);

            var workouts = TCXReader.ReadWorkouts(Directory.EnumerateFiles($"{_pathToZipFiles}{Path.GetFileNameWithoutExtension(_filename)}/Workouts", "*.tcx"));
            var workoutIds = _workoutService.SaveWorkoutsFromZipFile(workouts);

            var userFromFile = GetUserEntityFromFile();
            if (userFromFile != null) 
            {
                _userCommandService.SaveOrUpdateUser(new UserEntity 
                {
                    Id = Guid.NewGuid(),
                    GoogleId = userFromFile.SiteConnections?.FirstOrDefault(e => e.Site != null && e.Site.ToLower().Trim().Equals("google"))?.Identifier ?? throw new Exception("User entity has no google Id. This is required."),
                    Country = userFromFile.Country,
                    Email = userFromFile.Email,
                    Gender = userFromFile.Gender,
                    FirstName = userFromFile.FirstName,
                    LastName = userFromFile.LastName,
                    HeigtInCm = userFromFile.HeigtInCm,
                    TimeZone = userFromFile.TimeZone,
                    WorkoutIds = workoutIds,
                    SiteConnections = userFromFile?.SiteConnections?.Select(e => new SiteConnection { Site = e.Site, Identifier = e.Identifier })
                });
            }           
        }

        private UserFromFile? GetUserEntityFromFile() =>
            JsonSerializer.Deserialize<UserFromFile>(File.ReadAllText($"Data/{Path.GetFileNameWithoutExtension(_filename)}/Profile/Profile.json"));

        private void TryUnzip(string filename)
        {
            if (!Directory.Exists(Path.Join(_pathToZipFiles, Path.GetFileNameWithoutExtension(filename))))
            {
                System.IO.Compression.ZipFile.ExtractToDirectory(Path.Join(_pathToZipFiles, filename), _pathToZipFiles);
            }            
        }

        private record UserFromFile
        {
            [JsonPropertyName("FirstName")]
            public string? FirstName { get; init; }

            [JsonPropertyName("LastName")]
            public string? LastName { get; init; }

            [JsonPropertyName("email")]
            public string? Email { get; init; }

            [JsonPropertyName("country")]
            public string? Country { get; init; }

            [JsonPropertyName("TimeZone")]
            public string? TimeZone { get; init; }

            [JsonPropertyName("Gender")]
            public string? Gender { get; init; }

            [JsonPropertyName("heigtInCm")]
            public int? HeigtInCm { get; init; }

            [JsonPropertyName("SiteConnections")]
            public IEnumerable<SiteConnection>? SiteConnections { get; init; }
        }
    }
}
