using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PrivacyBudgetServer.Models;
using PrivacyBudgetServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database

builder.Services.Configure<PrivacyBudgetDatabaseSettings>(builder.Configuration.GetSection("PrivacyBudgetDatabase"));
builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(sp.GetService<IOptions<PrivacyBudgetDatabaseSettings>>().Value.ConnectionString));
builder.Services.AddSingleton<IMongoDatabase>(sp => sp.GetService<IMongoClient>().GetDatabase(sp.GetService<IOptions<PrivacyBudgetDatabaseSettings>>().Value.DatabaseName));
builder.Services.AddSingleton<TransactionService>();
builder.Services.AddSingleton<ICRUDService<Transaction>>(sp => sp.GetService<TransactionService>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
