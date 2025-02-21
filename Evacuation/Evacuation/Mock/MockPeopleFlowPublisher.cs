using Evacuation.Domain;
using Evacuation.Interface;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evacuation.Mock
{
    class MockPeopleFlowPublisher : IPeopleFlowPublisher
    {
        private bool _connection;
        public MockPeopleFlowPublisher()
        {
            _connection = Connection().GetAwaiter().GetResult();
        }
        private async Task<bool> Connection()
        {
            try
            {
                Console.WriteLine("Open Connecion to RabitMQ");
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"[ERROR] Błąd przy otwieraniu połączenia: {ex.Message}");
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
                    await Console.Out.WriteLineAsync("Nawiązanie połączenia");
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
