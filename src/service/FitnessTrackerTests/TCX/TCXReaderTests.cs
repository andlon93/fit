using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FitnessTracker.TCX.Tests
{
    [TestClass]
    public class TCXReaderTests
    {
        [TestMethod]
        public void ReadTrainingCenterDatabaseFromFileTest()
        {
            var trainingCenterDatabase = TCXReader.ReadTrainingCenterDatabaseFromFile(@"C:\Users\ola\git\fit2\src\service\FitnessTrackerTests\TCX\2020-09-19_11-52-42.0.tcx");

            Assert.AreEqual(1, trainingCenterDatabase.Activities.Activity.Length);
            var activity = trainingCenterDatabase.Activities.Activity[0];
            Assert.AreEqual(new System.DateTime(2020, 9, 19, 11, 52, 42), activity.Id);
            Assert.AreEqual(Sport_t.Running, activity.Sport);
            Assert.AreEqual(11, activity.Lap.Length);
            var firstLap = activity.Lap[0];
            Assert.AreEqual(new System.DateTime(2020, 9, 19, 8, 26, 17), firstLap.StartTime);
            Assert.AreEqual(317.0, firstLap.TotalTimeSeconds);
            Assert.AreEqual(1000.0, firstLap.DistanceMeters);
            Assert.AreEqual(574, firstLap.Calories);
            Assert.AreEqual(181, firstLap.AverageHeartRateBpm.Value);
            Assert.AreEqual(188, firstLap.MaximumHeartRateBpm.Value);
            Assert.AreEqual(Intensity_t.Active, firstLap.Intensity);
            Assert.AreEqual(88, firstLap.Cadence);
            Assert.AreEqual(TriggerMethod_t.Manual, firstLap.TriggerMethod);
            Assert.AreEqual(317, firstLap.Track.Length);
            var firstTrackPoint = firstLap.Track[0];
            Assert.AreEqual(new System.DateTime(2020, 9, 19, 8, 26, 17), firstTrackPoint.Time);
            Assert.AreEqual(59.88717833, firstTrackPoint.Position.LatitudeDegrees);
            Assert.AreEqual(10.82581167, firstTrackPoint.Position.LongitudeDegrees);
            Assert.AreEqual(103.024, firstTrackPoint.AltitudeMeters);
            Assert.AreEqual(0.19999999494757503, firstTrackPoint.DistanceMeters);
            Assert.AreEqual(159, firstTrackPoint.HeartRateBpm.Value);
            Assert.AreEqual(0, firstTrackPoint.Cadence);
        }
    }
}