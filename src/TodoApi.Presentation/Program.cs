using TodoApi.Application.Handlers;
using TodoApi.Application.Interfaces;
using TodoApi.Infrastructure.Interfaces;
using TodoApi.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => 
{
    c.SwaggerDoc(
        "v1",
        new Microsoft.OpenApi.Models.OpenApiInfo { Title = "ToDo API", Version = "v1" });
});

builder.Services.AddSingleton<ITodoCommandHandler, TodoCommandHandler>();
builder.Services.AddSingleton<ITodoQueryHandler, TodoQueryHandler>();

builder.Services.AddSingleton<IEventRepository, EventRepository>();
builder.Services.AddSingleton<ITodoRepository, TodoRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDo API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
