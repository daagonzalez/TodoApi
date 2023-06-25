Feature: Todo

Scenario: Get all Todos
	Given there exist multiple Todo
	When I get all Todos
	Then the response status code should be 200
		And the response should contain all the Todos

Scenario: Get all Todos when there are none
	Given there are no Todos
	When I get all Todos
	Then the response status code should be 200
		And the response should be empty

Scenario: Get a specific Todo by ID
    Given I have a Todo Id
    	And there exists a Todo with my Id
    When I get a Todo by my Id
    Then the response status code should be 200
    	And the response should contain the Todo with my Id

Scenario: Get a non-existing Todo by ID
    Given I have a Todo Id
    	And there are no Todos with my Id
    When I get a Todo by my Id
    Then the response status code should be 404

Scenario: Create a new Todo
    Given I have a valid Todo payload
    When I create a Todo with my payload
    Then the response status code should be 201
    	And the response should contain the created Todo