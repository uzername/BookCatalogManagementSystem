// this file is weird because I forgot to enable some option on project init heheh
using BCMS_Backend.Repository;
using BCMS_Backend.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Add CORS policy to allow all origins. CORS is annoying
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin() // Allow requests from any origin
               .AllowAnyHeader() // Allow any headers
               .AllowAnyMethod(); // Allow any HTTP methods (GET, POST, etc.)
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// I remove that some day
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable CORS and shut down restrictions
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
//I want to access configuration of app anywhere
GlobalSettings.Configuration = app.Configuration;
app.Run();