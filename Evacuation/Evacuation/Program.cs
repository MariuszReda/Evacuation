using Evacuation;
using Evacuation.Interface;
using Evacuation.Mock;
using Microsoft.Extensions.DependencyInjection;


var serviceProvider = new ServiceCollection()
    .AddSingleton<ICameraEventDataStore, MockCameraEventDataStore>()
    .AddSingleton<IPeopleFlowPublisher, MockRabbitMqPeopleFlowPublisher>()
    .AddSingleton<CentralServer>()
    .BuildServiceProvider();


var server = serviceProvider.GetRequiredService<CentralServer>();
var publisher = serviceProvider.GetRequiredService<IPeopleFlowPublisher>();

var camera1 = new CameraSimulator("C1", publisher);
var camera2 = new CameraSimulator("C2", publisher);
var camera3 = new CameraSimulator("C3", publisher);


await camera1.GenerateEvent(2, 0);
await camera1.GenerateEvent(3, 1);
await camera2.GenerateEvent(1, 0);
await camera1.GenerateEvent(0, 1);
await camera3.GenerateEvent(4, 0);
await camera2.GenerateEvent(0, 1);

Console.WriteLine("System ewakuacji uruchomiony. Naciśnij Enter, aby zakończyć.");
Console.ReadLine();

