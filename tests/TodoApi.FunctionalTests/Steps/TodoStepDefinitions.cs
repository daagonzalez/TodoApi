using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using NSubstitute;
using RandomTestValues;
using TechTalk.SpecFlow;
using TodoApi.Domain.Interfaces.Infrastructure;
using TodoApi.Domain.Models;
using TodoApi.Presentation.Controllers;

namespace TodoApi.FunctionalTests.Steps
{
    [Binding]
    public sealed class TodoStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly ITodoRepository _todoRepository;
        private readonly TodoController _todoController;

        public TodoStepDefinitions(ScenarioContext scenarioContext, ITodoRepository todoRepository, TodoController todoController)
       {
           _scenarioContext = scenarioContext;
            _todoRepository = todoRepository;
            _todoController = todoController;
        }

       [Given("there exist multiple Todo")]
       public void GivenThereExistMultipleTodo()
       {
           var multipleTodo = RandomValue.List<Todo>();

           _todoRepository.Read().Returns(multipleTodo);

           _scenarioContext.Set(multipleTodo, ContextKeys.MultipeTodo);
       }
        
       [When("I get all Todos")]
       public void WhenIGetAllTodos()
       {
           var httpResponse = _todoController.GetAll();

           _scenarioContext.Set(httpResponse, ContextKeys.Response);
       }

       [Then("the response status code should be (.*)")]
       public void ThenTheResponseStatusCodeShouldBe(int statusCode)
       {
           var response = _scenarioContext.Get<IStatusCodeActionResult>(ContextKeys.Response);

            response.StatusCode.Should().Be(statusCode);
       }
    }
}
