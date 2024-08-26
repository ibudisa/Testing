using Microsoft.EntityFrameworkCore;
using DAL.Data;
using DAL.Repositorys;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddResponseCaching();
builder.Services.AddControllers(option =>
{
    option.CacheProfiles.Add("120SecondsDuration",
                             new CacheProfile { Duration = 120 });
});
var connectionString = builder.Configuration.GetConnectionString("myconn");
builder.Services.AddDbContext<WeatherDBContext>(x => x.UseSqlServer(connectionString));

builder.Services.AddScoped<IDataRepository, DataRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
