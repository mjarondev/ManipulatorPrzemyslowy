
# Programowanie manipulatora przemysłowego
Program służący do komunikacji z robotem RV-E2/RV-E2M, umożliwiający programowanie go. Napisany w C# WPF.

## Spis treści
- [Informacje ogólne](#informacje-ogólne)
- [Technologie](#technologie)
- [Uruchamianie programu](#uruchamianie-programu)
- [Korzystanie z aplikacji](#korzystanie-z-aplikacji)
- [Status projektu](#status-projektu)

## Informacje ogólne
Program łączy się z robotem za pomocą portu COM. Umożliwia wysyłanie komend do robota i otrzymywanie danych z robota.

## Technologie
Projekt został napisany przy użyciu
- C#
- WPF
- .Net Core 3.1

Dodatkowe biblioteki
- System.IO.Ports v5.0.1

## Korzystanie z aplikacji
Program wykrywa aktywne porty COM, gdy nie wykryje żadnego, nie będzie mógł działać prawidłowo, niemożliwe stanie się połączenie, wysyłanie i otrzymywanie danych. Aby przetestować aplikację bez podłączania zewnętrznych urządzeń należy utworzyć wirtualne porty COM np. za pomocą aplikacji Virtual Serial Port Tools. Wysyłanie i odbieranie wiadomości można testować po utworzeniu wirtualnych portów COM np. za pomocą aplikacji Docklight.

## Status projektu
Program w trakcie rozbudowy

