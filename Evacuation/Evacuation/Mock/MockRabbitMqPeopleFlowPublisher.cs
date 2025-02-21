using Evacuation.Domain;
using Evacuation.Interface;

namespace Evacuation.Mock
{
    class MockRabbitMqPeopleFlowPublisher : IPeopleFlowPublisher
    {
        public MockRabbitMqPeopleFlowPublisher()
        {
            Connection();
        }
        public async Task PublishReportAsync(string dataToSend)
        {
            await Console.Out.WriteLineAsync(dataToSend);
        }
        private void Connection()
        {
            Console.WriteLine("Conntecion to rabitmq is success");
        }
    }
}
