using MediatR;
using PlatformTests.PredicateBuilder;
using SharedCommon.Commons.HttpResponse;

namespace PlatformTests.Shared;

[Collection("JsonHttpResponse Test")]
public class JsonHttpResponseTest
{
	[Fact]
	[Trait("Category", "JsonHttpResponse")]
	public void SuccessWithNoDataNoMessage_ShouldReturnUnitValue()
	{
		// arrange

		// act
		var response = JsonHttpResponse.Success();

		// assert
		Assert.NotNull(response);
		Assert.Null(response.Message);
		Assert.Equal(Unit.Value, response.Data);
		Assert.Equal(200, response.Status);
		Assert.True(response.IsSuccess);
	}

	[Fact]
	[Trait("Category", "JsonHttpResponse")]
	public void SuccessWithDataNoMessage_ShouldReturnData()
	{
		// arrange
		var data = new FakeAccount("test", 1);

		// act
		var response = JsonHttpResponse.Success(data);

		// assert
		Assert.NotNull(response);
		Assert.Null(response.Message);
		Assert.Equal(data, response.Data);
		Assert.Equal(200, response.Status);
		Assert.True(response.IsSuccess);
	}

	[Theory]
	[Trait("Category", "JsonHttpResponse")]
	[InlineData("Oke la")]
	[InlineData("Oke ne")]
	[InlineData("Hokage")]
	public void SuccessWithDataWithMessage_ShouldReturnDataWithMessage(string message)
	{
		// arrange
		var data = new FakeAccount("test", 1);

		// act
		var response = JsonHttpResponse.Success(data, message);

		// assert
		Assert.NotNull(response);
		Assert.NotNull(response.Message);
		Assert.Equal(message, response.Message);
		Assert.Equal(data, response.Data);
		Assert.Equal(200, response.Status);
		Assert.True(response.IsSuccess);
	}

	[Fact]
	[Trait("Category", "JsonHttpResponse")]
	public void FailNoMessage_ShouldReturnNoMessage()
	{
		// arrange

		// act
		var response = JsonHttpResponse.Fail();

		// assert
		Assert.NotNull(response);
		Assert.NotNull(response.Message);
		Assert.Equal("Cannot find anything!", response.Message);
		Assert.Null(response.Data);
		Assert.Equal(400, response.Status);
		Assert.False(response.IsSuccess);
	}

	[Theory]
	[Trait("Category", "JsonHttpResponse")]
	[InlineData("No no no")]
	[InlineData("Out out 123")]
	public void FailWithMessage_ShouldReturnWithMessage(string message)
	{
		// arrange

		// act
		var response = JsonHttpResponse.Fail(message);

		// assert
		Assert.NotNull(response);
		Assert.NotNull(response.Message);
		Assert.Null(response.Data);
		Assert.Equal(message, response.Message);
		Assert.Equal(400, response.Status);
		Assert.False(response.IsSuccess);
	}
}
