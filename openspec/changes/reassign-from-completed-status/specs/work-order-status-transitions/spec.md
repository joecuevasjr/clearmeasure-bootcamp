## ADDED Requirements

### Requirement: Reassign completed work order
The system SHALL allow a completed work order to be reassigned by transitioning it from `Complete` to `Assigned`.

#### Scenario: Creator reassigns completed work order
- **GIVEN** a work order is in `Complete` status
- **AND** the current user is the work order creator
- **WHEN** the user executes the completed reassignment command
- **THEN** the work order status SHALL become `Assigned`
- **AND** the assignee SHALL be updated to the selected user

### Requirement: Reassigning a completed order reopens lifecycle dates
When a completed work order is reassigned, completion metadata SHALL be cleared and assignment metadata SHALL be refreshed.

#### Scenario: Completion date is cleared after reassignment
- **GIVEN** a work order has `Status = Complete`
- **AND** `CompletedDate` is populated
- **WHEN** the work order is reassigned from complete status
- **THEN** `CompletedDate` SHALL be cleared
- **AND** `AssignedDate` SHALL be set to the reassignment time

### Requirement: Completed status exposes reassignment in UI
The work order manage UI SHALL allow assignee selection for completed work orders when reassignment is valid for the current user.

#### Scenario: Completed work order assignee selector is enabled
- **GIVEN** a creator opens a completed work order
- **WHEN** valid commands are rendered
- **THEN** the assignee selector SHALL be enabled
- **AND** a `Reassign` command button SHALL be available
