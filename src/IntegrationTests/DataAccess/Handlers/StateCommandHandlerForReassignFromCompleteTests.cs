using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Model.StateCommands;
using ClearMeasure.Bootcamp.DataAccess.Handlers;
using ClearMeasure.Bootcamp.UnitTests.Core.Queries;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace ClearMeasure.Bootcamp.IntegrationTests.DataAccess.Handlers;

public class StateCommandHandlerForReassignFromCompleteTests : IntegratedTestBase
{
    [Test]
    public async Task ShouldReassignCompletedWorkOrder()
    {
        new DatabaseTests().Clean();

        var creator = Faker<Employee>();
        var originalAssignee = Faker<Employee>();
        var newAssignee = Faker<Employee>();
        var order = Faker<WorkOrder>();
        order.Id = Guid.Empty;
        order.Creator = creator;
        order.Assignee = originalAssignee;
        order.Status = WorkOrderStatus.Complete;
        order.CompletedDate = TestHost.TestTime.DateTime.AddHours(-2);

        await using (var context = TestHost.GetRequiredService<DbContext>())
        {
            context.Add(creator);
            context.Add(originalAssignee);
            context.Add(newAssignee);
            context.Add(order);
            await context.SaveChangesAsync();
        }

        order.Assignee = newAssignee;
        var command = new CompleteToAssignedCommand(order, creator);
        var remotedCommand = RemotableRequestTests.SimulateRemoteObject(command);

        var handler = TestHost.GetRequiredService<StateCommandHandler>();
        var result = await handler.Handle(remotedCommand);

        var context3 = TestHost.GetRequiredService<DbContext>();
        var rehydrated = context3.Find<WorkOrder>(result.WorkOrder.Id) ?? throw new InvalidOperationException();
        rehydrated.Status.ShouldBe(WorkOrderStatus.Assigned);
        rehydrated.Assignee.ShouldBe(newAssignee);
        rehydrated.AssignedDate.ShouldBe(TestHost.TestTime.DateTime);
        rehydrated.CompletedDate.ShouldBeNull();
    }
}
