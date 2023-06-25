using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using NSubstitute;
using RandomTestValues;
using TechTalk.SpecFlow;
using TodoApi.Domain.Interfaces.Infrastructure;
using TodoApi.Domain.Models;
using TodoApi.Presentation.Controllers;
using TodoApi.Presentation.Models;

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
            var multipleTodo = new List<Todo>(RandomValue.Int(10));
            for (var i = 0; i < multipleTodo.Capacity; i++)
            {
                var todoToAdd = Todo.Create(RandomValue.Guid(), RandomValue.String());
                if (RandomValue.Bool())
                {
                    todoToAdd.Complete();
                }

                multipleTodo.Add(todoToAdd);
            }

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

        [Then("the response should contain all the Todos")]
        public void ThenTheResponseShouldContainAllTheTodos()
        {
            var expectedTodos = _scenarioContext.Get<List<Todo>>(ContextKeys.MultipeTodo);
            var response = _scenarioContext.Get<OkObjectResult>(ContextKeys.Response);
            var actualResult = response.Value as List<TodoResponse>;
            
            actualResult.Count.Should().Be(expectedTodos.Count);
            foreach (var todo in expectedTodos)
            {
                var item = actualResult.Find(x => x.TodoId == todo.Id);
                item.Should().NotBeNull();
                item.Description.Should().Be(todo.Description);
                item.Completed.Should().Be(todo.Completed);
            }
        }
    }
}
