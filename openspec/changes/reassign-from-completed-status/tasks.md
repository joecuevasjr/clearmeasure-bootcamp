## 1. Domain Workflow

- [x] 1.1 Add `CompleteToAssignedCommand` to transition `Complete -> Assigned`
- [x] 1.2 Update state command registration so completed reassignment is discoverable
- [x] 1.3 Clear `CompletedDate` and refresh `AssignedDate` during completed reassignment

## 2. UI Behavior

- [x] 2.1 Allow assignee selection for completed work orders
- [x] 2.2 Ensure completed work orders show the `Reassign` command when valid for the current user

## 3. MCP Support

- [x] 3.1 Add `CompleteToAssignedCommand` handling in `execute-work-order-command`
- [x] 3.2 Update MCP reference transition metadata to include complete-to-assigned transition

## 4. Test Coverage

- [x] 4.1 Add unit tests for `CompleteToAssignedCommand`
- [x] 4.2 Add integration tests for complete-to-assigned state command handling
- [x] 4.3 Update acceptance tests to cover completed reassignment
- [x] 4.4 Update MCP integration tests for completed reassignment command support
