using backend.Core.Accounts;
using backend.DataAccess.Accounts;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace backend.Tests.DataAccess.Accounts
{
    [TestFixture]
    public class AuthRepositoryTests
    {
        private Mock<SignInManager<AppUser>> _mockSignInManager;
        private Mock<UserManager<AppUser>> _mockUserManager;
        private AuthRepository _authRepository;

        [SetUp]
        public void SetUp()
        {
            var userStoreMock = new Mock<IUserStore<AppUser>>();
            _mockUserManager = new Mock<UserManager<AppUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contextAccessor = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<AppUser>>();
            _mockSignInManager = new Mock<SignInManager<AppUser>>(
                _mockUserManager.Object,
                contextAccessor.Object,
                claimsFactory.Object,
                null, null, null, null);

            _authRepository = new AuthRepository(_mockSignInManager.Object, _mockUserManager.Object);
        }

        [Test]
        public void AuthRepository_ShouldBeInstantiable()
        {
            // Assert
            Assert.That(_authRepository, Is.Not.Null);
        }

        [Test]
        public async Task CreateAsync_ShouldThrowArgumentNullException_WhenDtoIsNull()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _authRepository.CreateAsync(null!));
        }

        [Test]
        public async Task CreateAsync_ShouldThrowArgumentException_WhenEmailIsEmpty()
        {
            // Arrange
            var dto = new CreateUserDto
            {
                Email = "",
                UserName = "testuser",
                Password = "Test123!"
            };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _authRepository.CreateAsync(dto));
        }

        [Test]
        public async Task CreateAsync_ShouldThrowArgumentException_WhenUserNameIsEmpty()
        {
            // Arrange
            var dto = new CreateUserDto
            {
                Email = "test@example.com",
                UserName = "",
                Password = "Test123!"
            };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _authRepository.CreateAsync(dto));
        }

        [Test]
        public async Task CreateAsync_ShouldThrowInvalidOperationException_WhenUserExists()
        {
            // Arrange
            var dto = new CreateUserDto
            {
                Email = "existing@example.com",
                UserName = "existinguser",
                Password = "Test123!"
            };

            var existingUser = new AppUser { Email = dto.Email, UserName = dto.UserName };
            _mockUserManager.Setup(x => x.FindByEmailAsync(dto.Email))
                .ReturnsAsync(existingUser);

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _authRepository.CreateAsync(dto));
        }

        [Test]
        public async Task DeleteAsync_ShouldThrowInvalidOperationException_WhenUserNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockUserManager.Setup(x => x.FindByIdAsync(userId.ToString()))
                .ReturnsAsync((AppUser?)null);

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _authRepository.DeleteAsync(userId));
        }

        [Test]
        public async Task GetByEmail_ShouldReturnNull_WhenUserNotFound()
        {
            // Arrange
            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((AppUser?)null);

            // Act
            var result = await _authRepository.GetByEmail("notfound@example.com");

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetById_ShouldThrowArgumentException_WhenIdIsEmpty()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _authRepository.GetById(Guid.Empty));
        }

        [Test]
        public async Task GetById_ShouldReturnNull_WhenUserNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockUserManager.Setup(x => x.FindByIdAsync(userId.ToString()))
                .ReturnsAsync((AppUser?)null);

            // Act
            var result = await _authRepository.GetById(userId);

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}
