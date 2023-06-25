using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
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

        [Given(@"I have a valid Todo payload")]
        public void GivenIhaveavalidTodopayload()
        {
            var description = RandomValue.String();

            _scenarioContext.Set(description, ContextKeys.CreatePayload);
        }

        [Given(@"there are no Todos with my Id")]
        public void GiventherearenoTodoswithmyId()
        {
            var todoId = _scenarioContext.Get<Guid>(ContextKeys.TodoId);

            _todoRepository.Read(todoId).Throws(new Exception($"Unable to find a record with Id {todoId}"));
        }

        [Given(@"there exists a Todo with my Id")]
        public void GiventhereexistsaTodowithmyId()
        {
            var todoId = _scenarioContext.Get<Guid>(ContextKeys.TodoId);

            var expectedTodo = Todo.CreateForTesting(todoId, RandomValue.String(), RandomValue.Bool());

            _todoRepository.Read(todoId).Returns(expectedTodo);
            _scenarioContext.Set(expectedTodo, ContextKeys.ExpectedTodo);
        }

        [Given(@"I have a Todo Id")]
        public void GivenIhaveaTodoId()
        {
            var todoId = RandomValue.Guid();

            _scenarioContext.Set(todoId, ContextKeys.TodoId);
        }

        [Given("there are no Todos")]
        public void GivenThereAreNoTodos()
        {
            _todoRepository.Read().Returns(new List<Todo>());
        }

        [Given("there exist multiple Todo")]
        public void GivenThereExistMultipleTodo()
        {
            var multipleTodo = new List<Todo>(RandomValue.Int(10));
            for (var i = 0; i < multipleTodo.Capacity; i++)
            {
                var todoToAdd = Todo.CreateForTesting(RandomValue.Guid(), RandomValue.String(), RandomValue.Bool());
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

        [When(@"I get a Todo by my Id")]
        public void WhenIgetaTodobymyId()
        {
            var todoId = _scenarioContext.Get<Guid>(ContextKeys.TodoId);
            var httpResponse = _todoController.GetById(todoId);

            _scenarioContext.Set(httpResponse, ContextKeys.Response);
        }

        [When(@"I create a Todo with my payload")]
        public void WhenIcreateaTodowithmypayload()
        {
            var payload = _scenarioContext.Get<string>(ContextKeys.CreatePayload);
            var httpResponse = _todoController.Create(payload);

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

        [Then("the response should be empty")]
        public void ThenTheResponseShouldBeEmpty()
        {
            var response = _scenarioContext.Get<OkObjectResult>(ContextKeys.Response);
            var actualResult = response.Value as List<TodoResponse>;

            actualResult.Count.Should().Be(0);
        }

        [Then(@"the response should contain the Todo with my Id")]
        public void ThentheresponseshouldcontaintheTodowithmyId()
        {
            var expectedTodo = _scenarioContext.Get<Todo>(ContextKeys.ExpectedTodo);
            var response = _scenarioContext.Get<OkObjectResult>(ContextKeys.Response);
            var actualResult = response.Value as TodoResponse;

            actualResult.TodoId.Should().Be(expectedTodo.Id);
            actualResult.Description.Should().Be(expectedTodo.Description);
            actualResult.Completed.Should().Be(expectedTodo.Completed);
        }

        [Then(@"the response should contain the created Todo")]
        public void ThentheresponseshouldcontainthecreatedTodo()
        {
            var payload = _scenarioContext.Get<string>(ContextKeys.CreatePayload);
            var response = _scenarioContext.Get<CreatedAtActionResult>(ContextKeys.Response);

            var createdId = response.RouteValues.GetValueOrDefault("id") as Guid?;
            var actualResult = response.Value as TodoResponse;

            actualResult.TodoId.Should().Be(createdId.Value);
            actualResult.Description.Should().Be(payload);
            actualResult.Completed.Should().BeFalse();
        }

    }
}
