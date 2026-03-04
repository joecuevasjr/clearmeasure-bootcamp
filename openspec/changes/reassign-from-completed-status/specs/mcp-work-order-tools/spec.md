## ADDED Requirements

### Requirement: MCP tool supports completed reassignment command
The `execute-work-order-command` MCP tool SHALL support `CompleteToAssignedCommand` for reassigning completed work orders.

#### Scenario: Execute completed reassignment with assignee
- **GIVEN** a work order is in `Complete` status
- **AND** a valid creator username is provided as `executingUsername`
- **AND** a valid `assigneeUsername` is provided
- **WHEN** `execute-work-order-command` is called with `commandName = "CompleteToAssignedCommand"`
- **THEN** the command SHALL execute successfully
- **AND** the returned work order status SHALL be `Assigned`
- **AND** `CompletedDate` SHALL be null

#### Scenario: Missing assignee for completed reassignment
- **GIVEN** a work order is in `Complete` status
- **WHEN** `execute-work-order-command` is called with `commandName = "CompleteToAssignedCommand"` and no `assigneeUsername`
- **THEN** the tool SHALL return a validation message indicating `assigneeUsername` is required
