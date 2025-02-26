 # Decentralized Camera-Based Evacuation 

 ## Podział na warstwy i struktura katalogów:
System został podzielony na logiczne moduły, które zapewniają jego elastyczność, skalowalność oraz czytelność kodu.

1️⃣ Domain – Modele biznesowe 
Zawiera centralne modele logiki systemu: <br />
CameraEvent.cs – Reprezentuje zdarzenia rejestrowane przez kamery. <br />
ZoneOccupancy.cs – Zarządza historią zdarzeń dla poszczególnych kamer. <br />

2️⃣ Infrastructure – Implementacja dostępu do danych
Odpowiada za operacje na danych oraz komunikację z systemami zewnętrznymi: <br />
MockCameraEventDataStore.cs – Symuluje przechowywanie i pobieranie danych zdarzeń. <br />
MockPeopleFlowPublisher.cs – Symuluje wysyłanie informacji do systemów kolejkowych (np. RabbitMQ, Kafka). <br />

3️⃣ Interfaces – Abstrakcje i interfejsy
Definiuje interfejsy dla poszczególnych komponentów systemu, co zapewnia elastyczność i modularność: <br />
ICameraEventDataStore.cs – Interfejs dostępu do danych zdarzeń. <br />
ICameraSimulator.cs – Interfejs symulacji działania kamer. <br />
IPeopleFlowPublisher.cs – Interfejs wysyłania zdarzeń do systemu kolejkowego. <br />

4️⃣ Simulation – Symulacja kamer i zarządzanie zdarzeniami
Odpowiada za generowanie i przetwarzanie danych z kamer w czasie rzeczywistym: <br />
CameraSimulator.cs – Symuluje działanie kamer i generowanie zdarzeń. <br />
CentralServer.cs – Odpowiada za przetwarzanie zdarzeń i zarządzanie danymi w systemie. <br />

5️⃣ Application – Uruchamianie i sterowanie systemem
Odpowiada za zarządzanie cyklem życia aplikacji i interakcję użytkownika: <br />
StartApplication.cs – Obsługuje wprowadzanie komend oraz uruchamianie procesów w systemie. <br />
Program.cs – Główny plik startowy aplikacji. <br />

 ## Wykorzystane wzorce projektow:
 Dependency Injection (DI) – Umożliwia łatwe zarządzanie zależnościami i modularność kodu. <br />
 Repository Pattern – Abstrahuje operacje na danych, ułatwiając testowanie i wymianę warstwy danych. <br />
 Observer Pattern – CentralServer nasłuchuje zdarzeń generowanych przez CameraSimulator. <br />
 
 ## Jak uruchomić
skompiluj całe rozwiązanie uruchom plik wykonalny
```bash
Evacuation.exe
```
 
