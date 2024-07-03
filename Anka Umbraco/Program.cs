
using Microsoft.AspNetCore.Cors.Infrastructure;
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
var corsPolicy = new CorsPolicyBuilder()
    .WithOrigins("http://localhost:8080", "http://localhost:8082", "http://localhost:8081", "https://ankatedarik.com.tr", "http://ankatedarik.com.tr" ,"https://ankatedarik.com", "http://ankatedarik.com") // Add any other origins as needed
    .AllowAnyMethod()
    .AllowAnyHeader()
    .Build();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", corsPolicy);
});


builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddDeliveryApi()
    .AddComposers()
    .Build();

WebApplication app = builder.Build();

await app.BootUmbracoAsync();

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
        u.UseWebsite();
    })
    .WithEndpoints(u =>
    {
        u.UseInstallerEndpoints();
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });

await app.RunAsync();
