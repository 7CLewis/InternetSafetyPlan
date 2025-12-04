namespace InternetSafetyPlan.Domain.Test.DeviceAggregate;
public class DeviceTests
{
    private readonly Guid DeviceId = Guid.NewGuid();
    private readonly Guid TeamId = Guid.NewGuid();
    private readonly ManufacturerName Name = ManufacturerName.Create("iPhone 133").Value;
    private readonly Nickname Nickname = Nickname.Create("Jay's phone").Value;
    private readonly DeviceType DeviceType = DeviceType.Phone;

    [Fact]
    public void Create_Should_SetAllProperties()
    {
        // Arrange

        // Act
        var device = Device.Create(DeviceId, TeamId, Name, Nickname, DeviceType).Value;

        // Assert
        SharedAssertions<Device>.AllPropertiesNotNull(device);
        Assert.Equal(device.TeamId, TeamId);
        Assert.Equal(device.Type, DeviceType);
        Assert.Equal(device.Name, Name);
        Assert.Equal(device.Nickname, Nickname);
    }

    [Fact]
    public void UpdateInformation_Should_UpdateSpecifiedProperties()
    {
        // Arrange
        var originalDeviceType = DeviceType.Router;
        var originalName = ManufacturerName.Create("iPhone 133").Value;
        var originalNickname = Nickname.Create("Jay'd phon").Value;

        var updatedDeviceType = DeviceType;
        var updatedName = Name;
        var updatedNickname = Nickname;

        var device = Device.Create(DeviceId, TeamId, originalName, originalNickname, originalDeviceType).Value;

        // Act
        var updateResult = device.UpdateInformation(updatedName, updatedNickname, updatedDeviceType);

        // Assert
        Assert.True(updateResult.IsSuccess);
        Assert.Equal(device.Type, updatedDeviceType);
        Assert.Equal(device.Name, updatedName);
        Assert.Equal(device.Nickname, updatedNickname);
    }

    [Fact]
    public void AddTag_Should_SuccessfullyAddTag_WhenTagIsNotInList()
    {
        // Arrange
        var tag = Tag.Create(Guid.NewGuid(), "Twitter", TagType.Application).Value;
        var device = Device.Create(DeviceId, TeamId, Name, Nickname, DeviceType).Value;

        // Act
        var addResult = device.AddTag(tag);

        // Assert
        Assert.True(addResult.IsSuccess);
        Assert.Single(device.Tags);
        Assert.Equal(tag, device.Tags.Single());
    }

    [Fact]
    public void AddTag_Should_ReturnFailure_WhenTagIsInList()
    {
        // Arrange
        var tag = Tag.Create(Guid.NewGuid(), "Twitter", TagType.Application).Value;
        var device = Device.Create(DeviceId, TeamId, Name, Nickname, DeviceType).Value;

        // Act
        var add1 = device.AddTag(tag);
        var add2 = device.AddTag(tag);

        // Assert
        Assert.True(add1.IsSuccess);
        Assert.True(add2.IsFailure);
        Assert.Equal(tag, device.Tags.Single());
    }

    [Fact]
    public void AddTag_Should_ReturnFailure_WhenTagListIsAtCapacity()
    {
        // Arrange
        var device = Device.Create(DeviceId, TeamId, Name, Nickname, DeviceType).Value;

        // Act
        List<Result<Unit>> addResults = [];
        for (var i = 0; i < Device.TagCapacity; i++)
        {
            var tag = Tag.Create(Guid.NewGuid(), i.ToString(), TagType.Application).Value;
            addResults.Add(device.AddTag(tag));
        }

        var finalAdd = device.AddTag(Tag.Create(Guid.NewGuid(), "Twitter", TagType.Application).Value);

        // Assert
        addResults.ForEach(result => Assert.True(result.IsSuccess));
        Assert.True(finalAdd.IsFailure);
        Assert.Equal(Device.TagCapacity, device.Tags.Count());
    }

    [Fact]
    public void RemoveTag_Should_SuccessfullyRemoveTag_WhenTagIsInList()
    {
        // Arrange
        var tag1 = Tag.Create(Guid.NewGuid(), "Twitter", TagType.Application).Value;
        var tag2 = Tag.Create(Guid.NewGuid(), "Facebook", TagType.Application).Value;
        var device = Device.Create(DeviceId, TeamId, Name, Nickname, DeviceType).Value;
        device.AddTag(tag1);
        device.AddTag(tag2);

        // Act
        var remove = device.RemoveTag(tag1);

        // Assert
        Assert.True(remove.IsSuccess);
        Assert.Single(device.Tags);
        Assert.Equal(tag2, device.Tags.Single());
    }

    [Fact]
    public void RemoveTag_Should_ReturnFailure_WhenTagIsNotInList()
    {
        // Arrange
        var tag1 = Tag.Create(Guid.NewGuid(), "Twitter", TagType.Application).Value;
        var tag2 = Tag.Create(Guid.NewGuid(), "Facebook", TagType.Application).Value;
        var device = Device.Create(DeviceId, TeamId, Name, Nickname, DeviceType).Value;
        device.AddTag(tag1);

        // Act
        var remove = device.RemoveTag(tag2);

        // Assert
        Assert.True(remove.IsFailure);
        Assert.Single(device.Tags);
        Assert.Equal(tag1, device.Tags.Single());
    }

    [Fact]
    public void AddTeammate_Should_SuccessfullyAddTeammate_WhenTeammateIsNotInList()
    {
        // Arrange
        var teammate = Teammate.Create(Guid.NewGuid(), TeamId, TeammateName.Create("Tim Teammate").Value, DateOfBirth.Create(DateTime.UtcNow).Value).Value;
        var device = Device.Create(DeviceId, TeamId, Name, Nickname, DeviceType).Value;

        // Act
        var add = device.AddTeammate(teammate);

        // Assert
        Assert.True(add.IsSuccess);
        Assert.Single(device.Teammates);
        Assert.Equal(teammate, device.Teammates.Single());
    }

    [Fact]
    public void AddTeammate_Should_ReturnFailure_WhenTeammateIsInList()
    {
        // Arrange
        var teammate = Teammate.Create(Guid.NewGuid(), TeamId, TeammateName.Create("Tim Teammate").Value, DateOfBirth.Create(DateTime.UtcNow).Value).Value;
        var device = Device.Create(DeviceId, TeamId, Name, Nickname, DeviceType).Value;

        // Act
        var addResult1 = device.AddTeammate(teammate);
        var addResult2 = device.AddTeammate(teammate);

        // Assert
        Assert.True(addResult1.IsSuccess);
        Assert.True(addResult2.IsFailure);
        Assert.Equal(teammate, device.Teammates.Single());
    }

    [Fact]
    public void AddTeammate_Should_ReturnFailure_WhenTeammateListIsAtCapacity()
    {
        // Arrange
        var device = Device.Create(DeviceId, TeamId, Name, Nickname, DeviceType).Value;

        // Act
        List<Result<Unit>> addResults = [];
        for (var i = 0; i < Device.TeammateCapacity; i++)
        {
            var email = Email.Create($"teammate{i}@gmail.com").Value;
            var teammate = Teammate.Create(Guid.NewGuid(), TeamId, TeammateName.Create($"Teammate {i}").Value, DateOfBirth.Create(DateTime.UtcNow).Value).Value;
            addResults.Add(device.AddTeammate(teammate));
        }

        var finalTeammate = Teammate.Create(Guid.NewGuid(), TeamId, TeammateName.Create("Tim Teammate").Value, DateOfBirth.Create(DateTime.UtcNow).Value).Value;
        var finalAdd = device.AddTeammate(finalTeammate);

        // Assert
        addResults.ForEach(result => Assert.True(result.IsSuccess));
        Assert.True(finalAdd.IsFailure);
        Assert.Equal(Device.TeammateCapacity, device.Teammates.Count());
    }

    [Fact]
    public void RemoveTeammate_Should_SuccessfullyRemoveTeammate_WhenTeammateIsInList()
    {
        // Arrange
        var teammate1 = Teammate.Create(Guid.NewGuid(), TeamId, TeammateName.Create("Tim Teammate").Value, DateOfBirth.Create(DateTime.UtcNow).Value).Value;
        var teammate2 = Teammate.Create(Guid.NewGuid(), TeamId, TeammateName.Create("Tom Teammate").Value, DateOfBirth.Create(DateTime.UtcNow).Value).Value;

        var device = Device.Create(DeviceId, TeamId, Name, Nickname, DeviceType).Value;
        device.AddTeammate(teammate1);
        device.AddTeammate(teammate2);

        // Act
        var remove = device.RemoveTeammate(teammate1);

        // Assert
        Assert.True(remove.IsSuccess);
        Assert.Single(device.Teammates);
        Assert.Equal(teammate2, device.Teammates.Single());
    }

    [Fact]
    public void RemoveTeammate_Should_ReturnFailure_WhenTeammateIsNotInList()
    {
        // Arrange
        var teammate1 = Teammate.Create(Guid.NewGuid(), TeamId, TeammateName.Create("Tim Teammate").Value, DateOfBirth.Create(DateTime.UtcNow).Value).Value;
        var teammate2 = Teammate.Create(Guid.NewGuid(), TeamId, TeammateName.Create("Tom Teammate").Value, DateOfBirth.Create(DateTime.UtcNow).Value).Value;

        var device = Device.Create(DeviceId, TeamId, Name, Nickname, DeviceType).Value;

        device.AddTeammate(teammate1);

        // Act
        var remove = device.RemoveTeammate(teammate2);

        // Assert
        Assert.True(remove.IsFailure);
        Assert.Single(device.Teammates);
        Assert.Equal(teammate1, device.Teammates.Single());
    }
}
