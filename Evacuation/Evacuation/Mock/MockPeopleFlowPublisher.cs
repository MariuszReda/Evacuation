using Evacuation.Interface;

namespace Evacuation.Mock
{
    class MockPeopleFlowPublisher : IPeopleFlowPublisher
    {
        private bool _connection;

        private async Task<bool> Connection()
        {
            try
            {
                Console.WriteLine("Open Connecion to RabitMQ");
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"[ERROR] opening connection: {ex.Message}");
                return false;
            }
        }
        public async Task PublishNumberOfPeopleAsync(string cameraEvent)
        {
            try
            {
                if (!_connection) 
                {
                    _connection = await Connection();
                    await Console.Out.WriteLineAsync("Open Connecionto RabitMQ");
                }

                await Console.Out.WriteLineAsync("Publish message");
            }
            catch
            {
                throw;
            }
        }
        public async ValueTask DisposeAsync()
        {
            Console.WriteLine("Close connection to RabbitMQ");
            _connection = false;
            GC.SuppressFinalize(this);
        }
    }
}
