using Microsoft.AspNetCore.Builder;
using Bookstore.Web.Startup;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

WebApplication app = builder.ConfigureConfiguration()
    .ConfigureServices()
    .ConfigureAuthentication()
    .ConfigureDependencyInjection()
    .Build();

await app.ConfigureMiddlewareAsync();

app.Run();