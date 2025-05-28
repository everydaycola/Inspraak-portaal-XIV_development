# Integratieproject 1 | 2024-2025

### Team 14_

## Inspraak portaal XIV [IP14]

Op ons platform IP14 kan een organisatie, gemeenschap, club, of eender wie anders, een account aanmaken, Hiermee kan u Panels hosten. 
Ons platvorm is vooral gemaakt voor het aanmaken van burgerpanels maar kan ook gebruikt worden voor kleinere panels. 
We bieden U een tool om uw panel samen te stellen, U kan mensen uitnodigen op basis van verschillende criteria. 
Wanneer de uitgenodigde beslissen deel te nemen aan het panel aan uw panel, zal u via ons een project pagina kunnen samenstellen waar u posts kunt maken en kunt communiceren met het panel. 
U kunt hier vergaderingen inplannen en stemming houden.

## Ons team
- [Ilja Nachtergaele](https://www.linkedin.com/in/ilja-nachtergaele-8665b622a/)
- [Stijn Similon](https://www.linkedin.com/in/stijn-similon-b86a27332/)
- [Tibo Eycken](https://www.linkedin.com/in/tibo-eycken-b81aaa1a3/)
- [Zachary Van De Staey](https://www.linkedin.com/in/zachary-van-de-staey-04ba56229/)

## Het runnen van ons project






### Running locally
The project utilizes env. variables for connecting to the database.

Our project uses environment variables for connecting to our data sources, the following config is required
```
Key: 'ConnectionStrings__DefaultConnection', Value: '<your local db connection string>'
// Default for our docker config: 'Host=localhost;Database=CitizenPanel_DB;Username=user;Password=password;'

```
Add 'ConnectionStrings__DefaultConnection' with value '<your local db connection string>' to your systems environment variables.
