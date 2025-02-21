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
                Console.WriteLine("Otwarto połączenie z RabbitMQ");
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Błąd podczas otwierania połączenia: {ex.Message}");
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
                    Console.WriteLine("Otwarto połączenie z RabbitMQ");
                }

                Console.WriteLine("Wysłano komunikat do RabitMQ");
            }
            catch
            {
                throw;
            }
        }
        public async ValueTask DisposeAsync()
        {
            Console.WriteLine("Zamknięcie połączenia z RabbitMQ");
            _connection = false;
            GC.SuppressFinalize(this);
        }
    }
}
