using Evacuation.Domain;
using Evacuation.Interface;
using StackExchange.Redis;
using System.Text.Json;

namespace Evacuation
{
    class CameraEventDataStore : ICameraEventDataStore
    {
        private static readonly Lazy<ConnectionMultiplexer> _lazyConnection =
           new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect("localhost"));
        private readonly IDatabase _redisDb;
        public CameraEventDataStore()
        {
            _redisDb = _lazyConnection.Value.GetDatabase();
        }

        public void SaveEvent(string zoneId, CameraEvent cameraEvent)
        {
            var eventData = JsonSerializer.Serialize(cameraEvent);
            _redisDb.ListRightPush($"zone:{zoneId}:events", eventData);
        }

        public List<CameraEvent> LoadEvents(string zoneId)
        {
            var history = _redisDb.ListRange($"zone:{zoneId}:events");
            var events = new List<CameraEvent>();

            foreach (var record in history)
            {
                var cameraEvent = JsonSerializer.Deserialize<CameraEvent>(record);
                if (cameraEvent != null)
                {
                    events.Add(cameraEvent);
                }
            }
            return events;
        }

        public List<string> GetAllZones()
        {
            throw new NotImplementedException();
        }
    }
}
