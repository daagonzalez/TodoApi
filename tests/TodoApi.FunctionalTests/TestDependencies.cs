using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using SolidToken.SpecFlow.DependencyInjection;
using TodoApi.Application.Handlers;
using TodoApi.Application.Interfaces;
using TodoApi.Domain.Interfaces.Infrastructure;
using TodoApi.Presentation.Controllers;
using System;

namespace TodoApi.FunctionalTests;

public static class TestDependencies
{
    [ScenarioDependencies]
    public static IServiceCollection CreateServices()
    {
        var services = new ServiceCollection();

         // App Dependencies
        services.AddSingleton<ITodoCommandHandler, TodoCommandHandler>();
        services.AddSingleton<ITodoQueryHandler, TodoQueryHandler>();

        // Infrastructure Dependencies
        var mockTodoRespository = Substitute.For<ITodoRepository>();
        var mockEventRepository = Substitute.For<IEventRepository>();
        services.AddSingleton(mockTodoRespository);
        services.AddSingleton(mockEventRepository);

        // Controllers
        services.AddSingleton(provider => {
           return new TodoController(Substitute.For<ILogger<TodoController>>(), provider.GetService<ITodoCommandHandler>(), provider.GetService<ITodoQueryHandler>());
        });
        
        return services;
    }

}