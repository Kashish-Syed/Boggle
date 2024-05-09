using BoggleAccessors;
using BoggleContracts;
using BoggleEngines;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Configure connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "_MyAllowSubdomainPolicy",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();

            policy.WithOrigins("http://localhost:5173")
                       .AllowAnyHeader()
                       .AllowAnyMethod();
        }
    );
});

// Dependency Injections into Boggle Controller
builder.Services.AddSingleton<IGameDice, GameDice>();
builder.Services.AddSingleton<IWord, Word>();
builder.Services.AddSingleton<IGameSession, GameSession>();

// DIs for the database accessors
builder.Services.AddScoped<IDatabaseGameInfo>(provider =>
    new DatabaseGameInfo(new SqlConnection(connectionString)));

builder.Services.AddScoped<IDatabasePlayerInfo>(provider =>
    new DatabasePlayerInfo(new SqlConnection(connectionString)));

// Inject IWord here as well
builder.Services.AddScoped<IDatabaseWordInfo>(provider =>
    new DatabaseWordInfo(
        new SqlConnection(connectionString),
        provider.GetRequiredService<IWord>()
    ));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("_MyAllowSubdomainPolicy");
app.UseAuthorization();
app.MapControllers();
app.Run();