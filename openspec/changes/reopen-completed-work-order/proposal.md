## Why

Completed work orders are currently terminal for the assignee in both UI and MCP workflows. In practice, work can be marked complete too early or require follow-up after inspection. The system needs an explicit, auditable way to resume a completed work order without creating duplicates.

## What Changes

- Add a new state command `CompleteToAssignedCommand` with transition verb `Reopen`
- Allow transition `Complete -> Assigned` for the current assignee
- Clear `CompletedDate` when reopening
- Include the new command in `StateCommandList` so it appears in Work Order Manage actions
- Expose the command in MCP `execute-work-order-command`
- Update MCP status transition reference data to document the new path
- Add unit, integration, and acceptance coverage for reopen behavior

## Capabilities

### New Capabilities

- `work-order-reopen`: allows reopening a completed work order back to assigned state

### Modified Capabilities

- `mcp-work-order-tools`: adds `CompleteToAssignedCommand` support to `execute-work-order-command`
- `mcp-reference-resources`: updates status transition map with `CompleteToAssignedCommand`

## Impact

- **Domain:** one new `IStateCommand` implementation and updated command list ordering
- **UI:** completed work orders are no longer read-only for eligible assignees; `Reopen` action is shown
- **MCP:** command list and reference transitions now include reopen support
- **Data:** no schema changes; `CompletedDate` is set to `NULL` when reopened
