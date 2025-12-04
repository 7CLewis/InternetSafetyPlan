namespace InternetSafetyPlan.Domain.Test.GoalAggregate;

public class ActionItemTests
{
    private readonly Guid ActionItemId = Guid.NewGuid();
    private readonly Guid GoalId = Guid.NewGuid();

    [Fact]
    public void Create_Should_SetAllProperties()
    {
        // Arrange
        var name = ActionItemName.Create("Install CovenantEyes on Layla's phone.");
        var description = Description.Create("Download the CovenantEyes app from the Google Play store, login, and configure the phone settings to trust the app.");
        var dueDate = DueDate.Create(new DateTime(2199, 12, 13));

        // Act
        var result = ActionItem.Create(ActionItemId, GoalId, name.Value, description.Value, dueDate.Value);
        var actionItem = result.Value;


        // Assert
        Assert.True(result.IsSuccess);
        SharedAssertions<ActionItem>.AllPropertiesNotNull(actionItem);
        Assert.Equal(actionItem.GoalId, GoalId);
        Assert.Equal(actionItem.Name, name.Value);
        Assert.Equal(actionItem.Description, description.Value);
        Assert.Equal(actionItem.DueDate, dueDate.Value);
        Assert.False(actionItem.IsComplete);
    }

    [Fact]
    public void UpdateInformation_Should_UpdateSpecifiedProperties()
    {
        // Arrange
        var originalActionItemName = ActionItemName.Create("Install CovenantEyes on Layla's phon.");
        var originalDescription = Description.Create("Get the CovenantEyes app from the Google Play store, login, and configure the phone settings to trust the app.");

        var updatedActionItemName = ActionItemName.Create("Install CovenantEyes on Layla's phone.");
        var updatedDescription = Description.Create("Download the CovenantEyes app from the Google Play store, login, and configure the phone settings to trust the app.");

        var createResult = ActionItem.Create(ActionItemId, GoalId, originalActionItemName.Value, originalDescription.Value);
        var actionItem = createResult.Value;

        // Act
        var updateResult = actionItem.Update(updatedActionItemName.Value, updatedDescription.Value);

        // Assert
        Assert.True(updateResult.IsSuccess);
        Assert.Equal(actionItem.GoalId, GoalId);
        Assert.Equal(actionItem.Name, updatedActionItemName.Value);
        Assert.Equal(actionItem.Description, updatedDescription.Value);
        Assert.False(actionItem.IsComplete);
    }

    [Fact]
    public void UpdateInformation_Should_ReturnFailureResult_WhenMarkedAsComplete()
    {
        // Arrange
        var originalName = ActionItemName.Create("Install CovenantEyes on Layla's phon.");
        var originalDescription = Description.Create("Get the CovenantEyes app from the Google Play store, login, and configure the phone settings to trust the app.");

        var updatedName = ActionItemName.Create("Install CovenantEyes on Layla's phone.");
        var updatedDescription = Description.Create("Download the CovenantEyes app from the Google Play store, login, and configure the phone settings to trust the app.");

        var createResult = ActionItem.Create(ActionItemId, GoalId, originalName.Value, originalDescription.Value);
        var actionItem = createResult.Value;
        actionItem.ToggleCompletion();

        // Act
        var updateResult = actionItem.Update(updatedName.Value, updatedDescription.Value);

        // Assert
        Assert.True(actionItem.IsComplete);
        Assert.True(updateResult.IsFailure);
        // TODO: Check error type once those are defined in their own classes.
        Assert.Equal(actionItem.GoalId, GoalId);
        Assert.Equal(actionItem.Name, originalName.Value);
        Assert.Equal(actionItem.Description, originalDescription.Value);
    }

    [Fact]
    public void ToggleCompletion_Should_ToggleCompletionState()
    {
        // Arrange
        var name = ActionItemName.Create("Install CovenantEyes on Layla's phone.");
        var description = Description.Create("Download the CovenantEyes app from the Google Play store, login, and configure the phone settings to trust the app.");

        var result = ActionItem.Create(ActionItemId, GoalId, name.Value, description.Value);
        var actionItem = result.Value;
        var originalCompletionStatus = actionItem.IsComplete;

        // Act
        actionItem.ToggleCompletion();

        // Assert
        Assert.False(originalCompletionStatus);
        Assert.True(actionItem.IsComplete);
    }
}
