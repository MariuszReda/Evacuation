using Evacuation.Domain;
using System.Text.Json;

namespace Evacuation
{
    public static class CameraEventSerializer
    {
        public static string ToJson(CameraEvent cameraEvent) => JsonSerializer.Serialize(new
        {
            camera_id = cameraEvent.CameraId,
            timestamp = cameraEvent.Timestamp.ToString("o"),
            @in = cameraEvent.PeopleIn,
            @out = cameraEvent.PeopleOut
        });

        public static CameraEvent FromJson(string json)
        {
            var data = JsonSerializer.Deserialize<JsonCameraEvent>(json);
            return new CameraEvent(data.camera_id, DateTime.Parse(data.timestamp), data.@in, data.@out);
        }

        private record JsonCameraEvent(string camera_id, string timestamp, int @in, int @out);
    }
}
