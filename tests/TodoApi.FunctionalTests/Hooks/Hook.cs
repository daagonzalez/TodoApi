using System;
using System.Collections.Generic;
using Castle.Core.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using TechTalk.SpecFlow;
using TodoApi.Application.Handlers;
using TodoApi.Application.Interfaces;
using TodoApi.Domain.Interfaces.Infrastructure;
using TodoApi.Domain.Models;
using TodoApi.Presentation.Controllers;

namespace TodoApi.FunctionalTests.Hooks;

[Binding]
public class Hook
{
    private static ServiceProvider _serviceProvider;
    private readonly ScenarioContext _scenarioContext;

    public Hook(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [BeforeTestRun]
    public static void SetupDependencies()
    {
        var services = new ServiceCollection();

        // App Dependencies
        services.AddSingleton<ITodoCommandHandler, TodoCommandHandler>();
        services.AddSingleton<ITodoQueryHandler, TodoQueryHandler>();

        // Infrastructure Dependencies
        var mockTodoRespository = Substitute.For<ITodoRepository>();
        services.AddSingleton(mockTodoRespository);

        // Controllers
        services.AddSingleton(provider => {
           return new TodoController(Substitute.For<ILogger<TodoController>>(), provider.GetService<ITodoCommandHandler>(), provider.GetService<ITodoQueryHandler>());
        });

        _serviceProvider = services.BuildServiceProvider();
    }

    [BeforeScenario]
    public void SetScenarioServiceProvider()
    {
        Console.WriteLine(_serviceProvider.GetService<ITodoRepository>());
        _scenarioContext.Set(_serviceProvider);
    }

    [AfterScenario]
    public void DisposeServices()
    {
        // Dispose the service provider after each scenario
        _serviceProvider.Dispose();
    }
}