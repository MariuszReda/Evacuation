using Evacuation.Interface;

namespace Evacuation.Domain
{
    public class ZoneOccupancy
    {
        public string CameraId { get; private set; }
        public int CurrentPeopleInZone { get; private set; }
        private List<CameraEvent> _eventHistory = new();
        private readonly ICameraEventDataStore _cameraEventRepository;
        public ZoneOccupancy(string cameraId, ICameraEventDataStore cameraEventRepository)
        {
            CameraId = cameraId ?? throw new ArgumentNullException(nameof(cameraId));
            CurrentPeopleInZone = 0;
            _cameraEventRepository = cameraEventRepository;
            LoadHistory();
        }

        public void ProcessEvent(CameraEvent cameraEvent)
        {
            _eventHistory.Add(cameraEvent);
            CurrentPeopleInZone += cameraEvent.PeopleIn - cameraEvent.PeopleOut;



            if (CurrentPeopleInZone < 0)
                CurrentPeopleInZone = 0;
        }
        public IReadOnlyList<CameraEvent> GetEventHistory() => _eventHistory.AsReadOnly();

        private void LoadHistory()
        {
            var history = _cameraEventRepository.LoadEvents(CameraId);
            foreach (var cameraEvent in history)
            {
                _eventHistory.Add(cameraEvent);
                CurrentPeopleInZone += cameraEvent.PeopleIn - cameraEvent.PeopleOut;
            }
        }

    }
}
