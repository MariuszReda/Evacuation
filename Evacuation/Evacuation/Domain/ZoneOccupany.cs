using Evacuation.Interface;
using StackExchange.Redis;
using System.Text.Json;

namespace Evacuation.Domain
{
    public class ZoneOccupancy
    {
        public string CameraId { get; private set; }
        public int CurrentOccupancy { get; private set; }
        private List<CameraEvent> _eventHistory = new();
        private readonly ICameraEventDataStore _cameraEventRepository;
        public ZoneOccupancy(string cameraId, ICameraEventDataStore _cameraEventRepository)
        {
            CameraId = cameraId ?? throw new ArgumentNullException(nameof(cameraId));
            CurrentOccupancy = 0;
            _cameraEventRepository = _cameraEventRepository;
            LoadHistory();
        }

        public void ProcessEvent(CameraEvent cameraEvent)
        {
            _eventHistory.Add(cameraEvent);
            CurrentOccupancy += cameraEvent.PeopleIn - cameraEvent.PeopleOut;

            if (CurrentOccupancy < 0)
                CurrentOccupancy = 0;
        }
        public IReadOnlyList<CameraEvent> GetEventHistory() => _eventHistory.AsReadOnly();
        private void LoadHistory()
        {
            var history = _cameraEventRepository.LoadEvents(CameraId);
            foreach (var cameraEvent in history)
            {
                _eventHistory.Add(cameraEvent);
                CurrentOccupancy += cameraEvent.PeopleIn - cameraEvent.PeopleOut;
            }
        }

    }
}
