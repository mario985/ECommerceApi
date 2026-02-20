using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using StackExchange.Redis;

public class AuthServiceTests
{
     private readonly Mock<IUserRepository> _userRepositoryMock;
     private readonly Mock<IRefreshTokenService> _refreshTokenServiceMock;
     private readonly Mock<ItokenService> _tokenServiceMock;
     private readonly IAuthService _authService;
     public AuthServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _tokenServiceMock = new Mock<ItokenService>();
        _refreshTokenServiceMock = new Mock<IRefreshTokenService>();
        _authService = new AuthService(
            _userRepositoryMock.Object,
            _tokenServiceMock.Object,
            _refreshTokenServiceMock.Object
        );
    }
    [Fact]
    public async Task LogInAsync_should_return_failure_if_userDoesNotExist()
    {
        // arrange
       _userRepositoryMock.Setup(x =>x.GetByEmailWithTokensAsync("test@mail.com")).ReturnsAsync((User?)null);
        // act
       var result = await  _authService.LogInAsync("test@mail.com" , "123");    
        // assert
        Assert.False(result.IsAuthenticated);
        Assert.Equal("Email or password is not correct" , result.Message);
        _tokenServiceMock.Verify(x =>x.GenerateToken(It.IsAny<string>() , It.IsAny<string>() , It.IsAny<User>()) , Times.Never);
    }
    [Fact]
    public async Task LogInAsync_should_return_failure_if_passwordIsInvalid()
    {
        // arrange
        var user = new User{Email ="test@mail.com"};
       _userRepositoryMock.Setup(x =>x.GetByEmailWithTokensAsync("test@mail.com"))
       .ReturnsAsync(user);
       _userRepositoryMock.Setup(x=>x.CheckPassword(It.IsAny<User>() , "wrong"))
       .ReturnsAsync(false);
    
        // Act
       var result = await  _authService.LogInAsync("test@mail.com" , "wrong");  

        // assert
        Assert.False(result.IsAuthenticated);
        Assert.Equal("Email or password is not correct" , result.Message);
        _tokenServiceMock.Verify(x =>x.GenerateToken(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<User>()
            ),Times.Never);
    }
    [Fact]
    public async Task LoginAsync_ShouldReturnSuccess_WhenCredentialsAreValid()
    {
        // Arrange
        var user = new User
        {
            Email = "test@mail.com",
            UserName = "mario",
            RefreshTokens = new List<RefreshToken>()
        };

        var refreshToken = new RefreshToken
        {
            Token = "refresh-token",
            ExpiresOn = DateTime.UtcNow.AddDays(7),
            CreatedOn = DateTime.UtcNow,
            
        };

        _userRepositoryMock
            .Setup(x => x.GetByEmailWithTokensAsync(user.Email))
            .ReturnsAsync(user);

        _userRepositoryMock
            .Setup(x => x.CheckPassword(user, "123"))
            .ReturnsAsync(true);

        _userRepositoryMock
            .Setup(x => x.GetRole(user))
            .ReturnsAsync("Admin");

        _tokenServiceMock
            .Setup(x => x.GenerateToken(user.UserName, "Admin", user))
            .Returns("access-token");

        _refreshTokenServiceMock
            .Setup(x => x.GenerateRefreshToken())
            .Returns(refreshToken);

        // Act
        var result = await _authService.LogInAsync(user.Email, "123");

        // Assert
        Assert.True(result.IsAuthenticated);
        Assert.Equal("access-token", result.Token);
        Assert.Equal("refresh-token", result.RefreshToken);
        Assert.Equal("Admin", result.Role);

        _tokenServiceMock.Verify(x => x.GenerateToken(user.UserName, "Admin", user), Times.Once);
        _refreshTokenServiceMock.Verify(x => x.GenerateRefreshToken(), Times.Once);
        _userRepositoryMock.Verify(x => x.UpdateAsync(user), Times.Once);
    }
      [Fact]
    public async Task LoginAsync_should_revoke_oldRefreshTokens()
    {
        var oldToken = new RefreshToken
        {
         ExpiresOn = DateTime.UtcNow.AddDays(7)
        };
        // Arrange
        var user = new User
        {
            Email = "test@mail.com",
            UserName = "mario",
            RefreshTokens = new List<RefreshToken>{oldToken}
        };

        var refreshToken = new RefreshToken
        {
            Token = "refresh-token",
            ExpiresOn = DateTime.UtcNow.AddDays(7),
            CreatedOn = DateTime.UtcNow,
            
        };

        _userRepositoryMock
            .Setup(x => x.GetByEmailWithTokensAsync(user.Email))
            .ReturnsAsync(user);

        _userRepositoryMock
            .Setup(x => x.CheckPassword(user, "123"))
            .ReturnsAsync(true);

        _userRepositoryMock
            .Setup(x => x.GetRole(user))
            .ReturnsAsync("Admin");

        _tokenServiceMock
            .Setup(x => x.GenerateToken(user.UserName, "Admin", user))
            .Returns("access-token");

        _refreshTokenServiceMock
            .Setup(x => x.GenerateRefreshToken())
            .Returns(refreshToken);

        // Act
        var result = await _authService.LogInAsync(user.Email, "123");

        // Assert
        Assert.NotNull(oldToken.RevokedOn);
    }

}