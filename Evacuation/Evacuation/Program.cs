using Evacuation;
using Evacuation.Interface;
using Evacuation.Mock;
using Microsoft.Extensions.DependencyInjection;


var cameraIds = new List<string> { "C1", "C2", "C3" };
var cameraSimulators = cameraIds.Select(id => new CameraSimulator(id)).ToList();

var serviceProvider = new ServiceCollection()
    .AddSingleton<ICameraEventDataStore, MockCameraEventDataStore>()
    .AddSingleton(sp => cameraIds.Select(id => new CameraSimulator(id)).Cast<ICameraSimulator>().ToList())
    .AddSingleton<CentralServer>(sp =>
        new CentralServer(
            sp.GetRequiredService<ICameraEventDataStore>(),
            sp.GetRequiredService<List<ICameraSimulator>>()
        ))
    .BuildServiceProvider();


var server = serviceProvider.GetRequiredService<CentralServer>();


Console.WriteLine("System ewakuacji uruchomiony. Naciśnij Enter, aby zakończyć.");
Console.ReadLine();

