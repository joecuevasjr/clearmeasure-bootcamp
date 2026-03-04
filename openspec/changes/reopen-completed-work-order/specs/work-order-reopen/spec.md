## ADDED Requirements

### Requirement: Completed work orders can be reopened by the assignee
The system SHALL provide a `CompleteToAssignedCommand` state command with transition verb `Reopen` that moves a work order from `Complete` to `Assigned`.

#### Scenario: Reopen is valid for assignee on complete work order
- **GIVEN** a work order in `Complete` status
- **AND** the current user is the work order assignee
- **WHEN** command validity is evaluated
- **THEN** `CompleteToAssignedCommand` is valid

#### Scenario: Reopen is invalid for non-assignee or non-complete status
- **GIVEN** a work order not in `Complete` status or a user who is not the assignee
- **WHEN** command validity is evaluated
- **THEN** `CompleteToAssignedCommand` is invalid

### Requirement: Reopening clears completion timestamp
When a completed work order is reopened, `CompletedDate` SHALL be cleared.

#### Scenario: Completed date reset on reopen execution
- **GIVEN** a work order in `Complete` status with a non-null `CompletedDate`
- **WHEN** `CompleteToAssignedCommand` is executed
- **THEN** the work order status becomes `Assigned`
- **AND** `CompletedDate` becomes `null`

### Requirement: Work order management UI exposes Reopen command
The Work Order Manage page SHALL show a `Reopen` command action when viewing a completed work order as the assignee.

#### Scenario: Reopen action available after completion
- **GIVEN** the assignee opens a work order in `Complete` status
- **WHEN** available state commands are rendered
- **THEN** a command button with verb `Reopen` is shown

### Requirement: MCP command execution supports reopen transition
The MCP `execute-work-order-command` tool SHALL accept `CompleteToAssignedCommand` and execute the same domain transition semantics as UI/API flows.

#### Scenario: MCP reopens a completed work order
- **GIVEN** a completed work order with an assignee
- **WHEN** `execute-work-order-command` is called with `commandName = "CompleteToAssignedCommand"` and `executingUsername` set to the assignee
- **THEN** the work order transitions to `Assigned`
- **AND** `CompletedDate` is `null`

#### Scenario: MCP reference transition map includes reopen path
- **WHEN** `churchbulletin://reference/status-transitions` is requested
- **THEN** the transition map for `Complete` includes command `CompleteToAssignedCommand` targeting `Assigned`
