using Evacuation.Domain;
using Evacuation.Interface;
using System.Text.Json;

namespace Evacuation
{

    //Symuluje prace kamery
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

            await Task.Delay(_random.Next(1000, 5000)); // Symulacja opóźnienia
            var cameraEvent = new CameraEvent(
                cameraId,
                DateTime.UtcNow,
                _random.Next(0, 5),
                _random.Next(0, 5)
            );

            string message = $"[{cameraEvent.Timestamp}] Kamera {cameraEvent.CameraId}: " +
                $"IN {cameraEvent.PeopleIn}, OUT {cameraEvent.PeopleOut}.";

            return message;

        }
    }
}
