using Evacuation.Domain;
using Evacuation.Interface;
using System.Data.Common;
using System.Threading.Channels;

namespace Evacuation
{
    public class CentralServer : ICameraObserver
    {
        private int _totalPeople = 0;
        private readonly Dictionary<string, ZoneOccupancy> _zones = new();
        private readonly ICameraEventDataStore _repository;

        public CentralServer(ICameraEventDataStore repository)
        {
            _repository = repository;
            LoadExistingZones();
        }
        public void UpdateOfPeopleCount(CameraEvent data)
        {
            _totalPeople += data.PeopleIn - data.PeopleOut;
        }

        public void ProcessCameraEvent(CameraEvent cameraEvent)
        {
            if (!_zones.ContainsKey(cameraEvent.CameraId))
            {
                _zones[cameraEvent.CameraId] = new ZoneOccupancy(cameraEvent.CameraId, _repository);
            }
            _zones[cameraEvent.CameraId].ProcessEvent(cameraEvent);
        }

        public IReadOnlyList<CameraEvent>? GetHistoryForZone(string zoneId)
        {
            if (_zones.TryGetValue(zoneId, out var zone))
            {
                return zone.GetEventHistory();
            }
            return null;
        }

        public int GetCurrentOccupancy(string zoneId)
        {
            return _zones.TryGetValue(zoneId, out var zone) ? zone.CurrentOccupancy : 0;
        }

        private void LoadExistingZones()
        {
            var zones = _repository.GetAllZones();
            foreach (var zoneId in zones)
            {
                if (!_zones.ContainsKey(zoneId))
                {
                    _zones[zoneId] = new ZoneOccupancy(zoneId, _repository);
                }
            }
        }
    }
}
