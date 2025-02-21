using Evacuation.Domain;

namespace Evacuation.Interface
{
    public interface ICameraEventDataStore : IAsyncDisposable
    {
        void SaveEvent(CameraEvent cameraEvent);
        List<CameraEvent> LoadEvents(string zoneId);
        List<string> GetAllZones();
    }
}
