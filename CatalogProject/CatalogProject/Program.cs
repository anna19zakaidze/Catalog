using CatalogProject.Repositories;
using CatalogProject.Settings;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDBSettings)).Get<MongoDBSettings>();
builder.Services.AddSingleton<IMongoClient>(serviceProvider=>
{  
    return new MongoClient(mongoDbSettings.ConnectionString);
});

builder.Services.AddSingleton<IItemsRepository, MongoDBItemsRepository>();
builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks().AddMongoDb(
    mongoDbSettings.ConnectionString, 
    name:"mongodb", 
    timeout:TimeSpan.FromSeconds(3),
    tags: new[] {"ready"});

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
app.MapHealthChecks("/health/ready",new HealthCheckOptions
{
    Predicate =(check)=>check.Tags.Contains("ready")
});
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = (_) => false
});

app.Run();
