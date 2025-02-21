using Evacuation.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evacuation.Interface
{
    public interface IPeopleFlowPublisher : IAsyncDisposable
    {
        Task PublishNumberOfPeopleAsync(string cameraEvent);
    }
}
