namespace Evacuation.Interface
{
    public interface ICameraSimulator
    {
        Task<string> GenerateEventAsync(string cameraId);
        string GetCameraId();
    }
}
