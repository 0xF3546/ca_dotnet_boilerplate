using backend.DataAccess;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Tests.DataAccess
{
    [TestFixture]
    public class ConfigurationTests
    {
        [Test]
        public void ConfigureDataAccess_ShouldBeCallable()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.ConfigureDataAccess();

            // Assert
            Assert.Pass("ConfigureDataAccess executed without exception");
        }

        [Test]
        public void ConfigureDataAccess_ShouldAcceptServiceCollection()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act & Assert
            Assert.DoesNotThrow(() => services.ConfigureDataAccess());
        }
    }
}
