using Microsoft.Extensions.DependencyInjection;

namespace Evacuation
{
    public class StartAplication
    {
        private readonly CentralServer centralServer;

        public StartAplication(IServiceProvider serviceProvider)
        {
            centralServer = serviceProvider.GetRequiredService<CentralServer>();

        }
        //public async Task StartApp()
        //{
        //    var cts = new CancellationTokenSource();
        //    var cameraSimulators = new List<Task>();
        //    bool isSimulationRunning = false;

        //    Console.WriteLine("System monitorowania ewakuacji uruchomiony. Wpisz 'help' aby zobaczyć dostępne komendy.");

        //    while (true)
        //    {
        //        Console.Write("> ");
        //        var input = Console.ReadLine()?.Trim().ToLower();

        //        if (input == "exit")
        //        {
        //            cts.Cancel();
        //            break;
        //        }
        //        if (input == "help")
        //        {
        //            Console.WriteLine("Dostępne komendy:");
        //            Console.WriteLine("  start  - Rozpoczęcie symulacji kamer");
        //            Console.WriteLine("  stop  - Zatrzymanie symulacji kamer");
        //            Console.WriteLine("  loadhistory  - Wczytaj dane historyczne");
        //            Console.WriteLine("  viewzone [zoneId]  - Podgląd danych dla konkretnej strefy");
        //            Console.WriteLine("  exit  - Wyjście z programu");
        //            continue;
        //        }

        //        var parts = input?.Split(' ');
        //        if (parts == null || parts.Length == 0) continue;

        //        switch (parts[0])
        //        {
        //            case "start":
        //                if (isSimulationRunning)
        //                {
        //                    Console.WriteLine("Symulacja już trwa.");
        //                    continue;
        //                }
        //                isSimulationRunning = true;
        //                cameraSimulators.Clear();
        //                centralServer.StartListeningCameras(cts.Token); 
        //                Console.WriteLine("Symulacja kamer rozpoczęta.");
        //                break;

        //            case "stop":
        //                if (!isSimulationRunning)
        //                {
        //                    Console.WriteLine("Symulacja nie jest uruchomiona.");
        //                    continue;
        //                }
        //                cts.Cancel();
        //                isSimulationRunning = false;
        //                Console.WriteLine("Symulacja kamer zatrzymana.");
        //                break;

        //            case "loadhistory":
        //                Console.WriteLine("Wczytywanie danych historycznych...");
        //                centralServer.RestoreCameraHistory();
        //                Console.WriteLine("Dane historyczne zostały wczytane.");
        //                break;

        //            case "viewzone":
        //                if (parts.Length < 2) { Console.WriteLine("Błąd: Podaj ZoneId."); continue; }
        //                string zoneId = parts[1];
        //                centralServer.GetCurrentOccupancy(zoneId);
        //                break;

        //            default:
        //                Console.WriteLine("Nieznana komenda. Wpisz 'help' aby zobaczyć listę dostępnych komend.");
        //                break;
        //        }
        //    }

        //    await Task.WhenAll(cameraSimulators);
        //}

        public async Task StartApp()
        {
            var cts = new CancellationTokenSource();
            var cameraSimulators = new List<Task>();
            bool isSimulationRunning = false;

            Console.WriteLine("System monitorowania ewakuacji uruchomiony. Wpisz 'help' aby zobaczyć dostępne komendy.");

            while (true)
            {
                Console.Write("> ");
                var input = Console.ReadLine()?.Trim().ToLower();

                if (input == "exit")
                {
                    cts.Cancel();
                    break;
                }
                if (input == "help")
                {
                    Console.WriteLine("Dostępne komendy:");
                    Console.WriteLine("  start  - Rozpoczęcie symulacji kamer");
                    Console.WriteLine("  stop  - Zatrzymanie symulacji kamer");
                    Console.WriteLine("  loadhistory  - Wczytywanie danych historycznych z bazy danych ze wszystkich kamer");
                    Console.WriteLine("  viewzone [zoneId]  - Podgląd danych dla wybranej kamery (np. viewzone c1)");
                    Console.WriteLine("  exit  - Wyjście z programu");
                    continue;
                }

                var parts = input?.Split(' ');
                if (parts == null || parts.Length == 0) continue;

                switch (parts[0])
                {
                    case "start":
                        if (isSimulationRunning)
                        {
                            Console.WriteLine("Symulacja już trwa.");
                            continue;
                        }
                        isSimulationRunning = true;
                        cameraSimulators.Clear();

                        if (cts.IsCancellationRequested)
                        {
                            cts.Dispose();
                            cts = new CancellationTokenSource();
                        }

                        centralServer.StartListeningCameras(cts.Token);
                        Console.WriteLine("Symulacja kamer rozpoczęta.");
                        break;

                    case "stop":
                        if (!isSimulationRunning)
                        {
                            Console.WriteLine("Symulacja nie jest uruchomiona.");
                            continue;
                        }
                        cts.Cancel();
                        isSimulationRunning = false;
                        Console.WriteLine("Symulacja kamer zatrzymana.");
                        break;

                    case "loadhistory":
                        Console.WriteLine("Wczytywanie danych historycznych z bazy danych ze wszystkich kamer...");
                        centralServer.RestoreFromAllCameraHistory();
                        Console.WriteLine("Dane historyczne zostały wczytane.");
                        break;

                    case "viewzone":
                        if (parts.Length < 2) { Console.WriteLine("Błąd: Podaj ZoneId."); continue; }
                        string zoneId = parts[1];
                        centralServer.GetCurrentOccupancy(zoneId.ToUpper());
                        break;

                    default:
                        Console.WriteLine("Nieznana komenda. Wpisz 'help' aby zobaczyć listę dostępnych komend.");
                        break;
                }
            }

            await Task.WhenAll(cameraSimulators);
        }

    }
}
