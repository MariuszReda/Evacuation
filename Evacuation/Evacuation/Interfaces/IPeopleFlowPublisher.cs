namespace Evacuation.Interface
{
    public interface IPeopleFlowPublisher : IAsyncDisposable
    {
        Task PublishNumberOfPeopleAsync(string cameraEvent);
    }
}
