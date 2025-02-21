using Evacuation.Domain;
using Evacuation.Interface;
using System.Text.Json;

namespace Evacuation
{

    //Symuluje prace kamery
    public class CameraSimulator
    {
        public string Id { get; }
        private readonly List<ICameraObserver> _observers = new List<ICameraObserver>();
        private readonly Random _random = new Random();
        private readonly IPeopleFlowPublisher _reportPublisher;

        public CameraSimulator(string id, IPeopleFlowPublisher raportPublisher)
        {
            Id = id;
            _reportPublisher = raportPublisher;
        }

        public async Task GenerateEvent(int peopleIn, int peopleOut)
        {
            var cameraEvent = new CameraEvent(Id, DateTime.UtcNow, peopleIn, peopleOut);
            await SendReportAsync(cameraEvent);
        }

        private async Task SendReportAsync(CameraEvent dataFromCameraOfEvent)
        {
            string message = $"[{dataFromCameraOfEvent.Timestamp}] Kamera {dataFromCameraOfEvent.CameraId}: " +
                    $"IN {dataFromCameraOfEvent.PeopleIn}, OUT {dataFromCameraOfEvent.PeopleOut}.";

            await _reportPublisher.PublishReportAsync(message);
        }

        //public async Task StartStreamingAsync(CancellationToken cancellationToken)
        //{
        //    while (!cancellationToken.IsCancellationRequested)
        //    {
        //        int peopleIn = _random.Next(0, 5);
        //        int peopleOut = _random.Next(0, 5);

        //        var data = new CameraEvent(Id, DateTime.UtcNow, peopleIn, peopleOut);

        //        foreach (var observer in _observers)
        //        {
        //            observer.UpdateOfPeopleCount(data);
        //        }

        //        await SendReportOfPeopleFlow(data);

        //        await Task.Delay(1000);
        //    }
        //}

    }
}
