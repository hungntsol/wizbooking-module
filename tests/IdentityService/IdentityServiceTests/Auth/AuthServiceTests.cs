using Identity.Domain.Entities;
using Identity.Infrastructure.Services;
using Identity.Infrastructure.SettingOptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;

namespace IdentityServiceTests.Auth;

internal class MockAppSettingIConfiguration : Mock<IOptions<AuthAppSetting>>
{
    public MockAppSettingIConfiguration Setup()
    {
        SetupGet(q => q.Value).Returns(new AuthAppSetting
        {
            SecretKey = "yxAxeBr9fm3sFu6UjWUprJhbPOFZwyy4",
            ExpirationMinutes = 1
        });

        return this;
    }
}

[Collection("AuthService")]
public class AuthServiceTests
{
    [Fact]
    [Trait("Category", "AuthService JWT")]
    public void Should_GivenUser_ReturnAccessToken()
    {
        // arrange
        var account = UserAccount.New("hung@mail.com", "test@123", "hung", "nguyen", "MALE", "ADMIN");
        var mockAuthSetting = new MockAppSettingIConfiguration().Setup();
        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

        var authService = new AuthService(mockHttpContextAccessor.Object, mockAuthSetting.Object);

        // act
        var accessToken = authService.GenerateAccessToken(account);

        // assert
        Assert.NotNull(accessToken);
        Assert.True(accessToken.Length > 15);
    }
}
