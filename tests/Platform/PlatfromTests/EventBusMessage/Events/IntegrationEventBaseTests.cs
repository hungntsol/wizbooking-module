using EventBusMessage.Events.Base;

namespace PlatformTests.EventBusMessage.Events;

[Collection("EventBusMessage Test")]
public class IntegrationEventBaseTests
{
	[Fact]
	[Trait("Category", "EventBusMessage")]
	public void ConstructNewMessage_ShouldHaveBaseProp()
	{
		// arrange

		// act
		var newMessage = new TestEvent();

		// assert
		Assert.NotNull(newMessage);
		Assert.NotEqual(Guid.Empty, newMessage.Id);
		Assert.True(DateTime.UtcNow > newMessage.CreatedAt);
	}

	public class TestEvent : IntegrationEventBase
	{
	}
}
