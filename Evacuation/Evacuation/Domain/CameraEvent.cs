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
                throw new ArgumentException("Camera ID cannot be null or empty", nameof(cameraId));
            if (peopleIn < 0 || peopleOut < 0)
                throw new ArgumentException("People count cannot be negative");

            CameraId = cameraId;
            Timestamp = timestamp;
            PeopleIn = peopleIn;
            PeopleOut = peopleOut;
        }
    }
}
