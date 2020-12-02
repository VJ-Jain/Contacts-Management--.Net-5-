# Contacts Management

#### ASP.NET Core Project using .NET 5

---

* A simple Web API project to create, update & delete tenants and clients.
* Clients must belong to a tenant, so create a tenant first & then create a client.
* Once created, client can also be moved to a different tenant using PATCH endpoint.
* PredictAge API endpoint calls external endpoint (`https://api.agify.io/?name=<anyname>`)

This project uses -

* .NET Core 5.0
* Entity Framework Core
* Automapper
* Swagger
* IOptions (For Injecting configuration settings)
* Entity Framework InMemory (For tests)
* FluentAssertions  (For tests)
* xUnit  (For tests)
* WebApplicationFactory (For tests)
* Moq (For tests)

Update database name and connection string in `appsettings.json` & then run `dotnet ef database update`.

Api endpoints - <kbd>![Endpoints](./Endpoints.JPG)</kbd>

