# Game Design Document - Document Chaos

## 1. Základní koncept
* **Název hry:** Document Chaos
* **Žánr:** 2D Tycoon
* **Cílová platforma:** PC (Windows/Linux)
* **Krátký popis:** Hráč spravuje rušný úřad, do kterého přicházejí návštěvníci vyřídit své dokumenty v různých patrech. Z vydělaných peněz hráč postupně odemyká nové kanceláře, zvyšuje kapacitu budovy a rozšiřuje úřad do vyšších pater.

## 2. Herní smyčka (Gameplay)
1. **Příchod návštěvníků:** Do úřadu průběžne přichází návštěvníci se seznamem pater, kam se musí dostat
2. **Hledání místnosti:** Návštěvníci si buď najdou prázdnou místnost, a tam se vydají, nebo se dostanou na požadované patro a počkají u výtahu.
3. **Zisk peněz:** Po vyřízení úkolu v kanceláři se hráči přičte částka, podle typu kanceláře, návštěvník kancelář opustí a čeká-li na uvolnění další, vejde tam po něm. Návštěvník pokračuje v seznamu další kanceláří.
4. **Expanze úřadu:** Pomocí získaných peněz si hráč postupně dokupuje další kanceláře na navýšení kapacity a dokupuje si další patra pro vyšší zisk.

 ## 3. Herní mechaniky
 * **Automatické vygenerování místností:** Při spuštění hry se automaticky vytvoří všechny místnosti, správně se nastaví (typ, pozice, vzhled, cena,...) a schovají se, dokud nebude zakoupena vedlejší místnost.
 * **Nákup místností a expanze:** Nákup místností spotřebuje určité množství peněz, místnost se zviditelní a odkryjí se sousední místnost.
 * **Vytváření návštěvníků:** Během hry se podle počtu místností vytváří určítý počet návštěvníků, ti při vytvoření dostanou náhodný vzhled a list úkolů. Pak postupně jdou do každého patra, pokud je na daném patře volná místnost, jsou vykonat svůj úkol, pokud ne tak čekají u výtahu. Po dokončení úkolu dostává hráč peníze a další čekající návštěvník může vejít do místnosti.

## 4. Vizuální a technické řešení
* **Engine a jazyk:** Hra je vytvořena v Unity za použití jazyku C#
* **Grafika:** Pixel artové sprity jsou vytvořeny v nástroji Piskelapp.com.
