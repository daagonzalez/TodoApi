Feature: Todo

Scenario: Get all Todos
	Given there exist multiple Todo
	When I get all Todos
	Then the response status code should be 200
