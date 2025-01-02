// this file is weird because I forgot to enable some option on project init heheh
using BCMS_Backend.Repository;
using BCMS_Backend.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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
GlobalSettings.Configuration = app.Configuration;
InMemoryDatabase.startupInMemoryDatabase();
app.Run();
// ahahah this fragment is never reached. Deal with your memory leak. Actually, who cares, app is closing anyway. Garbage collector will collect resources. 
InMemoryDatabase.finalizeInMemoryDatabase();