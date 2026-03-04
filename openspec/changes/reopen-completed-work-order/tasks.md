## 1. Domain State Command

- [x] 1.1 Add `CompleteToAssignedCommand` under `src/Core/Model/StateCommands/`
- [x] 1.2 Enforce validity rules: begin status `Complete`, actor is current assignee
- [x] 1.3 Clear `CompletedDate` during reopen execution
- [x] 1.4 Register the new command in `StateCommandList`

## 2. MCP Surface Updates

- [x] 2.1 Add `CompleteToAssignedCommand` to `execute-work-order-command` mapping in `WorkOrderTools`
- [x] 2.2 Update MCP command descriptions and unknown-command help text
- [x] 2.3 Add `CompleteToAssignedCommand` transition in `ReferenceResources.GetStatusTransitions`

## 3. Tests

- [x] 3.1 Add unit tests for `CompleteToAssignedCommand` validity and execution
- [x] 3.2 Update `StateCommandListTests` command count/order assertions
- [x] 3.3 Add integration test for `StateCommandHandler` reopen flow
- [x] 3.4 Add MCP integration tests for reopen command and transitions
- [x] 3.5 Update acceptance workflow tests to validate reopen behavior from completed state

## 4. Validation

- [x] 4.1 Run `./PrivateBuild.ps1` and ensure full pass before commit/PR
