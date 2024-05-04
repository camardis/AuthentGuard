using AuthentGuard.API.Database;
using AuthentGuard.API.Models;
using AuthentGuard.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AuthentGuard.API.Tests
{
    [TestClass]
    public class AuthServiceTests
    {
        private static IConfiguration _configuration;
        private static SimplyDbContext _dbContext;
        private static ILogger<AuthService> _logger;

        [TestInitialize]
        public void TestInitialize()
        {
            // Load test configuration
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json") // Your test configuration file
                .Build();

            // Create DbContext using test database connection string
            var options = new DbContextOptionsBuilder<SimplyDbContext>()
                .UseMySql(_configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 28)))
                .Options;
            _dbContext = new SimplyDbContext(options);

            // Set up logger
            _logger = new Logger<AuthService>(new LoggerFactory());
        }


        [TestMethod]
        public void Authenticate_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var authService = new AuthService(_dbContext, _logger, _configuration);
            var email = "newuser1@example.com";
            var password = "Password123";

            // Act
            var token = authService.Authenticate(email, password);

            // Assert
            Assert.IsNotNull(token);
            // Additional assertions can be added to verify token properties
        }

        [TestMethod]
        public void Authenticate_InvalidCredentials_ReturnsNull()
        {
            // Arrange
            var authService = new AuthService(_dbContext, _logger, _configuration);
            var email = "nonexistent@example.com";
            var password = "wrongpassword";

            // Act
            var token = authService.Authenticate(email, password);

            // Assert
            Assert.IsNull(token);
        }

        [TestMethod]
        public void Register_NewUser_SuccessfullyRegistered()
        {
            // Arrange
            var authService = new AuthService(_dbContext, _logger, _configuration);
            var newRegister = new Register
            {
                Email = "newuser1@example.com",
                Password = "Password123",
                AgrredToTerms = true // Assuming the terms are agreed
            };

            // Act
            var result = authService.Register(newRegister);

            // Assert
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void Register_ExistingUser_ReturnsUserAlreadyExists()
        {
            // Arrange
            var authService = new AuthService(_dbContext, _logger, _configuration);
            var existingUser = new Register
            {
                Email = "existinguser@example.com",
                Password = "password",
                AgrredToTerms = true // Assuming the terms are agreed
            };

            // Add the existing user to the database
            _dbContext.Registers.Add(existingUser);
            _dbContext.SaveChanges();

            var newRegister = new Register
            {
                Email = "existinguser@example.com", // Using existing email
                Password = "password",
                AgrredToTerms = true
            };

            // Act
            var result = authService.Register(newRegister);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("User already exists", result.Message);
        }

        [TestMethod]
        public void ValidateToken_ValidToken_ReturnsTrue()
        {
            // Arrange
            var authService = new AuthService(_dbContext, _logger, _configuration);
            var email = "test@example.com";
            var password = "Password123";
            var token = authService.Authenticate(email, password);

            // Act
            var isValid = authService.ValdidateToken(token);

            // Assert
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void ValidateToken_InvalidToken_ReturnsFalse()
        {
            // Arrange
            var authService = new AuthService(_dbContext, _logger, _configuration);
            var invalidToken = "invalid_token";

            // Act
            var isValid = authService.ValdidateToken(invalidToken);

            // Assert
            Assert.IsFalse(isValid);
        }
    }
}
