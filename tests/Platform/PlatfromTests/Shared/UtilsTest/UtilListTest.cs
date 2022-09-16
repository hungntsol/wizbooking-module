using SharedCommon.Utilities;

namespace PlatformTests.Shared.UtilsTest;

[Collection("Utils.List Test")]
public class UtilListTest
{
	[Fact]
	[Trait("Category", "Utils.List")]
	public void EmptyList_ShouldReturnEmpty()
	{
		// arrange

		// act
		var emptyList = Utils.List.Empty<string>();

		// assert
		Assert.NotNull(emptyList);
		Assert.Empty(emptyList);
	}
}
