// this file is weird because I forgot to enable some option on project init heheh
using BCMS_Backend.Repository;
using BCMS_Backend.Settings;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
// rate limiting
// https://learn.microsoft.com/en-us/aspnet/core/performance/rate-limit?view=aspnetcore-9.0
// https://medium.com/@nirajranasinghe/secure-your-asp-net-core-app-with-rate-limiting-middleware-for-efficient-resource-allocation-5032e9d07531
/*
 PermitLimit to 4 and the time Window to 12. A maximum of 4 requests per each 12-second window are allowed.
QueueProcessingOrder to OldestFirst.
QueueLimit to 2.
 */
builder.Services.AddRateLimiter(_ => _
    .AddFixedWindowLimiter(policyName: "fixed", options =>
    {
        options.PermitLimit = 4;
        options.Window = TimeSpan.FromSeconds(12);
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 2;
    }));

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
// also for rate limiting
app.UseRateLimiter();

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