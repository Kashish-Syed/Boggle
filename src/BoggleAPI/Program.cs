using BoggleEngines;
using BoggleContracts;
using BoggleAccessors;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddSingleton<IDatabaseGameInfo, DatabaseGameInfo>();
builder.Services.AddSingleton<IDatabasePlayerInfo, DatabasePlayerInfo>();
builder.Services.AddSingleton<IDatabaseWordInfo, DatabaseWordInfo>();

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