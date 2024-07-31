var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.IMEB_ApiService>("apiservice");

builder.AddProject<Projects.IMEB_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();