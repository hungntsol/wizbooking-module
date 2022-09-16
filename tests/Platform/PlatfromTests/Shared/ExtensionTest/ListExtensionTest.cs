using SharedCommon.Utilities;
using SharedCommon.Utilities.Extensions;

namespace PlatformTests.Shared.ExtensionTest;

[Collection("ListExtension Test")]
public class ListExtensionTest
{
	[Fact]
	[Trait("Category", "ListExtension")]
	public void CheckEmptyList_ShouldReturnTrue()
	{
		// arrange
		var list = Utils.List.Empty<string>();

		// act
		var result = list.IsNullOrEmpty();

		// assert
		Assert.NotNull(list);
		Assert.Empty(list);
		Assert.True(result);
	}

	[Fact]
	[Trait("Category", "ListExtension")]
	public void CheckNotEmptyList_ShouldReturnFalse()
	{
		// arrange
		var list = new List<string> { "test1", "test2" };

		// act
		var result = list.IsNullOrEmpty();

		// assert
		Assert.NotNull(list);
		Assert.NotEmpty(list);
		Assert.False(result);
	}
}
