using Evacuation.Domain;
using Evacuation.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evacuation.Mock
{
    public class MockCameraEventDataStore : ICameraEventDataStore
    {
        private readonly ConcurrentDictionary<string, List<CameraEvent>> _storage = new();
        private readonly HashSet<string> _zones = new();
        public void SaveEvent(string zoneId, CameraEvent cameraEvent)
        {
            if (!_storage.ContainsKey(zoneId))
            {
                _storage[zoneId] = new List<CameraEvent>();
                _zones.Add(zoneId);
            }
            _storage[zoneId].Add(cameraEvent);
        }

        public List<CameraEvent> LoadEvents(string zoneId)
        {
            return _storage.TryGetValue(zoneId, out var events) ? new List<CameraEvent>(events) : new List<CameraEvent>();
        }

        public List<string> GetAllZones()
        {
            return _zones.ToList();
        }
    }
}
