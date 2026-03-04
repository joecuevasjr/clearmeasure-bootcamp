using ClearMeasure.Bootcamp.AcceptanceTests.Extensions;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Model.StateCommands;
using ClearMeasure.Bootcamp.Core.Queries;
using ClearMeasure.Bootcamp.IntegrationTests;
using ClearMeasure.Bootcamp.UI.Shared.Pages;

namespace ClearMeasure.Bootcamp.AcceptanceTests.WorkOrders;

public class WorkOrderCompleteTests : AcceptanceTestBase
{
    [Test, Retry(2)]
    public async Task ShouldCompleteWorkOrder()
    {
        await LoginAsCurrentUser();

        var order = await CreateAndSaveNewWorkOrder();
        order = await ClickWorkOrderNumberFromSearchPage(order);
        order = await AssignExistingWorkOrder(order, CurrentUser.UserName);
        order = await ClickWorkOrderNumberFromSearchPage(order);

        order = await BeginExistingWorkOrder(order);
        order = await ClickWorkOrderNumberFromSearchPage(order);

        var expectedTitle = "Title from automation";
        var expectedDescription = "Description";
        order.Title = expectedTitle;
        order.Description = expectedDescription;
        order = await CompleteExistingWorkOrder(order);
        order = await ClickWorkOrderNumberFromSearchPage(order);

        await Expect(Page.GetByTestId(nameof(WorkOrderManage.Elements.Title))).ToHaveValueAsync(expectedTitle,
            new LocatorAssertionsToHaveValueOptions
            {
                Timeout = 10000 // 10 seconds
            });

        await Expect(Page.GetByTestId(nameof(WorkOrderManage.Elements.Description)))
            .ToHaveValueAsync(expectedDescription);
        await Expect(Page.GetByTestId(nameof(WorkOrderManage.Elements.Status)))
            .ToHaveTextAsync(WorkOrderStatus.Complete.FriendlyName);


        var displayedDateTime = await Page.GetDateTimeFromTestIdAsync(nameof(WorkOrderManage.Elements.CompletedDate));

        var rehyratedOrder = await Bus.Send(new WorkOrderByNumberQuery(order.Number!)) ??
                             throw new InvalidOperationException();
        rehyratedOrder.CompletedDate.TruncateToMinute().ShouldBe(displayedDateTime);
    }

    [Test, Retry(2)]
    public async Task ShouldReassignCompletedWorkOrder()
    {
        await LoginAsCurrentUser();

        var reassignedEmployee = Faker<Employee>();
        reassignedEmployee.UserName = $"reassign_{TestTag}_{reassignedEmployee.UserName}";
        using (var context = TestHost.NewDbContext())
        {
            context.Add(reassignedEmployee);
            context.SaveChanges();
        }

        var order = await CreateAndSaveNewWorkOrder();
        order = await ClickWorkOrderNumberFromSearchPage(order);

        order = await AssignExistingWorkOrder(order, CurrentUser.UserName);
        order = await ClickWorkOrderNumberFromSearchPage(order);

        order = await BeginExistingWorkOrder(order);
        order = await ClickWorkOrderNumberFromSearchPage(order);

        order = await CompleteExistingWorkOrder(order);
        order = await ClickWorkOrderNumberFromSearchPage(order);

        await Expect(Page.GetByTestId(nameof(WorkOrderManage.Elements.Assignee))).ToBeEnabledAsync();
        await Expect(Page.GetByTestId(nameof(WorkOrderManage.Elements.CommandButton) + CompleteToAssignedCommand.Name))
            .ToBeVisibleAsync();

        order = await ReassignCompletedWorkOrder(order, reassignedEmployee.UserName);
        order = await ClickWorkOrderNumberFromSearchPage(order);

        var rehydratedOrder = await Bus.Send(new WorkOrderByNumberQuery(order.Number!)) ??
                             throw new InvalidOperationException();
        rehydratedOrder.Status.ShouldBe(WorkOrderStatus.Assigned);
        rehydratedOrder.Assignee!.UserName.ShouldBe(reassignedEmployee.UserName);
        rehydratedOrder.CompletedDate.ShouldBeNull();
    }
}
