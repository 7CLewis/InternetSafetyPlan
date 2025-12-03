using InternetSafetyPlan.Domain.GoalAggregate;

namespace InternetSafetyPlan.Domain.Test.GoalAggregate;

public class GoalTests
{
    private readonly Guid GoalId = Guid.NewGuid();
    private readonly Guid UltimateGoalId = Guid.NewGuid();

    private readonly GoalName Name = GoalName.Create("Research and purchase improved router.").Value;
    private readonly GoalCategory GoalCategory = GoalCategory.WiFi;
    private readonly Description Description = Description.Create("Research routers with extensive parental control options and purchase the one that best suits our family's needs.").Value;
    private readonly DueDate DueDate = DueDate.Create(DateTime.MaxValue).Value;

    [Fact]
    public void Create_Should_SetAllProperties()
    {
        // Arrange

        // Act
        var result = Goal.Create(GoalId, UltimateGoalId, Name, GoalCategory, Description, DueDate);
        var goal = result.Value;


        // Assert
        Assert.True(result.IsSuccess);
        SharedAssertions<Goal>.AllPropertiesNotNull(goal);
        Assert.Equal(goal.UltimateGoalId, UltimateGoalId);
        Assert.Equal(goal.Name, Name);
        Assert.Equal(goal.Description, Description);
        Assert.False(goal.IsComplete);
    }

    [Fact]
    public void UpdateInformation_Should_UpdateSpecifiedProperties()
    {
        // Arrange
        var originalGoalName = GoalName.Create("Research and purchase imporved rooter.").Value;
        var originalDescription = Description.Create("Readsorch routers with extensive parental control options and purchase the one that best suits our family's needs.").Value;

        var updatedGoalName = Name;
        var updatedDescription = Description;

        var createResult = Goal.Create(GoalId, UltimateGoalId, originalGoalName, GoalCategory, originalDescription);
        var goal = createResult.Value;

        // Act
        var updateResult = goal.Update(updatedGoalName, GoalCategory, updatedDescription);

        // Assert
        Assert.True(updateResult.IsSuccess);
        Assert.Equal(goal.UltimateGoalId, UltimateGoalId);
        Assert.Equal(goal.Name, updatedGoalName);
        Assert.Equal(goal.Description, updatedDescription);
        Assert.False(goal.IsComplete);
    }

    [Fact]
    public void UpdateInformation_Should_ReturnFailureResult_WhenMarkedAsComplete()
    {
        // Arrange
        var originalName = GoalName.Create("Research and purchase imporved rooter.").Value;
        var originalDescription = Description.Create("Readsorch routers with extensive parental control options and purchase the one that best suits our family's needs.").Value;

        var updatedName = Name;
        var updatedDescription = Description;

        var createResult = Goal.Create(GoalId, UltimateGoalId, originalName, GoalCategory, originalDescription);
        var goal = createResult.Value;
        goal.ToggleCompletion();

        // Act
        var updateResult = goal.Update(updatedName, GoalCategory, updatedDescription);

        // Assert
        Assert.True(goal.IsComplete);
        Assert.True(updateResult.IsFailure);
        // TODO: Check error type once those are defined in their own classes.
        Assert.Equal(goal.UltimateGoalId, UltimateGoalId);
        Assert.Equal(goal.Name, originalName);
        Assert.Equal(goal.Description, originalDescription);
    }

    [Fact]
    public void ToggleCompletion_Should_ToggleCompletionState()
    {
        // Arrange
        var result = Goal.Create(GoalId, UltimateGoalId, Name, GoalCategory, Description);
        var goal = result.Value;
        var originalCompletionStatus = goal.IsComplete;

        // Act
        goal.ToggleCompletion();

        // Assert
        Assert.False(originalCompletionStatus);
        Assert.True(goal.IsComplete);
    }

    [Fact]
    public void AddTag_Should_SuccessfullyAddTag_WhenTagIsNotInList()
    {
        // Arrange
        var tag = Tag.Create(Guid.NewGuid(), "Twitter", TagType.Application).Value;
        var goal = Goal.Create(GoalId, UltimateGoalId, Name, GoalCategory, Description, DueDate).Value;

        // Act
        var addResult = goal.AddTag(tag);

        // Assert
        Assert.True(addResult.IsSuccess);
        Assert.Single(goal.Tags);
        Assert.Equal(tag, goal.Tags.Single());
    }

    [Fact]
    public void AddTag_Should_ReturnFailure_WhenTagIsInList()
    {
        // Arrange
        var tag = Tag.Create(Guid.NewGuid(), "WiFi", TagType.Application).Value;
        var goal = Goal.Create(GoalId, UltimateGoalId, Name, GoalCategory, Description, DueDate).Value;

        // Act
        var add1 = goal.AddTag(tag);
        var add2 = goal.AddTag(tag);

        // Assert
        Assert.True(add1.IsSuccess);
        Assert.True(add2.IsFailure);
        Assert.Equal(tag, goal.Tags.Single());
    }

    [Fact]
    public void AddTag_Should_ReturnFailure_WhenTagListIsAtCapacity()
    {
        // Arrange
        var goal = Goal.Create(GoalId, UltimateGoalId, Name, GoalCategory, Description, DueDate).Value;

        // Act
        List<Result<Unit>> addResults = [];
        for (var i = 0; i < Goal.TagCapacity; i++)
        {
            var tag = Tag.Create(Guid.NewGuid(), i.ToString(), TagType.Application).Value;
            addResults.Add(goal.AddTag(tag));
        }

        var finalAdd = goal.AddTag(Tag.Create(Guid.NewGuid(), "Twitter", TagType.Application).Value);

        // Assert
        addResults.ForEach(result => Assert.True(result.IsSuccess));
        Assert.True(finalAdd.IsFailure);
        Assert.Equal(Goal.TagCapacity, goal.Tags.Count());
    }

    [Fact]
    public void RemoveTag_Should_SuccessfullyRemoveTag_WhenTagIsInList()
    {
        // Arrange
        var tag1 = Tag.Create(Guid.NewGuid(), "WiFi", TagType.Application).Value;
        var tag2 = Tag.Create(Guid.NewGuid(), "Parental Controls", TagType.Application).Value;

        var goal = Goal.Create(GoalId, UltimateGoalId, Name, GoalCategory, Description, DueDate).Value;

        goal.AddTag(tag1);
        goal.AddTag(tag2);

        // Act
        var remove = goal.RemoveTag(tag1);

        // Assert
        Assert.True(remove.IsSuccess);
        Assert.Single(goal.Tags);
        Assert.Equal(tag2, goal.Tags.Single());
    }

    [Fact]
    public void RemoveTag_Should_ReturnFailure_WhenTagIsNotInList()
    {
        // Arrange
        var tag1 = Tag.Create(Guid.NewGuid(), "WiFi", TagType.Application).Value;
        var tag2 = Tag.Create(Guid.NewGuid(), "Parental Controls", TagType.Application).Value;
        var goal = Goal.Create(GoalId, UltimateGoalId, Name, GoalCategory, Description, DueDate).Value;
        goal.AddTag(tag1);

        // Act
        var remove = goal.RemoveTag(tag2);

        // Assert
        Assert.True(remove.IsFailure);
        Assert.Single(goal.Tags);
        Assert.Equal(tag1, goal.Tags.Single());
    }

    [Fact]
    public void AddActionItem_Should_SuccessfullyAddActionItem_WhenActionItemIsNotInList()
    {
        // Arrange
        var goal = Goal.Create(GoalId, UltimateGoalId, Name, GoalCategory, Description, DueDate).Value;
        var actionItem = ActionItem.Create(Guid.NewGuid(), GoalId, ActionItemName.Create("Read PYE article").Value).Value;

        // Act
        var addResult = goal.AddActionItem(actionItem);

        // Assert
        Assert.True(addResult.IsSuccess);
        Assert.Single(goal.ActionItems);
        Assert.Equal(actionItem, goal.ActionItems.Single());
    }

    [Fact]
    public void AddActionItem_Should_ReturnFailure_WhenActionItemIsInList()
    {
        // Arrange
        var goal = Goal.Create(GoalId, UltimateGoalId, Name, GoalCategory, Description, DueDate).Value;
        var actionItem = ActionItem.Create(Guid.NewGuid(), GoalId, ActionItemName.Create("Read PYE article").Value).Value;

        // Act
        var add1 = goal.AddActionItem(actionItem);
        var add2 = goal.AddActionItem(actionItem);

        // Assert
        Assert.True(add1.IsSuccess);
        Assert.True(add2.IsFailure);
        Assert.Equal(actionItem, goal.ActionItems.Single());
    }

    [Fact]
    public void AddActionItem_Should_ReturnFailure_WhenActionItemListIsAtCapacity()
    {
        // Arrange
        var goal = Goal.Create(GoalId, UltimateGoalId, Name, GoalCategory, Description, DueDate).Value;
        var goalId = goal.Id;

        // Act
        List<Result<Unit>> addResults = [];
        for (var i = 0; i < Goal.ActionItemCapacity; i++)
        {
            var actionItem = ActionItem.Create(Guid.NewGuid(), goalId, ActionItemName.Create(i.ToString()).Value).Value;
            addResults.Add(goal.AddActionItem(actionItem));
        }

        var finalAdd = goal.AddActionItem(ActionItem.Create(Guid.NewGuid(), goalId, ActionItemName.Create("Read PYE article").Value).Value);

        // Assert
        addResults.ForEach(result => Assert.True(result.IsSuccess));
        Assert.True(finalAdd.IsFailure);
        Assert.Equal(Goal.ActionItemCapacity, goal.ActionItems.Count());
    }

    [Fact]
    public void DeleteActionItem_Should_SuccessfullyDeleteActionItem_WhenActionItemIsInList()
    {
        // Arrange
        var goal = Goal.Create(GoalId, UltimateGoalId, Name, GoalCategory, Description, DueDate).Value;
        var actionItem1 = ActionItem.Create(Guid.NewGuid(), GoalId, ActionItemName.Create("Read PYE article").Value).Value;
        var actionItem2 = ActionItem.Create(Guid.NewGuid(), goal.Id, ActionItemName.Create("Purchase router").Value).Value;

        goal.AddActionItem(actionItem1);
        goal.AddActionItem(actionItem2);

        // Act
        var remove = goal.DeleteActionItem(actionItem1);

        // Assert
        Assert.True(remove.IsSuccess);
    }

    [Fact]
    public void DeleteActionItem_Should_ReturnFailure_WhenActionItemIsNotInList()
    {
        // Arrange
        var goal = Goal.Create(GoalId, UltimateGoalId, Name, GoalCategory, Description, DueDate).Value;
        var actionItem1 = ActionItem.Create(Guid.NewGuid(), goal.Id, ActionItemName.Create("Read PYE article").Value).Value;
        var actionItem2 = ActionItem.Create(Guid.NewGuid(), goal.Id, ActionItemName.Create("Purchase router").Value).Value;

        goal.AddActionItem(actionItem1);

        // Act
        var remove = goal.DeleteActionItem(actionItem2);

        // Assert
        Assert.True(remove.IsFailure);
        Assert.Single(goal.ActionItems);
        Assert.Equal(actionItem1, goal.ActionItems.Single());
    }

    [Fact]
    public void AddAffectedDevice_Should_SuccessfullyAddDevice_WhenDeviceIsNotInList()
    {
        // Arrange
        var device = Device.Create(Guid.NewGuid(), Guid.NewGuid(), ManufacturerName.Create("iPhone 13").Value, Nickname.Create("Xi's iPhone").Value, DeviceType.Phone).Value;
        var goal = Goal.Create(GoalId, UltimateGoalId, Name, GoalCategory, Description, DueDate).Value;

        // Act
        var addResult = goal.AddAffectedDevice(device);

        // Assert
        Assert.True(addResult.IsSuccess);
        Assert.Single(goal.AffectedDevices);
        Assert.Equal(device, goal.AffectedDevices.Single());
    }

    [Fact]
    public void AddAffectedDevice_Should_ReturnFailure_WhenDeviceIsInList()
    {
        // Arrange
        var device = Device.Create(Guid.NewGuid(), Guid.NewGuid(), ManufacturerName.Create("iPhone 13").Value, Nickname.Create("Xi's iPhone").Value, DeviceType.Phone).Value;
        var goal = Goal.Create(GoalId, UltimateGoalId, Name, GoalCategory, Description, DueDate).Value;

        // Act
        var add1 = goal.AddAffectedDevice(device);
        var add2 = goal.AddAffectedDevice(device);

        // Assert
        Assert.True(add1.IsSuccess);
        Assert.True(add2.IsFailure);
        Assert.Equal(device, goal.AffectedDevices.Single());
    }

    [Fact]
    public void AddAffectedDevice_Should_ReturnFailure_WhenDeviceListIsAtCapacity()
    {
        // Arrange
        var goal = Goal.Create(GoalId, UltimateGoalId, Name, GoalCategory, Description, DueDate).Value;

        // Act
        List<Result<Unit>> addResults = [];
        for (var i = 0; i < Goal.DeviceCapacity; i++)
        {
            var device = Device.Create(Guid.NewGuid(), Guid.NewGuid(), ManufacturerName.Create(i.ToString()).Value, Nickname.Create("nickname").Value, DeviceType.Phone).Value;
            addResults.Add(goal.AddAffectedDevice(device));
        }

        var finalAdd = goal.AddAffectedDevice(Device.Create(Guid.NewGuid(), Guid.NewGuid(), ManufacturerName.Create("iPhone 13").Value, Nickname.Create("Xi's iPhone").Value, DeviceType.Phone).Value);

        // Assert
        addResults.ForEach(result => Assert.True(result.IsSuccess));
        Assert.True(finalAdd.IsFailure);
        Assert.Equal(Goal.DeviceCapacity, goal.AffectedDevices.Count());
    }

    [Fact]
    public void RemoveAffectedDevice_Should_SuccessfullyRemoveDevice_WhenDeviceIsInList()
    {
        // Arrange
        var device1 = Device.Create(Guid.NewGuid(), Guid.NewGuid(), ManufacturerName.Create("iPhone X").Value, Nickname.Create("Lei's iPhone").Value, DeviceType.Phone).Value;
        var device2 = Device.Create(Guid.NewGuid(), Guid.NewGuid(), ManufacturerName.Create("iPhone 13").Value, Nickname.Create("Xi's iPhone").Value, DeviceType.Phone).Value;

        var goal = Goal.Create(GoalId, UltimateGoalId, Name, GoalCategory, Description, DueDate).Value;

        goal.AddAffectedDevice(device1);
        goal.AddAffectedDevice(device2);

        // Act
        var remove = goal.RemoveAffectedDevice(device1);

        // Assert
        Assert.True(remove.IsSuccess);
        Assert.Single(goal.AffectedDevices);
        Assert.Equal(device2, goal.AffectedDevices.Single());
    }

    [Fact]
    public void RemoveAffectedDevice_Should_ReturnFailure_WhenDeviceIsNotInList()
    {
        // Arrange
        var device1 = Device.Create(Guid.NewGuid(), Guid.NewGuid(), ManufacturerName.Create("iPhone X").Value, Nickname.Create("Lei's iPhone").Value, DeviceType.Phone).Value;
        var device2 = Device.Create(Guid.NewGuid(), Guid.NewGuid(), ManufacturerName.Create("iPhone 13").Value, Nickname.Create("Xi's iPhone").Value, DeviceType.Phone).Value;

        var goal = Goal.Create(GoalId, UltimateGoalId, Name, GoalCategory, Description, DueDate).Value;
        goal.AddAffectedDevice(device1);

        // Act
        var remove = goal.RemoveAffectedDevice(device2);

        // Assert
        Assert.True(remove.IsFailure);
        Assert.Single(goal.AffectedDevices);
        Assert.Equal(device1, goal.AffectedDevices.Single());
    }

    [Fact]
    public void AddAffectedTeammate_Should_SuccessfullyAddTag_WhenTagIsNotInList()
    {
        // Arrange
        var teammate = Teammate.Create(Guid.NewGuid(), Guid.NewGuid(), TeammateName.Create("Lee").Value, DateOfBirth.Create(new DateTime(1989, 12, 13)).Value).Value;
        var goal = Goal.Create(GoalId, UltimateGoalId, Name, GoalCategory, Description, DueDate).Value;

        // Act
        var addResult = goal.AddAffectedTeammate(teammate);

        // Assert
        Assert.True(addResult.IsSuccess);
        Assert.Single(goal.AffectedTeammates);
        Assert.Equal(teammate, goal.AffectedTeammates.Single());
    }

    [Fact]
    public void AddAffectedTeammate_Should_ReturnFailure_WhenTagIsInList()
    {
        // Arrange
        var teammate = Teammate.Create(Guid.NewGuid(), Guid.NewGuid(), TeammateName.Create("Lee").Value, DateOfBirth.Create(new DateTime(1989, 12, 13)).Value).Value;
        var goal = Goal.Create(GoalId, UltimateGoalId, Name, GoalCategory, Description, DueDate).Value;

        // Act
        var add1 = goal.AddAffectedTeammate(teammate);
        var add2 = goal.AddAffectedTeammate(teammate);

        // Assert
        Assert.True(add1.IsSuccess);
        Assert.True(add2.IsFailure);
        Assert.Equal(teammate, goal.AffectedTeammates.Single());
    }

    [Fact]
    public void AddAffectedTeammate_Should_ReturnFailure_WhenTagListIsAtCapacity()
    {
        // Arrange
        var goal = Goal.Create(GoalId, UltimateGoalId, Name, GoalCategory, Description, DueDate).Value;

        // Act
        List<Result<Unit>> addResults = [];
        for (var i = 0; i < Goal.TeammateCapacity; i++)
        {
            var teammate = Teammate.Create(Guid.NewGuid(), Guid.NewGuid(), TeammateName.Create("Lee").Value, DateOfBirth.Create(new DateTime(1989, 12, 13)).Value).Value;
            addResults.Add(goal.AddAffectedTeammate(teammate));
        }

        var finalAdd = goal.AddAffectedTeammate(Teammate.Create(Guid.NewGuid(), Guid.NewGuid(), TeammateName.Create("Lee").Value, DateOfBirth.Create(new DateTime(1989, 12, 13)).Value).Value);

        // Assert
        addResults.ForEach(result => Assert.True(result.IsSuccess));
        Assert.True(finalAdd.IsFailure);
        Assert.Equal(Goal.TeammateCapacity, goal.AffectedTeammates.Count());
    }

    [Fact]
    public void RemoveAffectedTeammate_Should_SuccessfullyRemoveTag_WhenTagIsInList()
    {
        // Arrange
        var teammate1 = Teammate.Create(Guid.NewGuid(), Guid.NewGuid(), TeammateName.Create("Lee").Value, DateOfBirth.Create(new DateTime(1989, 12, 13)).Value).Value;
        var teammate2 = Teammate.Create(Guid.NewGuid(), Guid.NewGuid(), TeammateName.Create("Dee").Value, DateOfBirth.Create(new DateTime(1989, 12, 13)).Value).Value;

        var goal = Goal.Create(GoalId, UltimateGoalId, Name, GoalCategory, Description, DueDate).Value;

        goal.AddAffectedTeammate(teammate1);
        goal.AddAffectedTeammate(teammate2);

        // Act
        var remove = goal.RemoveAffectedTeammate(teammate1);

        // Assert
        Assert.True(remove.IsSuccess);
        Assert.Single(goal.AffectedTeammates);
        Assert.Equal(teammate2, goal.AffectedTeammates.Single());
    }

    [Fact]
    public void RemoveAffectedTeammate_Should_ReturnFailure_WhenTagIsNotInList()
    {
        // Arrange
        var teammate1 = Teammate.Create(Guid.NewGuid(), Guid.NewGuid(), TeammateName.Create("Lee").Value, DateOfBirth.Create(new DateTime(1989, 12, 13)).Value).Value;
        var teammate2 = Teammate.Create(Guid.NewGuid(), Guid.NewGuid(), TeammateName.Create("Dee").Value, DateOfBirth.Create(new DateTime(1989, 12, 13)).Value).Value;

        var goal = Goal.Create(GoalId, UltimateGoalId, Name, GoalCategory, Description, DueDate).Value;
        goal.AddAffectedTeammate(teammate1);

        // Act
        var remove = goal.RemoveAffectedTeammate(teammate2);

        // Assert
        Assert.True(remove.IsFailure);
        Assert.Single(goal.AffectedTeammates);
        Assert.Equal(teammate1, goal.AffectedTeammates.Single());
    }
}
