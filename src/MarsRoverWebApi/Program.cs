using MarsRoverWebApi.Services;
using MarsRoverWebApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Enable CORS to allow the MVC app to call this API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMvc", corsBuilder =>
    {
        corsBuilder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
    });
});

// Register application services
// IHistoryRepository: Handles persistence of simulation history (JSON-based)
builder.Services.AddSingleton<IHistoryRepository, JsonHistoryRepository>();

// IRoverSimulationService: Contains the core rover simulation logic
// This service handles all rover movement, rotation, and command processing
builder.Services.AddSingleton<IRoverSimulationService, RoverSimulationService>();

// Add API documentation with Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Apply CORS policy
app.UseCors("AllowMvc");

app.UseAuthorization();

app.MapControllers();

app.Run();
