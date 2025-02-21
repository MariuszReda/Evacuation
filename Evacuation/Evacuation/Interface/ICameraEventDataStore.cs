using Evacuation.Domain;

namespace Evacuation.Interface
{
    public interface ICameraEventDataStore : IDisposable
    {
        void SaveEvent(CameraEvent cameraEvent);
        List<CameraEvent> LoadEvents(string zoneId);
        List<string> GetAllZones();
    }
}
