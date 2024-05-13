using BoggleEngines;
using NUnit.Framework;
using System;

namespace BoggleUnitTests.BoggleEnginesTests
{
    [TestFixture]
    public class ValidationTests
    {
        private Validation validator;

        [SetUp]
        public void Setup()
        {
            validator = new Validation();
        }

        [TestCase("Alice")]
        [TestCase("bob123")]
        [TestCase("Charlie_321")]
        [TestCase("delta.echo")]
        [TestCase("TestUser")]
        public void ValidateUsername_ValidUsernames_ReturnsTrue(string username)
        {
            bool result = validator.validateUsername(username);
            Assert.That(result, Is.True, $"Expected true for valid username: {username}");
        }

        [TestCase("Alice@123")] 
        [TestCase("bob#123")]   
        [TestCase("")]          
        [TestCase(null)]        
        public void ValidateUsername_InvalidUsernames_ReturnsFalse(string username)
        {
            bool result = validator.validateUsername(username);
            Assert.That(result, Is.False, $"Expected false for invalid username: {username}");
        }

        [TestCase("StrongP1!")]
        [TestCase("Passw0rd!")]
        [TestCase("Password1")]

        public void ValidatePassword_ValidPasswords_ReturnsTrue(string password)
        {
            bool result = validator.validatePassword(password);
            Assert.That(result, Is.True, $"Expected true for valid password: {password}");
        }

        [TestCase("weak")]             
        [TestCase("alllowercase")]    
        [TestCase("ALLUPPERCASE")]     
        [TestCase("12345678")]         
        [TestCase("NoNums!")]         
        [TestCase("nouppercase1")]    
        [TestCase("NOLOWERCASE1")]   
        public void ValidatePassword_InvalidPasswords_ReturnsFalse(string password)
        {
            bool result = validator.validatePassword(password);
            Assert.That(result, Is.False, $"Expected false for invalid password: {password}");
        }
    }
}
