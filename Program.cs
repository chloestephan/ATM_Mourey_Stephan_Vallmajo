using System;
using System.Data.SQLite;

namespace ATM
{
    class Program
    {
        Database databaseObject = new Database();

        public Program()
        {
            Console.Title = "FQC's ATM system";
        }

        public void MainMenu()
        {
            Console.Clear();
            Center("**** Welcome to GTBPI Banking System ****\n");
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            DrawLine();
            Console.WriteLine("|{0}|", AlignText(0, ""));
            Console.WriteLine("|{0}|", AlignText(35, "1. Log In as Admin"));
            Console.WriteLine("|{0}|", AlignText(35, "2. Log In as Client"));
            Console.WriteLine("|{0}|", AlignText(35, "3. Exit"));
            Console.WriteLine("|{0}|", AlignText(35, "4. About Us"));
            Console.WriteLine("|{0}|", AlignText(0, ""));
            DrawLine();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("\n{0}", AlignText(36, "Enter your choice : ", "L"));
            int choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    LogInAdmin();
                    break;
                case 2:
                    LogInClient();
                    break;
                case 3:
                    About();
                    break;
                case 4:
                    Console.WriteLine("\n");
                    Center("Thanks for using our service!");
                    Center("Press any key to close the console.");
                    Console.ReadKey();
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Incorrect choice, please try again.");
                    break;
            }
        }

        /* --- ADMIN --- */
        public void AdminMenu()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            DrawLine();
            Console.WriteLine("|{0}|", AlignText(0, ""));
            Console.WriteLine("|{0}|", AlignText(37, "1. Create a client"));
            Console.WriteLine("|{0}|", AlignText(37, "2. Manage a client"));
            Console.WriteLine("|{0}|", AlignText(37, "3. Verify user transactions"));
            Console.WriteLine("|{0}|", AlignText(37, "4. View all clients"));
            Console.WriteLine("|{0}|", AlignText(37, "5. Logout"));
            Console.WriteLine("|{0}|", AlignText(0, ""));
            DrawLine();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("\n{0}", AlignText(38, "Enter your choice : ", "L"));
            int choice = Convert.ToInt32(Console.ReadLine());

            while (true)
            {
                switch (choice)
                {
                    case 1:
                        CreateClient();
                        break;
                    case 2:
                        ManageClient();
                        break;
                    case 3:
                        VerifyTransactions();
                        break;
                    case 4:
                        GetAll();
                        break;
                    case 5:
                        LogOutAdmin();
                        break;
                    default:
                        Console.WriteLine("Incorrect choice, please try again.");
                        break;
                }
                Console.ReadKey();
                Console.Clear();
            }
        }

        public void LogInAdmin()
        {
            Console.WriteLine("Enter username:");
            string usernameInput = Console.ReadLine();
            Console.WriteLine("Enter password:");
            string passwordInput = Console.ReadLine();

            if (usernameInput == "username" && passwordInput == "password")
            {
                Console.WriteLine("Connection successful");
                AdminMenu();
            }
            else
            {
                Console.WriteLine("Please Try again");
            }
        }

        public void ManageClient()
        {
            //change data in database
            //update function
        }

        public void VerifyTransactions()
        {
            //change value in the transactions database
            //if verified : bool true ; if not verified : bool false
        }
        public void CreateClient()
        {
            string query = "INSERT INTO clients ('GUID', 'FirstName', 'LastName', 'PIN', 'MainCurrency', 'isBlocked') VALUES (@GUID, @FirstName, @LastName, @PIN, @MainCurrency, '@isBlocked')";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            databaseObject.OpenConnection();

            Console.Write("Enter the GUID of the client:");
            string GUIDClient = Console.ReadLine();
            myCommand.Parameters.AddWithValue("@GUID", GUIDClient);

            Console.Write("Enter the First Name of the client:");
            string FirstName = Console.ReadLine();
            myCommand.Parameters.AddWithValue("@FirstName", FirstName);

            Console.Write("Enter the Last Name of the client:");
            string LastName = Console.ReadLine();
            myCommand.Parameters.AddWithValue("@LastName", LastName);

            Console.Write("Enter the PIN of the client:");
            string PINClient = Console.ReadLine();
            myCommand.Parameters.AddWithValue("@PIN", PINClient);

            Console.Write("Enter the Main Currency of the client:");
            string MainCurrency = Console.ReadLine();
            myCommand.Parameters.AddWithValue("@MainCurrency", MainCurrency);

            Console.Write("Do you want this account to be blocked ? Enter 1 if yes and enter 0 if no.");
            int isBlocked = Convert.ToInt32(Console.ReadLine());
            myCommand.Parameters.AddWithValue("@isBlocked", isBlocked);

            var result = myCommand.ExecuteNonQuery();

            databaseObject.CloseConnection();

            //check that the value has been correctly added :
            Console.WriteLine("Rows added: {0}", result);
        }

        public void BlockClient()
        {

        }

        public void UnblockClient()
        {

        }

        public void ChangePin()
        {

        }

        

        public void GetAll()
        {
            string query = "SELECT * FROM clients";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            databaseObject.OpenConnection();

            SQLiteDataReader result = myCommand.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    Console.WriteLine("GUID: {0} - First Name: {1} - Last Name: {2} - PIN: {3} - Main Currency: {4}", result["GUID"], result["FirstName"], result["LastName"], result["PIN"], result["MainCurrency"]);
                }
            }

            databaseObject.CloseConnection();
        }

        public void LogOutAdmin()
        {
            Console.Clear();
            MainMenu();
        }

        /* --- CLIENT --- */

        public void ClientMenu()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            DrawLine();
            Console.WriteLine("|{0}|", AlignText(0, ""));
            Console.WriteLine("|{0}|", AlignText(37, "1. Deposit Money"));
            Console.WriteLine("|{0}|", AlignText(37, "2. Withdraw Money"));
            Console.WriteLine("|{0}|", AlignText(37, "3. Tranfer Money"));
            Console.WriteLine("|{0}|", AlignText(37, "4. Show My Account Details"));
            Console.WriteLine("|{0}|", AlignText(37, "5. Logout"));
            Console.WriteLine("|{0}|", AlignText(0, ""));
            DrawLine();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("\n{0}", AlignText(38, "Enter your choice : ", "L"));
            int choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    MakeTransaction();
                    break;
                case 2:
                    MakeTransaction();
                    break;
                case 3:
                    MakeTransaction();
                    break;
                case 4:
                    AboutClient();
                    break;
                case 5:
                    LogOutClient();
                    break;
                default:
                    Console.WriteLine("Incorrect choice, please try again.");
                    break;
            }
        }

        private void LogInClient()
        {
            //check GUID + PIN
        }

        private void MakeTransaction()
        {
            string query = "INSERT INTO clients ('GUID', 'FirstName', 'LastName', 'PIN', 'MainCurrency', 'isBlocked') VALUES (@GUID, @FirstName, @LastName, @PIN, @MainCurrency, '@isBlocked')";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            databaseObject.OpenConnection();

            Console.Write("Enter the GUID of the client:");
            string GUIDClient = Console.ReadLine();
            myCommand.Parameters.AddWithValue("@GUID", GUIDClient);

            Console.Write("Enter the First Name of the client:");
            string FirstName = Console.ReadLine();
            myCommand.Parameters.AddWithValue("@FirstName", FirstName);

            Console.Write("Enter the Last Name of the client:");
            string LastName = Console.ReadLine();
            myCommand.Parameters.AddWithValue("@LastName", LastName);

            Console.Write("Enter the PIN of the client:");
            string PINClient = Console.ReadLine();
            myCommand.Parameters.AddWithValue("@PIN", PINClient);

            Console.Write("Enter the Main Currency of the client:");
            string MainCurrency = Console.ReadLine();
            myCommand.Parameters.AddWithValue("@MainCurrency", MainCurrency);

            Console.Write("Do you want this account to be blocked ? Enter 1 if yes and enter 0 if no.");
            int isBlocked = Convert.ToInt32(Console.ReadLine());
            myCommand.Parameters.AddWithValue("@isBlocked", isBlocked);

            var result = myCommand.ExecuteNonQuery();

            databaseObject.CloseConnection();

            //check that the value has been correctly added :
            Console.WriteLine("Rows added: {0}", result);
        }

        private void AboutClient()
        {

        }

        private void LogOutClient()
        {
            Console.Clear();
            MainMenu();
        }

        /* --- MAKE CONSOLE PRETTY --- */
        private string AlignText(int SpacesToAdd, string Msg, string Alignment = "R")
        {
            if (Alignment == "L")
                Msg = Msg.PadLeft(SpacesToAdd + Msg.Length);
            else
            {
                Msg = Msg.PadLeft(SpacesToAdd + Msg.Length);
                Msg = Msg.PadRight((98 - Msg.Length) + Msg.Length);
            }
            return Msg;
        }
        private void DrawLine()
        {
            Console.WriteLine("+--------------------------------------------------------------------------------------------------+");
        }
        private static void Center(string message)
        {
            int spaces = 50 + (message.Length / 2);
            Console.WriteLine(message.PadLeft(spaces));
        }

        private void About()
        {
            Console.Clear();
            Center("**** FQC ATM System | About Us ****\n");
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            DrawLine();
            Console.WriteLine("|{0}|", AlignText(0, ""));
            Console.WriteLine("|{0}|", AlignText(34, "Florent, Quentin and Chloé's ATM System"));
            Console.WriteLine("|{0}|", AlignText(35, "Developed for Intégration de systèmes : Fondamentaux"));
            Console.WriteLine("|{0}|", AlignText(0, ""));
            DrawLine();
            Console.BackgroundColor = ConsoleColor.Black;
        }


        static void Main(string[] args)
        {
            Program obj = new Program();
            obj.MainMenu();
        }
    }
}

