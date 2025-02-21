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
    }
}
