## Why

Assigned work orders can currently move forward to InProgress or be cancelled, but there is no way to return an assigned work order to an unassigned state when assignment needs to be reversed. This blocks real-world correction workflows when an order is assigned to the wrong person or should return to the backlog.

## What Changes

- Add a new state command AssignedToDraftCommand with UI verb Unassign
- Support transition from Assigned back to Draft (unassigned state)
- Clear assignment metadata (Assignee and AssignedDate) when unassigning
- Register the command in StateCommandList so it appears in valid command buttons
- Expose the command in MCP command routing and reference transition metadata
- Ensure detached-entity state command handling persists null assignee relationships
- Add unit and integration test coverage for command behavior and persistence

## Impact

- **Domain behavior:** adds reversible assignment flow (Assigned -> Draft)
- **UI behavior:** creator can execute Unassign when work order is Assigned
- **MCP behavior:** execute-work-order-command accepts AssignedToDraftCommand
- **Data access:** explicit persistence of null assignee FK for detached command updates
- **Compatibility:** additive change; existing transitions remain intact
