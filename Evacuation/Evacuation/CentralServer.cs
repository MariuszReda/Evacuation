using Evacuation.Domain;
using Evacuation.Interface;

namespace Evacuation
{
    public class CentralServer
    {
        private int _totalPeople = 0;
        private readonly Dictionary<string, ZoneOccupancy> _zones = new();
        private readonly List<ICameraSimulator> _cameraSimulators;
        private readonly ICameraEventDataStore _repository;
        private readonly IPeopleFlowPublisher _publisher;

        public CentralServer(ICameraEventDataStore repository, List<ICameraSimulator> cameraSimulators, IPeopleFlowPublisher publisher)
        {
            _repository = repository;
            _cameraSimulators = cameraSimulators;
            _publisher = publisher;
            LoadExistingZones();
            StartListeningCameras();
        }

        private void StartListeningCameras()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        var tasks = _cameraSimulators.Select(cam => cam.GenerateEventAsync(cam.GetCameraId()));
                        var events = await Task.WhenAll(tasks);

                        await using(_publisher)                        
                        {
                            foreach (var jsonEvent in events)
                            {
                                await _publisher.PublishNumberOfPeopleAsync(jsonEvent);
                                ProcessCameraEvent(jsonEvent);
                            }
                        }


                        Console.WriteLine($"Total number of people on site {_totalPeople}:");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[ERROR] Wystąpił błąd w StartListeningCameras(): {ex.Message}");
                    }
                    await Task.Delay(5000);
                }
            });
        }

        private void UpdateOfPeopleCount(CameraEvent data)
        {
            _totalPeople += data.PeopleIn - data.PeopleOut;
        }

        public void ProcessCameraEvent(string jsonEvent)
        {
            var cameraEvent = CameraEventSerializer.FromJson(jsonEvent);
            if (!_zones.ContainsKey(cameraEvent.CameraId))
            {
                _zones[cameraEvent.CameraId] = new ZoneOccupancy(cameraEvent.CameraId, _repository);
            }
            _zones[cameraEvent.CameraId].ProcessEvent(cameraEvent);
            Console.WriteLine($"Processed Event: {jsonEvent}");
            UpdateOfPeopleCount(cameraEvent);
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
