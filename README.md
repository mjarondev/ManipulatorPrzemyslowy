
# Programowanie manipulatora przemysłowego
Program służący do komunikacji z robotem RV-E2/RV-E2M, umożliwiający programowanie go. Napisany w C# WPF.

## Spis treści
- [Informacje ogólne](#informacje-ogólne)
- [Technologie](#technologie)
- [Uruchamianie programu](#uruchamianie-programu)
- [Korzystanie z aplikacji](#korzystanie-z-aplikacji)
- [Status projektu](#status-projektu)
- [Screeny projektu](#screeny-projektu)

## Informacje ogólne
Program łączy się z robotem za pomocą portu COM. Umożliwia wysyłanie komend do robota i otrzymywanie danych z robota.

## Technologie
Projekt został napisany przy użyciu
- C#
- WPF
- .Net Core 3.1

Dodatkowe biblioteki
- System.IO.Ports v5.0.1

## Uruchamianie programu
Aby skompilować program konieczne jest zainstalowanie dodatkowej biblioteki System.IO.Ports. W Visual Studio można to zrobić za pomocą NuGet.  

Program wykrywa aktywne porty COM, gdy nie wykryje żadnego, nie będzie mógł działać prawidłowo, niemożliwe stanie się połączenie, wysyłanie i otrzymywanie danych. Aby przetestować aplikację bez podłączania zewnętrznych urządzeń należy utworzyć wirtualne porty COM np. za pomocą aplikacji [Virtual Serial Port Tools](https://freevirtualserialports.com/). Wysyłanie i odbieranie wiadomości można testować po utworzeniu wirtualnych portów COM np. za pomocą aplikacji [Docklight](https://docklight.de/). 

## Korzystanie z aplikacji
Po uruchomieniu aplikacji pokaże się okno główne i jeżeli jest taka możliwość załadowane zostaną domyślne dane połączenia z portem COM. Przy chęci zmienienia danych połączenia lub gdy nie uda się ich załadować w oknie Communication Port można ustawić ponownie dane połączenia bądź odświeżyć je do ustawień domyślnych za pomocą przycisku strzałki. W oknie Command Tool można wpisywać i wysyłać pojedyncze komendy. Są one wylistowane i podana jest składnia każdej z nich. W oknie Jog Operator można wysyłać dane o przemieszczeniu manipulatora za pomocą przycisków. Aby wysyłać i odbierać dane konieczne jest połączenie się za pomocą przycisku Connect.

## Status projektu
Program w trakcie rozbudowy

## Screeny projektu
![MainWindow](https://user-images.githubusercontent.com/82177599/117480593-193cc180-af62-11eb-8e3d-e6d1f1631a42.png)  
![CommunicationPort](https://user-images.githubusercontent.com/82177599/117480483-f6121200-af61-11eb-86f9-9ae61b9702c4.png)  
![JogOperator](https://user-images.githubusercontent.com/82177599/117480505-fdd1b680-af61-11eb-842a-8fa0104a9071.png)  
![CommandTool](https://user-images.githubusercontent.com/82177599/117480532-05915b00-af62-11eb-9106-8f2b50ff940e.png)  


