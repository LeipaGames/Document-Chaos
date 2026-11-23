# Ruční testovací scénář
Tento dokument popisuje ruční testopvání základních herních mechanik a funkčnost hry **Document Chaos**
## Testovací případy

| ID | Název testu | Postup | Očekávaný výsledek | Skutečný výsledek | Stav |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **TC-01** | Spuštění a inicializace hry | Spustit spustitelný soubor hry `Document Chaos.exe`. | Hra se bez chyby načte, a zobrazí se úřad. | Hra se spustila plynule bez chyb. | Funguje |
| **TC-02** | Automatické vytvoření a nastavení místností | Zapnutím hry se správně vytvoří místnosti. | Po spuštění hry se místnostti samy vytvoří, pojmenují, přiřadí si ceny atd..., skryjí se, a zobrazí se pouze tehdy, až je zakoupená sousedící místnost. | Místnosti se vytvoří a zobrazují podle očekávání | Funguje |
| **TC-03** | Vytváření a pohyb návštěvníků | Pozorovat chování návštěvníků | Návštěvníci zaplní úřad, budou schopni si najít volnou místnost na patře, kam chtějí jít, popřípadně počkat ve frontě na uvolnění, a po dokončení úkolu v kanceláří, pokračovat dalším úkolem. | Návštěvníci se chovají podle představ | Funguje |
| **TC-04** | Nákup kanceláří | Kliknutím na nezakoupenou kancelář levým tlačítkem myši | Má-li hráč dostatek peněz, peníze se odečtou a místnost se odemče, tím bude viditelná návštěvníkům. | Místnost po dosažení dostatku peněz se dá zakoupit, a návštšvníci do ní začnou chodit. | Funguje |
| **TC-05** | Ukončení hry | Stisknout klávesu `ESC` | Hra se ukončí | Hra se ukončila | Funguje |

## Shrnutí testování
Všechny funkce hry fungují podle očekávání.
