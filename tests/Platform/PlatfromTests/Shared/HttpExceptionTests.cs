using SharedCommon.Exceptions.StatusCodes;
using SharedCommon.Exceptions.StatusCodes._400;
using SharedCommon.Exceptions.StatusCodes._500;

namespace PlatformTests.Shared;

[Collection("HttpException Test")]
public class HttpExceptionTests
{
    [Fact]
    [Trait("Category", "HttpException")]
    public void HttpException_GetStatusCodeAndMessage_ThrowDataNotFoundException()
    {
        // arrange

        // act
        Action act = () => throw new DataNotFoundException();

        // assert
        var ex = Assert.Throws<DataNotFoundException>(act);
        Assert.Equal("Data not found", ex.Message);
        Assert.Equal(400, ex.HttpCode);
    }

    [Fact]
    [Trait("Category", "HttpException")]
    public void HttpException_GetStatusCodeAndMessage_ThrowUnauthorizedException()
    {
        // arrange

        // act
        Action act = () => throw new UnauthorizedException();

        // assert
        var ex = Assert.Throws<UnauthorizedException>(act);
        Assert.Equal("You must authorized to perform this action", ex.Message);
        Assert.Equal(401, ex.HttpCode);
    }

    [Fact]
    [Trait("Category", "HttpException")]
    public void HttpException_GetStatusCodeAndMessage_ThrowForbiddenException()
    {
        // arrange

        // act
        Action act = () => throw new ForbiddenException();

        // assert
        var ex = Assert.Throws<ForbiddenException>(act);
        Assert.Equal("You have no access", ex.Message);
        Assert.Equal(403, ex.HttpCode);
    }

    [Fact]
    [Trait("Category", "HttpException")]
    public void HttpException_GetStatusCodeAndMessage_ThrowUnavailableServiceException()
    {
        // arrange

        // act
        Action act = () => throw new UnavailableServiceException();

        // assert
        var ex = Assert.Throws<UnavailableServiceException>(act);
        Assert.Equal("Some services is not available now", ex.Message);
        Assert.Equal(503, ex.HttpCode);
    }

    [Fact]
    [Trait("Category", "HttpException")]
    public void HttpException_GetStatusCodeAndMessage_ThrowInternalServerException()
    {
        // arrange

        // act
        Action act = () => throw new InternalServerException();

        // assert
        var ex = Assert.Throws<InternalServerException>(act);
        Assert.Equal("Server is downed", ex.Message);
        Assert.Equal(500, ex.HttpCode);
    }

    [Fact]
    [Trait("Category", "HttpException")]
    public void HttpException_GetStatusCodeAndMessage_ThrowAnCustomFromBaseHttpException()
    {
        // arrange

        // act
        Action act = () => throw new CustomHttpBaseException(1000, "Custom message");

        // assert
        var ex = Assert.Throws<CustomHttpBaseException>(act);
        Assert.Equal("Custom message", ex.Message, true);
        Assert.Equal(1000, ex.HttpCode);
    }
}

public class CustomHttpBaseException : HttpBaseException
{
    public CustomHttpBaseException(int code, string message) : base(code, message)
    {
    }
}
