# Inspraak Portaal - Team 14_
Inspraak portaal is a platform which simplifies the hosting process of citizenpanels.
# Running the project locally.
## Getting the database running.

## Environment variables
Our project uses environment variables for connecting to our data sources, the following config is required
```
Key: 'ConnectionStrings__DefaultConnection', Value: '<your local db connection string>'
// Default for our docker config: 'Host=localhost;Database=CitizenPanel_DB;Username=user;Password=password;'

```
Add 'ConnectionStrings__DefaultConnection' with value '<your local db connection string>' to your systems environment variables.
