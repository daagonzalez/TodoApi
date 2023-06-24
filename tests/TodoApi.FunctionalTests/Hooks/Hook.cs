using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using TechTalk.SpecFlow;

namespace TodoApi.FunctionalTests.Hooks;

[Binding]
public class Hooks
{
    private static ServiceProvider _serviceProvider;
    private readonly ScenarioContext _scenarioContext;

    public Hooks(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [BeforeTestRun]
    public static void SetupDependencies()
    {
        var services = new ServiceCollection();

        // var mockTodoRespository = Substitute.For<ITodoRepository>();
        // services.AddSingleton<ITodoRepository>(mockTodoRespository);

        _serviceProvider = services.BuildServiceProvider();
    }

    [BeforeScenario]
    public void SetScenarioServiceProvider()
    {
        _scenarioContext.Set(_serviceProvider);
    }

    [AfterScenario]
    public void DisposeServices()
    {
        // Dispose the service provider after each scenario
        _serviceProvider.Dispose();
    }
}