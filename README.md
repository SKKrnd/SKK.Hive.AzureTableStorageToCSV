# SKK.Hive.AzureTableStorageToCSV

Aplikacja służy do eksportu danych z sensorów zapisanych w Azure Table Storage do plików CSV.

## Omówienie

Aplikacja pozwala na eksport danych pochodzących z określonego dnia z zakresu ostatnich 7 dni.
Dane eksportowane są do plików CSV, których nazwy odpowiadają tabelom. Pliki generowane są w folderach z zachowaniem  struktury rok, miesiąc, dzień.

		.
		├── ...
		├── 2019
		|	├──  1                         
		|	|	├──  1
		│	│	│	├── 2019.01.01_seen.csv
		│	│	│	├── 2019.01.01_temperature.csv 
		|	|	|	└── ...
		│	│   	└── ...    
		│   	└── ...             
		└── ...



## Konfiguracja

Należy uzupełnić **connectionString** do zasobu Azure Storage.

``` C#

	static async Task Main(string[] args)
			{
				CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse("<connectionString>");
				
```

## Uruchomienie aplikacji

Aplikacja przyjmuje 2 argumenty:

```

	-p\--path : ścieżka do bazowej lokalizacji eksportowanych plików
	-d\--date : dzień, z którego eksportowane są odczyty (dzień z zakresu ostatnich 7 dni)
 
```


## Pomoc
W celu uzyskania pomoc, proszę kontaktować się z działem R&D firmy [SKK S.A](https://www.skk.com.pl/pl/kontakt.html).

![RnD Logo](logo_RnD.jpg)