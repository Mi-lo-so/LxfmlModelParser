using ModelParserApp.Modules;
using ModelParserWebApp.Modules;

var builder = WebApplication.CreateBuilder(args);


// Add services - dependency injection defined in /Modules for readability
builder.Services.AddAwsServices();
builder.Services.AddDynamoDbServices(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

// Call to ensure table existing 
app.InitializeDynamoDb();

app.Run();