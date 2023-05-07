using Microsoft.AspNetCore.Builder;
using Bookstore.Web.Startup;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.ConfigureConfiguration();

builder.ConfigureServices();

builder.ConfigureAuthentication();

builder.ConfigureDependencyInjection();

WebApplication app = builder.Build();

await app.ConfigureMiddlewareAsync();

app.Run();