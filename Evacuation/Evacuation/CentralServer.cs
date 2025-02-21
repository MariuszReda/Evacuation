using Evacuation.Domain;
using Evacuation.Interface;
using System.Collections.Concurrent;


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
            loadExistingZones();
        }

        public void StartListeningCameras(CancellationToken token)
        {
            Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        token.ThrowIfCancellationRequested();

                        var tasks = _cameraSimulators.Select(cam => cam.GenerateEventAsync(cam.GetCameraId()));
                        var events = await Task.WhenAll(tasks);

                        await using (_publisher)
                        await using (_repository)
                        {
                            foreach (var jsonEvent in events)
                            {
                                await _publisher.PublishNumberOfPeopleAsync(jsonEvent);
                                _repository.SaveEvent(CameraEventSerializer.FromJson(jsonEvent));
                                ProcessCameraEvent(jsonEvent);
                            }
                        }

                        Console.WriteLine($"Total number of people on site {_totalPeople}:");

                    }
                    catch (OperationCanceledException)
                    {
                        Console.WriteLine("Symulacja kamer została zatrzymana.");
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[ERROR] Wystąpił błąd w StartListeningCameras(): {ex.Message}");
                    }
                    await Task.Delay(5000, token);
                }
            }, token);
        }

        private void updateOfPeopleCount(CameraEvent data)
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
            Console.WriteLine($"Sygnał z kamery: {jsonEvent}");
            updateOfPeopleCount(cameraEvent);

        }

        public void RestoreFromAllCameraHistory()
        {
            if (_zones.Count == 0)
            {
                Console.WriteLine("Brak zapisanych kamer w systemie.");
                return;
            }

            foreach (var zone in _zones)
            {
                Console.WriteLine($"Historia zdarzeń dla kamery {zone.Key}:");
                var eventHistoryForZone = zone.Value.GetEventHistory();
                foreach (var eventHistory in eventHistoryForZone)
                {
                    Console.WriteLine($"{eventHistory.CameraId}, {eventHistory.Timestamp}, IN: {eventHistory.PeopleIn}, OUT {eventHistory.PeopleOut}");
                }
                Console.WriteLine($"Licznik w czasie rzeczywistym dla kamery {zone.Key}: {zone.Value.CurrentPeopleInZone}");
                Console.WriteLine("--------------------------------------");
            }

        }

        public void GetCurrentOccupancy(string zoneId)
        {

            if (_zones.TryGetValue(zoneId, out var zoneOccupancy))
            {
                var eventHistoryForZone = zoneOccupancy.GetEventHistory();
                foreach (var eventHistory in eventHistoryForZone)
                {
                    Console.WriteLine($"{eventHistory.CameraId}, {eventHistory.Timestamp}, IN: {eventHistory.PeopleIn}, OUT {eventHistory.PeopleOut}");
                }
                Console.WriteLine($"Licznik w czasie rzeczywistym dla kamery {zoneId}: {zoneOccupancy.CurrentPeopleInZone}");
            }
            else
            {
                Console.WriteLine($"Strefa {zoneId} nie została znaleziona.");
            }
        }

        private async Task loadExistingZones()
        {
            await using(_repository)
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
}
