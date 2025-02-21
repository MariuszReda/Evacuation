 # Decentralized Camera-Based Evacuation 

 Podział na warstwy i struktura katalogów:
 ## Domain – Zawiera modele biznesowe (CameraEvent, ZoneOccupancy), które są centralnym punktem logiki systemu.
 ## Infrastructure – Odpowiada za implementację dostępu do danych (MockCameraEventDataStore, MockPeopleFlowPublisher), co pozwala na łatwą podmianę warstwy danych.
 ## Interfaces – Definiuje interfejsy (ICameraEventDataStore, ICameraSimulator, IPeopleFlowPublisher), co wspiera zasady SOLID, zwłaszcza DIP (Dependency Inversion Principle).
 ## Simulation – Odpowiada za symulację kamer (CameraSimulator) i przetwarzanie danych (CentralServer).
 ## Application – Obsługuje sterowanie aplikacją (StartApplication, Program.cs), co pozwala na łatwe zarządzanie cyklem życia systemu.
