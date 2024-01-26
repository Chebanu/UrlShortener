using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PassGuardia.Domain.Commands;
using UrlShortener.Domain.Algorithm;
using UrlShortener.Domain.DbContexts;
using UrlShortener.Domain.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddDbContext<UrlShortenerDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
}, ServiceLifetime.Scoped);

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IUrlShortenerAlgoritm, UrlShortenerAlgoritm>();

builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(CreateShortUrlCommand).Assembly));

builder.Services.AddMvc(options =>
{

});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

await app.RunAsync();
