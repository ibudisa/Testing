

using DAL.Entities;
using DAL.Repositorys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using UsersAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddDbContext<UserDBContext>(options =>
//{
//    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//    options.UseMySQL(connectionString);
//});
//string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddDbContext<UserDBContext>(opt =>
//{
//opt.UseMySql(connectionString,
//                ServerVersion.AutoDetect(connectionString));

//});
//builder.Services.AddDbContextPool<UserDBContext>(
//      options => options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new System.Version(8, 0, 41)), options => options.EnableRetryOnFailure(
//                    maxRetryCount: 5,
//                    maxRetryDelay: System.TimeSpan.FromSeconds(30),
//                    errorNumbersToAdd: null))
//   );

var host = builder.Configuration["DBHOST"] ?? "localhost";
var port = builder.Configuration["DBPORT"] ?? "3306";
var password = builder.Configuration["DBPASSWORD"] ?? "dlswer_2asdlkj";

builder.Services.AddDbContext<UserDBContext>(opt =>
{
    opt.UseMySql($"server=db; user=root; password={password}; port={port}; database=userdata;", new MySqlServerVersion(new System.Version(8, 0, 41)));

});



builder.Services.AddScoped<IDataRepository, DataRepository>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.CreateDbIfNotExists();

app.Run();
