using System.Text.RegularExpressions;
public class tPassword
{
    public const string allowedPasswordChars = "ABCDEFGHIJKLMNOPQRSTUVXYZabcdefghijklmnopqrstuvxyz0123456789!@#$%^&*()-_+=";

    public tPassword() { }

    public static string ReadPasswordConsole()
    {
        string pass = new string("");
        ConsoleKey key;
        do
        {
            var keyInfo = Console.ReadKey(intercept: true);
            key = keyInfo.Key;

            if (key == ConsoleKey.Backspace && pass.Length > 0)
            {
                Console.Write("\b \b");
                pass = pass[0..^1];
            }
            else if (allowedPasswordChars.Contains(keyInfo.KeyChar))
            {
                Console.Write("*");
                pass += keyInfo.KeyChar;
            }
            else
            {
                Console.Beep();
            }
        } while (key != ConsoleKey.Enter);
        Console.WriteLine(pass);
        return pass;
    }

    public static bool IsPasswordValid(string password)
    {
        // Define a regular expression pattern to match US characters (letters and digits)
        // and allowed special characters.
        const string pattern = "^[A-Za-z0-9!@#$%^&*()-_+=]{8,}$";

        // Check if the password matches the pattern.
        return Regex.IsMatch(password, pattern);
    }
}