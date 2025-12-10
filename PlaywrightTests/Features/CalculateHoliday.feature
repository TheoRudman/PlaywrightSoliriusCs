Feature: Calculate Holiday

  Scenario: Change Irregular Hours selection
    Given I am on the Calculate Holiday page
    And I select Start Now
    And I set Irregular Hours to true
    When I click change on row 0
    Then I set Irregular Hours to false
