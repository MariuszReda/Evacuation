using Evacuation;
using Evacuation.Interface;
using Evacuation.Mock;
using Microsoft.Extensions.DependencyInjection;


var cameraIds = new List<string> { "C1", "C2", "C3" };
var cameraSimulators = cameraIds.Select(id => new CameraSimulator(id)).ToList();

var serviceProvider = new ServiceCollection()
    .AddSingleton<ICameraEventDataStore, MockCameraEventDataStore>()
    .AddSingleton<IPeopleFlowPublisher, MockPeopleFlowPublisher>()
    .AddSingleton(sp => cameraIds.Select(id => (ICameraSimulator)new CameraSimulator(id)).ToList())
    .AddSingleton<CentralServer>(sp =>
        new CentralServer(
            sp.GetRequiredService<ICameraEventDataStore>(),
            sp.GetRequiredService<List<ICameraSimulator>>(),
            sp.GetRequiredService<IPeopleFlowPublisher>()
        ))
    .AddSingleton<StartAplication>()
    .BuildServiceProvider();


var start = serviceProvider.GetRequiredService<StartAplication>();

await start.StartApp();


