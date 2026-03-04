## Why

Completed work orders are currently locked from reassignment. When a completed order needs follow-up work, users cannot re-open it by assigning it to another person, forcing manual data workarounds and inconsistent status handling.

## What Changes

- Add a new state command to reassign a work order from `Complete` to `Assigned`
- Allow reassignment controls in the Work Order manage screen for completed work orders
- Clear `CompletedDate` and refresh `AssignedDate` when a completed work order is reassigned
- Update MCP state-command support to include completed reassignment
- Add unit, integration, and acceptance test coverage for this lifecycle path

## Impact

- **Domain:** Adds `CompleteToAssignedCommand` with creator authorization
- **UI:** Completed work orders can be reassigned in the existing assign control path
- **MCP:** `execute-work-order-command` now supports completed reassignment
- **Testing:** Expanded lifecycle coverage for complete-to-assigned transitions
