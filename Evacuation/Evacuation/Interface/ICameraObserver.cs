using Evacuation.Domain;

namespace Evacuation.Interface
{
    public interface ICameraObserver
    {
        void UpdateOfPeopleCount(CameraEvent data);
    }
}
