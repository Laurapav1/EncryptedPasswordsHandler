using Password3;
using Xunit;

namespace Password3.Test;

public class UnitTest1
{
    private const string FilePath = "test_passwords.json";
    private const string EncryptionKey = "your_encryption_key"; // Skift denne med din faktiske nøgle

    [Fact]
    public void AddPassword_And_FindPassword()
    {
        // Arrange - Sætter det op
        var passwordList = new PasswordList(FilePath, EncryptionKey);

        // Act - bruger funktionerne
        passwordList.AddPassword("username", "password");
        var foundPassword = passwordList.FindPassword("username");

        // Assert - Tester at det hele virker som det skulle
        Assert.Equal("password", foundPassword);
    }

    [Fact]
    public void ReplacePassword()
    {
        // Arrange
        var passwordList = new PasswordList(FilePath, EncryptionKey);
        passwordList.AddPassword("username", "password");

        // Act
        var result = passwordList.ReplacePassword("username", "new_password");

        // Assert
        Assert.True(result);
        Assert.Equal("new_password", passwordList.FindPassword("username"));
    }
}