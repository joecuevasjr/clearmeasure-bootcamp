## ADDED Requirements

### Requirement: Assigned work orders can be moved back to unassigned
The system SHALL support moving a work order in Assigned status back to Draft using a state command named AssignedToDraftCommand with the user-visible verb Unassign.

#### Scenario: Creator unassigns an assigned work order
- **GIVEN** a work order in Assigned status
- **AND** the executing user is the work order creator
- **WHEN** the Unassign command is executed
- **THEN** the work order status becomes Draft
- **AND** the assignee is cleared
- **AND** the assigned date is cleared

#### Scenario: Non-creator cannot unassign
- **GIVEN** a work order in Assigned status
- **AND** the executing user is not the work order creator
- **WHEN** the user attempts to execute AssignedToDraftCommand
- **THEN** the command is invalid and SHALL NOT execute

### Requirement: Unassign transition is exposed through command surfaces
The system SHALL expose the unassign transition through all supported command-discovery surfaces.

#### Scenario: UI command list includes unassign when valid
- **GIVEN** a creator viewing a work order in Assigned status
- **WHEN** valid state commands are requested
- **THEN** the Unassign command is included in the available command list

#### Scenario: MCP command execution supports unassign
- **GIVEN** an assigned work order and a valid creator username
- **WHEN** execute-work-order-command is called with AssignedToDraftCommand
- **THEN** the command executes successfully and returns work order details in Draft status

### Requirement: Detached state command updates persist assignee removal
The system SHALL persist assignee removal when a state command executes against a detached work order instance.

#### Scenario: Detached command clears assignee relationship
- **GIVEN** a detached work order with an assignee
- **WHEN** a state command execution clears Assignee
- **THEN** the persisted AssigneeId foreign key is set to null
