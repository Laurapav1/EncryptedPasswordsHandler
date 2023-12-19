
namespace Password3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Master Password:");
            string? encryptionKey = tPassword.ReadPasswordConsole();


            if (!File.Exists("drowssapotpyrc.json"))
            {
                //create a file at pathName
                File.Create("drowssapotpyrc.json");
            }

            PasswordList passwords = new PasswordList("drowssapotpyrc.json", encryptionKey);
            try
            {
                passwords.DecryptFromFile();
            }
            catch (Exception e)
            {
                Console.WriteLine("The password is not correct. Try again!");
                Console.WriteLine($"Error: {e.Message}");
                return;
            }

            Console.WriteLine($"{passwords.Count()} key value pairs loaded from file {passwords.FilePath}.");

            // Enter UI loop
            ConsoleKeyInfo command = new ConsoleKeyInfo('n', ConsoleKey.N, false, false, false);
            do
            {
                string? theKey = null;
                switch (command.Key)
                {
                    case ConsoleKey.E:
                        if (theKey == null)
                        {
                            Console.Write("\nThe key: ");
                            theKey = Console.ReadLine();
                        }
                        if (theKey == null) return;
                        string? theValue = passwords.FindPassword(theKey);
                        if (theValue == null) return;
                        Console.Write($"Old value: {theValue}. New value: ");
                        theValue = Console.ReadLine();
                        if (theValue != null)
                        {
                            if (passwords.ReplacePassword(theKey, theValue))
                            {
                                Console.WriteLine($"{theKey} => {theValue}");
                                passwords.SaveEncryptedToFile();
                            }
                        }
                        break;

                    case ConsoleKey.D:
                        Console.Write("\nThe key: ");
                        theKey = Console.ReadLine();

                        //Now search for theKey in the database and show it if is there. If not we are about to add a new one ;-)
                        if (theKey == null) return;
                        theValue = passwords.FindPassword(theKey);
                        if (theValue != null)
                        {
                            string sSpaces = new String(' ', theKey.Length + 9);
                            Console.Write(sSpaces);
                            Console.Write($" => {theValue}\nDelete (y/n)>");
                            command = Console.ReadKey();
                            if (command.Key == ConsoleKey.Y)
                            {
                                passwords.RemovePassword(theKey);
                                Console.WriteLine($"\n{theKey} deleted succesful.");
                                passwords.SaveEncryptedToFile();
                            }
                        }
                        break;

                    case ConsoleKey.A:
                        Console.WriteLine();
                        foreach (var Password in passwords)
                            Console.WriteLine($"Key: {Password.Key}, Password: {Password.Value}");
                        theKey = null;
                        break;

                    case ConsoleKey.N:
                        if (args.Length > 0)
                        {
                            theKey = args[0];
                            Console.Write($"The key: {theKey} ");
                        }
                        else
                        {
                            Console.Write("\nThe key: ");
                            theKey = Console.ReadLine();
                        }

                        //Now search for theKey in the database and show it if is there. If not we are about to add a new one ;-)
                        if (theKey == null) return;
                        theValue = passwords.FindPassword(theKey);
                        if (theValue != null)
                        {
                            string sSpaces = new String(' ', theKey.Length + 9);
                            Console.Write(sSpaces);
                            Console.WriteLine($" => {theValue}");
                        }
                        else
                        {
                            // Key not found - we have to enter value
                            Console.Write("The value: ");
                            theValue = Console.ReadLine();

                            if ((theKey == null) || (theValue == null)) return;

                            passwords.AddPassword(theKey, theValue);
                            passwords.SaveEncryptedToFile();
                        }
                        break;

                    default:
                        Console.WriteLine("\nInvalid choice. Please enter 'e', 'n', 'a', or 'v'.");
                        break;
                }
                Console.Write("(e)dit, (n)ext, view (a)ll, (d)elete, (q)uit >");
                command = Console.ReadKey();

            } while ((command.Key != ConsoleKey.Q) || (args.Length > 0));
            Console.Clear();
        }
    }
}