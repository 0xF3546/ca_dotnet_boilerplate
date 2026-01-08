using backend.DataAccess;
using backend.DataAccess.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace backend.Tests.DataAccess
{
    [TestFixture]
    public class DatabaseConfigurationTests
    {
        [Test]
        public void ConfigureDatabase_ShouldAddDbContext_WhenConnectionStringExists()
        {
            // Arrange
            var services = new ServiceCollection();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockConnectionStringsSection = new Mock<IConfigurationSection>();
            var mockAppDbContextSection = new Mock<IConfigurationSection>();
            
            mockAppDbContextSection.Setup(s => s.Value)
                .Returns("Host=localhost;Database=testdb;Username=user;Password=pass");
            mockConnectionStringsSection.Setup(s => s.GetSection("AppDbContext"))
                .Returns(mockAppDbContextSection.Object);
            mockConnectionStringsSection.Setup(s => s["AppDbContext"])
                .Returns("Host=localhost;Database=testdb;Username=user;Password=pass");
            mockConfiguration.Setup(c => c.GetSection("ConnectionStrings"))
                .Returns(mockConnectionStringsSection.Object);

            // Act
            services.ConfigureDatabase(mockConfiguration.Object);

            // Assert
            var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(AppDbContext));
            Assert.That(descriptor, Is.Not.Null);
        }

        [Test]
        public void ConfigureDatabase_ShouldThrowException_WhenConnectionStringIsNull()
        {
            // Arrange
            var services = new ServiceCollection();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockConnectionStringsSection = new Mock<IConfigurationSection>();
            var mockAppDbContextSection = new Mock<IConfigurationSection>();
            
            mockAppDbContextSection.Setup(s => s.Value).Returns((string?)null);
            mockConnectionStringsSection.Setup(s => s.GetSection("AppDbContext"))
                .Returns(mockAppDbContextSection.Object);
            mockConfiguration.Setup(c => c.GetSection("ConnectionStrings"))
                .Returns(mockConnectionStringsSection.Object);

            // Act & Assert
            Assert.Throws<Exception>(() => services.ConfigureDatabase(mockConfiguration.Object));
        }

        [Test]
        public void ConfigureDatabase_ShouldThrowException_WhenConnectionStringIsEmpty()
        {
            // Arrange
            var services = new ServiceCollection();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockConnectionStringsSection = new Mock<IConfigurationSection>();
            var mockAppDbContextSection = new Mock<IConfigurationSection>();
            
            // GetConnectionString returns null when value is empty
            mockAppDbContextSection.Setup(s => s.Value).Returns(string.Empty);
            mockConnectionStringsSection.Setup(s => s.GetSection("AppDbContext"))
                .Returns(mockAppDbContextSection.Object);
            mockConfiguration.Setup(c => c.GetSection("ConnectionStrings"))
                .Returns(mockConnectionStringsSection.Object);

            // Act & Assert
            Assert.Throws<Exception>(() => services.ConfigureDatabase(mockConfiguration.Object));
        }

        [Test]
        public void ConfigureDatabase_ShouldRegisterDbContextWithCorrectLifetime()
        {
            // Arrange
            var services = new ServiceCollection();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockConnectionStringsSection = new Mock<IConfigurationSection>();
            var mockAppDbContextSection = new Mock<IConfigurationSection>();
            
            mockAppDbContextSection.Setup(s => s.Value)
                .Returns("Host=localhost;Database=testdb;Username=user;Password=pass");
            mockConnectionStringsSection.Setup(s => s.GetSection("AppDbContext"))
                .Returns(mockAppDbContextSection.Object);
            mockConnectionStringsSection.Setup(s => s["AppDbContext"])
                .Returns("Host=localhost;Database=testdb;Username=user;Password=pass");
            mockConfiguration.Setup(c => c.GetSection("ConnectionStrings"))
                .Returns(mockConnectionStringsSection.Object);

            // Act
            services.ConfigureDatabase(mockConfiguration.Object);

            // Assert
            var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(AppDbContext));
            Assert.That(descriptor, Is.Not.Null);
            Assert.That(descriptor!.Lifetime, Is.EqualTo(ServiceLifetime.Scoped));
        }
    }
}
