using Evacuation.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evacuation.Interface
{
    public interface ICameraEventDataStore
    {
        void SaveEvent(string zoneId, CameraEvent cameraEvent);
        List<CameraEvent> LoadEvents(string zoneId);
        List<string> GetAllZones();
    }
}
