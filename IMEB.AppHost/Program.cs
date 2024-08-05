var builder = DistributedApplication.CreateBuilder(args);

var keycloak = builder.AddKeycloak("keycloak", 8080)
    .WithDataVolume()
    .WithImageTag("25.0")
    .WithArgs("--features", "organization");

var apiService = builder.AddProject<Projects.IMEB_ApiService>("apiservice")
    .WithReference(keycloak)
    .WithEnvironment("Realm", "IMEB");

builder.AddProject<Projects.IMEB_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WithReference(keycloak)
    .WithEnvironment("Realm", "IMEB");

builder.Build().Run();