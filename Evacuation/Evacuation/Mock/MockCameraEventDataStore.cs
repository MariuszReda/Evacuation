using Evacuation.Domain;
using Evacuation.Interface;
using System.Collections.Concurrent;
using System.Data.Common;

namespace Evacuation.Mock
{
    public class MockCameraEventDataStore : ICameraEventDataStore
    {
        private readonly ConcurrentDictionary<string, List<CameraEvent>> _storage = new();
        private readonly HashSet<string> _zones = new();

        public MockCameraEventDataStore()
        {
            _storage = InitializeMockData();
        }
        public void SaveEvent(CameraEvent cameraEvent)
        {
            if (!_storage.ContainsKey(cameraEvent.CameraId))
            {
                _storage[cameraEvent.CameraId] = new List<CameraEvent>();
                _zones.Add(cameraEvent.CameraId);
            }
            _storage[cameraEvent.CameraId].Add(cameraEvent);
        }

        public List<CameraEvent> LoadEvents(string zoneId)
        {
            return _storage.TryGetValue(zoneId, out var events) ? new List<CameraEvent>(events) : new List<CameraEvent>();
        }

        public List<string> GetAllZones()
        {
            return _zones.ToList();
        }

        public void Dispose()
        {
            Console.WriteLine("Close connection to DB");
            GC.SuppressFinalize(this);
        }
        private static ConcurrentDictionary<string, List<CameraEvent>> InitializeMockData()
        {
            return new ConcurrentDictionary<string, List<CameraEvent>>
            {
                ["C1"] = new List<CameraEvent>
                {
                    new CameraEvent("C1", DateTime.UtcNow.AddDays(-1).AddMinutes(-30), 2, 1),
                    new CameraEvent("C1", DateTime.UtcNow.AddDays(-1).AddMinutes(-15), 4, 2),
                    new CameraEvent("C1", DateTime.UtcNow.AddDays(-1), 5, 3)
                },
                ["C2"] = new List<CameraEvent>
                {
                    new CameraEvent("C2", DateTime.UtcNow.AddDays(-1).AddMinutes(-45), 3, 2),
                    new CameraEvent("C2", DateTime.UtcNow.AddDays(-1).AddMinutes(-20), 6, 3),
                    new CameraEvent("C2", DateTime.UtcNow.AddDays(-1), 2, 1)
                },
                ["C3"] = new List<CameraEvent>
                {
                    new CameraEvent("C3", DateTime.UtcNow.AddDays(-1).AddMinutes(-50), 1, 1),
                    new CameraEvent("C3", DateTime.UtcNow.AddDays(-1).AddMinutes(-25), 7, 4),
                    new CameraEvent("C3", DateTime.UtcNow.AddDays(-1), 3, 2)
                }
            };
        }
    }
}
