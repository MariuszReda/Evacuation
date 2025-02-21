namespace Evacuation.Domain
{
    //CameraEvent reprezentuje zdarzenie, które zawiera informacje o liczbie osób wchodzących i wychodzących w określonym momencie.
    public record CameraEvent(string CameraId, DateTime Timestamp, int PeopleIn, int PeopleOut);
}
