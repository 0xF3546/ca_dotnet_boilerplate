using backend.Core.Emails;
using Microsoft.Extensions.Configuration;
using Moq;

namespace backend.Tests.Core.Emails
{
    [TestFixture]
    public class EmailServiceTests
    {
        private Mock<IConfiguration> _mockConfiguration;
        private EmailService _emailService;

        [SetUp]
        public void SetUp()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            
            _mockConfiguration.Setup(c => c["Email:FromAddress"]).Returns("noreply@example.com");
            _mockConfiguration.Setup(c => c["Email:FromName"]).Returns("Test Application");
            _mockConfiguration.Setup(c => c["Email:SmtpServer"]).Returns("smtp.example.com");
            _mockConfiguration.Setup(c => c["Email:SmtpPort"]).Returns("587");
            _mockConfiguration.Setup(c => c["Email:Username"]).Returns("testuser");
            _mockConfiguration.Setup(c => c["Email:Password"]).Returns("testpassword");
            _mockConfiguration.Setup(c => c["Email:EnableSsl"]).Returns("true");

            _emailService = new EmailService(_mockConfiguration.Object);
        }

        [Test]
        public void EmailService_ShouldBeInstantiable_WithConfiguration()
        {
            // Assert
            Assert.That(_emailService, Is.Not.Null);
        }

        [Test]
        public void EmailService_Constructor_ShouldAcceptConfiguration()
        {
            // Arrange & Act
            var service = new EmailService(_mockConfiguration.Object);

            // Assert
            Assert.That(service, Is.Not.Null);
        }

        // Note: Actually sending emails requires a real SMTP server or mock SMTP client
        // These tests verify the service structure but don't actually send emails
    }
}
