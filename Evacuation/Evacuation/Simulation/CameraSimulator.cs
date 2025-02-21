using Evacuation.Domain;
using Evacuation.Infrastructure;
using Evacuation.Interface;

namespace Evacuation.Simulation
{
    public class CameraSimulator : ICameraSimulator
    {
        private readonly Random _random = new Random();
        private readonly string _cameraId;
        public CameraSimulator(string cameraId)
        {
            _cameraId = cameraId;
        }
        public string GetCameraId()
        {
            return _cameraId;
        }
        public async Task<string> GenerateEventAsync(string cameraId)
        {

            await Task.Delay(_random.Next(1000, 5000));
            var cameraEvent = new CameraEvent(
                cameraId,
                DateTime.UtcNow,
                _random.Next(0, 5),
                _random.Next(0, 5)
            );

            return CameraEventSerializer.ToJson(cameraEvent);
        }
    }
}
