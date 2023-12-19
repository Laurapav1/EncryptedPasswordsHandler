using Password3;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace Password3
{
    public class PasswordList : IEnumerable<KeyValuePair<string, string>>
    {
        private readonly string filePath;
        private readonly string encryptionKey;
        private Dictionary<string, string> passwords;
        private readonly StringEncryptor encryptor;

        public PasswordList(string path, string key)
        {
            filePath = path;
            encryptionKey = key;
            passwords = new Dictionary<string, string>();
            encryptor = new StringEncryptor(encryptionKey);
        }

        public void ShowAllPasswords()
        {
            foreach (var password in passwords)
            {
                Console.WriteLine($"Key: {password.Key}, Password: {password.Value}");
            }
        }



        public void Decrypt()
        {
            try
            {
                string encryptedData = File.ReadAllText(filePath) ?? "";
                string decryptedData = encryptor.Decrypt(encryptedData) ?? "";
                passwords.Clear(); // Clear existing passwords before adding decrypted ones
                                   // Deserialize decrypted data and populate the 'passwords' dictionary
                                   // Example (assuming JSON data):
                passwords = JsonSerializer.Deserialize<Dictionary<string, string>>(decryptedData) ?? new Dictionary<string, string>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while decrypting: {ex.Message}");
                throw; // Rethrow the exception or handle it accordingly
            }
        }

       


        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return passwords.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // Opdater SaveEncryptedToFile metoden i PasswordList-klassen
        // Opdater SaveEncryptedToFile metoden i PasswordList-klassen
        public void SaveEncryptedToFile()
        {
            try
            {
                if (passwords.Count == 0)
                {
                    Console.WriteLine("No data to save.");
                    return;
                }

                Dictionary<string, string> encryptedPasswords = new Dictionary<string, string>();

                // Encrypt keys and values before saving
                foreach (var password in passwords)
                {
                    string encryptedKey = encryptor.Encrypt(password.Key);
                    string encryptedValue = encryptor.Encrypt(password.Value);

                    // Convert encrypted key and value to Base64 strings before saving
                    string base64EncryptedKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(encryptedKey));
                    string base64EncryptedValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(encryptedValue));

                    encryptedPasswords.Add(base64EncryptedKey, base64EncryptedValue);
                }

                // Convert encrypted passwords to JSON and save to file
                string json = JsonSerializer.Serialize(encryptedPasswords);
                File.WriteAllText(filePath, json);

                Console.WriteLine("Encrypted passwords saved to file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while saving encrypted data: {ex.Message}");
                throw;
            }
        }

        // Opdater DecryptFromFile metoden i PasswordList-klassen
        public void DecryptFromFile()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("File does not exist or cannot be found.");
                    return;
                }

                string encryptedJson = File.ReadAllText(filePath);
                Dictionary<string, string> encryptedPasswords = JsonSerializer.Deserialize<Dictionary<string, string>>(encryptedJson);

                // Clear existing passwords before decrypting from file
                passwords.Clear();

                // Decrypt keys and values after reading from file
                foreach (var encryptedPassword in encryptedPasswords)
                {
                    try
                    {
                        // Dekod Base64-strengene til bytes
                        byte[] encryptedKeyBytes = Convert.FromBase64String(encryptedPassword.Key);
                        byte[] encryptedValueBytes = Convert.FromBase64String(encryptedPassword.Value);

                        // Konverter bytes til tekst før dekryptering
                        string encryptedKey = Encoding.UTF8.GetString(encryptedKeyBytes);
                        string encryptedValue = Encoding.UTF8.GetString(encryptedValueBytes);

                        string decryptedKey = encryptor.Decrypt(encryptedKey);
                        string decryptedValue = encryptor.Decrypt(encryptedValue);

                        passwords.Add(decryptedKey, decryptedValue);
                        Console.WriteLine($"Passwords decrypted from file: {filePath}.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error while decrypting from file: {ex.Message}");
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while decrypting from file: {ex.Message}");
                throw;
            }
        }



        // Other methods for managing passwords in the dictionary
        public void AddPassword(string key, string value)
        {
            passwords[key] = value;
        }

        public bool ReplacePassword(string key, string newValue)
        {
            if (passwords.ContainsKey(key))
            {
                passwords[key] = newValue;
                return true;
            }
            return false;
        }

        public string? FindPassword(string key)
        {
            return passwords.TryGetValue(key, out var value) ? value : null;
        }

        public void RemovePassword(string key)
        {
            if (passwords.ContainsKey(key))
            {
                passwords.Remove(key);
            }
        }

        public int Count()
        {
            return passwords.Count;
        }
        public string FilePath => filePath;
    }
}
