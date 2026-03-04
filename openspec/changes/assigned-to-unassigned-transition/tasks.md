## 1. Domain Transition

- [x] 1.1 Add AssignedToDraftCommand implementing Assigned -> Draft
- [x] 1.2 Clear Assignee and AssignedDate during command execution
- [x] 1.3 Register the new command in StateCommandList

## 2. Detached Persistence Reliability

- [x] 2.1 Update state command handling to persist null assignee FK updates for detached entities

## 3. MCP Exposure

- [x] 3.1 Add AssignedToDraftCommand to MCP command routing and unknown-command help text
- [x] 3.2 Add AssignedToDraftCommand to MCP reference status transition metadata

## 4. Test Coverage

- [x] 4.1 Add unit tests for AssignedToDraftCommand validity and execution side effects
- [x] 4.2 Update unit tests for command list ordering and count
- [x] 4.3 Add integration tests for state command handler unassign behavior
- [x] 4.4 Add integration tests for MCP unassign command execution and status-transition metadata
