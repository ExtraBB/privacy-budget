using PrivacyBudgetServer.Models;
using PrivacyBudgetServer.Models.Database;
using PrivacyBudgetServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.Configure<PrivacyBudgetDatabaseSettings>(builder.Configuration.GetSection("Database"));
builder.Services.AddSingleton<DatabaseService>();
builder.Services.AddSingleton<ICRUDService<Transaction>>(sp => new CRUDService<Transaction>(sp.GetService<DatabaseService>()?.TransactionCollection));
builder.Services.AddSingleton<ICRUDService<Account>>(sp => new CRUDService<Account>(sp.GetService<DatabaseService>()?.AccountCollection));
builder.Services.AddSingleton<ICRUDService<Rule>>(sp => new CRUDService<Rule>(sp.GetService<DatabaseService>()?.RuleCollection));
builder.Services.AddSingleton<RuleProcessingService>();

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
