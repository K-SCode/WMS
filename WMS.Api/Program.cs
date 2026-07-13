using Microsoft.EntityFrameworkCore;
using WMS.Application.Interfaces;
using WMS.Infrastructure.Data;
using WMS.Infrastructure.Repositories;
using Mapster;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddMapster();

var connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<IAppDbContext>(sp =>
    sp.GetRequiredService<ApplicationDbContext>());

builder.Services.AddScoped<IUnitOfWorks, UnitOfWorks>();
builder.Services.AddMediator();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
