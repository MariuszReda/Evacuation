namespace Evacuation.Domain
{
    public record CameraEvent
    {
        public string CameraId { get; }
        public DateTime Timestamp { get; }
        public int PeopleIn { get; }
        public int PeopleOut { get; }

        public CameraEvent(string cameraId, DateTime timestamp, int peopleIn, int peopleOut)
        {
            if (string.IsNullOrWhiteSpace(cameraId))
                throw new ArgumentException("Camera ID nie może być null", nameof(cameraId));
            if (peopleIn < 0 || peopleOut < 0)
                throw new ArgumentException("Ludzie nie mogą być na minusie");

            CameraId = cameraId;
            Timestamp = timestamp;
            PeopleIn = peopleIn;
            PeopleOut = peopleOut;
        }
    }
}
