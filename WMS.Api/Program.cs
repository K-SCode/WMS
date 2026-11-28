using WMS.Infrastructure.Data;
using Mapster;
using WMS.Application;
using WMS.Infrastructure;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddMapster();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<ICurrentUser, CurrentUser>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await IdentitySeeder.SeedUserAsync(app.Services);
}
await IdentitySeeder.SeedRolesAsync(app.Services);


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
