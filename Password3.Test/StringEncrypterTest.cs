namespace Password3.Test;

public class StringEncryptorTest
{
    [Fact]
    public void Encrypt_Decrypt_Successful()
    {
        // Arrange
        string encryptionKey = "YourEncryptionKey"; // Skift dette med din faktiske nøgle
        StringEncryptor encryptor = new StringEncryptor(encryptionKey);

        string originalText = "This is a secret message.";

        // Act
        string encryptedText = encryptor.Encrypt(originalText);
        string decryptedText = encryptor.Decrypt(encryptedText);

        // Assert
        Assert.Equal(originalText, decryptedText);
    }
}