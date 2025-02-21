using Evacuation.Domain;

namespace Evacuation.Interface
{
    public interface IPeopleFlowPublisher
    {
        Task PublishReportAsync(string dataToSend);
    }
}
