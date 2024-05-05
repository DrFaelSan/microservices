using DevShooping.ProductAPI.Extensions;
using DevShooping.ProductAPI.Ioc;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

//EF Contexts
builder.Services.AddContexts(builder.Configuration);

//AutoMapper
builder.Services.AddMappingCongifs();

//Ioc Repositorys
builder.Services.RegisterRepositorys();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dev Shopping :)" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
