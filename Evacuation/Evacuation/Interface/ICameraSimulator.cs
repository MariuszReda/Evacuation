using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evacuation.Interface
{
    public interface ICameraSimulator
    {
        Task<string> GenerateEventAsync(string cameraId);
        string GetCameraId();
    }
}
