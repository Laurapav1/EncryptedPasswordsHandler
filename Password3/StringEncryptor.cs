using System.Security.Cryptography;
using System.Text;
// using Xunit;
//using NUnit.Framework; // For unit testing

namespace Password3
{

    public class StringEncryptor
    {
        private readonly string encryptionKey;

        public StringEncryptor(string key)
        {

            encryptionKey = (key + "ThisIsA16CharKeyThisIsA16CharKey").Substring(0, 32); // must be 32 characters long
        }


        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentException("Input cannot be null or empty.");

            using Aes aesAlg = Aes.Create();
            aesAlg.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aesAlg.IV = new byte[16]; // Use default IV for simplicity

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            byte[] encryptedBytes;
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                }
                encryptedBytes = msEncrypt.ToArray();
            }

            return Convert.ToBase64String(encryptedBytes);
        }


        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentException("Input cannot be null or empty.");

            using Aes aesAlg = Aes.Create();
            aesAlg.Key = Encoding.UTF8.GetBytes(encryptionKey);
            aesAlg.IV = new byte[16]; // Use default IV for simplicity

            // Use the generated key and IV for decryption (create decryptor instead of encryptor)
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using MemoryStream msDecrypt = new MemoryStream(cipherBytes);
            using CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using StreamReader srDecrypt = new StreamReader(csDecrypt);

            return srDecrypt.ReadToEnd();
        }


        // Unit test method using NUnit framework


        public static void TestEncryptionDecryption()
        {
            string encryptionKey = "YourEncryptionKey"; // Replace with your encryption key
            StringEncryptor encryptor = new StringEncryptor(encryptionKey);

            string originalText = "This is a secret message.";
            string encryptedText = encryptor.Encrypt(originalText);
            string decryptedText = encryptor.Decrypt(encryptedText);

            Console.WriteLine($"Original: {originalText}");
            Console.WriteLine($"Encrypted: {encryptedText}");
            Console.WriteLine($"Decrypted: {decryptedText}");

            if (originalText == decryptedText)
            {
                Console.WriteLine("Encryption and decryption successful. The texts match.");
            }
            else
            {
                Console.WriteLine("Encryption and decryption failed. The texts do not match.");
            }
        }
    }

}
