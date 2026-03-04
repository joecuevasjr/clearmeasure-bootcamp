using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Model.StateCommands;
using ClearMeasure.Bootcamp.DataAccess.Handlers;
using ClearMeasure.Bootcamp.UnitTests.Core.Queries;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace ClearMeasure.Bootcamp.IntegrationTests.DataAccess.Handlers;

public class StateCommandHandlerForUnassignTests : IntegratedTestBase
{
    [Test]
    public async Task ShouldSaveWorkOrderWithNoAssigneeAfterUnassign()
    {
        new DatabaseTests().Clean();

        var o = Faker<WorkOrder>();
        o.Id = Guid.Empty;
        o.Status = WorkOrderStatus.Assigned;
        var currentUser = Faker<Employee>();
        var assignee = Faker<Employee>();
        o.Creator = currentUser;
        o.Assignee = assignee;
        o.AssignedDate = TestHost.TestTime.DateTime;
        await using (var context = TestHost.GetRequiredService<DbContext>())
        {
            context.Add(currentUser);
            context.Add(assignee);
            await context.SaveChangesAsync();
        }

        var command = RemotableRequestTests.SimulateRemoteObject(new AssignedToDraftCommand(o, currentUser));

        var handler = TestHost.GetRequiredService<StateCommandHandler>();
        var result = await handler.Handle(command);

        var context3 = TestHost.GetRequiredService<DbContext>();
        var order = context3.Find<WorkOrder>(result.WorkOrder.Id) ?? throw new InvalidOperationException();
        order.Creator.ShouldBe(currentUser);
        order.Assignee.ShouldBeNull();
        order.AssignedDate.ShouldBeNull();
        order.Status.ShouldBe(WorkOrderStatus.Draft);
    }

    [Test]
    public async Task ShouldSaveWorkOrderWhenRemotingCommandOnly()
    {
        new DatabaseTests().Clean();

        var o = Faker<WorkOrder>();
        o.Id = Guid.Empty;
        o.Status = WorkOrderStatus.Assigned;
        var currentUser = Faker<Employee>();
        o.Creator = currentUser;
        await using (var context = TestHost.GetRequiredService<DbContext>())
        {
            context.Add(currentUser);
            context.Add(o);
            await context.SaveChangesAsync();
        }

        var command = new AssignedToDraftCommand(o, currentUser);
        var remotedCommand = RemotableRequestTests.SimulateRemoteObject(command);

        var handler = TestHost.GetRequiredService<StateCommandHandler>();
        var result = await handler.Handle(remotedCommand);

        var context3 = TestHost.GetRequiredService<DbContext>();
        var order = context3.Find<WorkOrder>(result.WorkOrder.Id) ?? throw new InvalidOperationException();
        order.Creator.ShouldBe(currentUser);
        order.Status.ShouldBe(WorkOrderStatus.Draft);
    }

    [Test]
    public async Task ShouldSaveWorkOrderWhenRemotingWorkOrderOnly()
    {
        new DatabaseTests().Clean();

        var o = Faker<WorkOrder>();
        o.Id = Guid.Empty;
        o.Status = WorkOrderStatus.Assigned;
        var currentUser = Faker<Employee>();
        o.Creator = currentUser;
        await using (var context = TestHost.GetRequiredService<DbContext>())
        {
            context.Add(currentUser);
            context.Add(o);
            await context.SaveChangesAsync();
        }

        var remotedOrder = RemotableRequestTests.SimulateRemoteObject(o);
        var command = new AssignedToDraftCommand(remotedOrder, currentUser);

        var handler = TestHost.GetRequiredService<StateCommandHandler>();
        var result = await handler.Handle(command);

        var context3 = TestHost.GetRequiredService<DbContext>();
        var order = context3.Find<WorkOrder>(result.WorkOrder.Id) ?? throw new InvalidOperationException();
        order.Creator.ShouldBe(currentUser);
        order.Status.ShouldBe(WorkOrderStatus.Draft);
    }
}
