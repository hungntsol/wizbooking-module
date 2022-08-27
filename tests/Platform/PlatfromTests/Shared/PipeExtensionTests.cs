using SharedCommon.Utils;

namespace PlatformTests.Shared;

[Collection("PipeExtension Test")]
public class PipeExtensionTests
{
	[Theory]
	[InlineData(1, 3, 4)]
	[InlineData(0, 0, 0)]
	[InlineData(1, -1, 0)]
	[InlineData(-1, -2, -3)]
	[Trait("Category", "PipeExtension")]
	public void PipeFunc_GiveNumber_ShouldPlusAdded(int init, int added, int expected)
	{
		// arrange

		// act
		var sum = init.Pipe(q => q + added);

		// assert
		Assert.Equal(expected, sum);
	}

	[Theory]
	[InlineData(1, 3, 4)]
	[InlineData(0, 0, 0)]
	[InlineData(1, -1, 1)]
	[InlineData(-1, -2, -1)]
	[Trait("Category", "PipeExtension")]
	public void PipeIfFunc_GivenNumber_ShouldPlusNoNegativeAdded(int init, int added, int expected)
	{
		// arrange

		// act
		var sum = init.PipeIf(added > 0, q => q += added);

		// assert
		Assert.Equal(expected, sum);
	}
}
