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
            Center("**** Welcome to FQC ATM System ****\n");
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            DrawLine();
            Console.WriteLine("|{0}|", AlignText(0, ""));
            Console.WriteLine("|{0}|", AlignText(35, "1. Log In as Admin"));
            Console.WriteLine("|{0}|", AlignText(35, "2. Log In as Client"));
            Console.WriteLine("|{0}|", AlignText(35, "3. About us"));
            Console.WriteLine("|{0}|", AlignText(35, "4. Exit"));
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
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            DrawLine();
            Console.WriteLine("|{0}|", AlignText(0, ""));
            Console.WriteLine("|{0}|", AlignText(37, "1. Create a client"));
            Console.WriteLine("|{0}|", AlignText(37, "2. Delete a client"));
            Console.WriteLine("|{0}|", AlignText(37, "3. Manage a client"));
            Console.WriteLine("|{0}|", AlignText(37, "4. Verify user transactions"));
            Console.WriteLine("|{0}|", AlignText(37, "5. View all clients"));
            Console.WriteLine("|{0}|", AlignText(37, "6. Logout"));
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
                        DeleteClient();
                        break;
                    case 3:
                        ManageClient();
                        break;
                    case 4:
                        VerifyTransactions();
                        break;
                    case 5:
                        GetAll();
                        break;
                    case 6:
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
                LogInAdmin();
            }
        }

        public void CreateClient()
        {
            string query = "INSERT INTO clients ('GUID', 'FirstName', 'LastName', 'PIN', 'MainCurrency', 'isBlocked', 'nbrTries', 'moneyAmount') VALUES (@GUID, @FirstName, @LastName, @PIN, @MainCurrency, @isBlocked, @nbrTries, @moneyAmount)";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            databaseObject.OpenConnection();

            //the GUID is generated randomly
            var byteArray = new Guid("a8828ddf-ef22-4d36-935a-1c66ae86ebb3").ToByteArray();
            string hex = BitConverter.ToString(byteArray).Replace("-", string.Empty);

            myCommand.Parameters.AddWithValue("@GUID", hex);

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

            //we assume that we don't block a brand new client from the start: all clients start off as not blocked
            //false: not blocked; true: blocked
            bool isBlocked = false;
            myCommand.Parameters.AddWithValue("@isBlocked", isBlocked);

            //we initialize the number of tries as 0
            int nbrTries = 0;
            myCommand.Parameters.AddWithValue("@nbrTries", nbrTries);

            //we initialize the amount of money to 0
            double moneyAmount = 0;
            myCommand.Parameters.AddWithValue("@moneyAmount", moneyAmount);

            var result = myCommand.ExecuteNonQuery();

            databaseObject.CloseConnection();

            //check that the value has been correctly added :
            Console.WriteLine("Rows added: {0}", result);

            AdminMenu();
        }

        public void DeleteClient()
        {
            databaseObject.OpenConnection();

            Console.Write("Enter the GUID of the client you wish to remove from the database:");
            string GUIDtodelete = Console.ReadLine();

            string query = "DELETE FROM clients WHERE GUID='" + GUIDtodelete + "'";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            var result = myCommand.ExecuteNonQuery();

            //check that the value has been correctly added :
            Console.WriteLine("Rows deleted: {0}", result);

            databaseObject.CloseConnection();
        }

        public void ManageClient()
        {
            Console.WriteLine("|{0}|", AlignText(0, ""));
            Console.WriteLine("|{0}|", AlignText(37, "Please choose what you want to do:"));
            Console.WriteLine("|{0}|", AlignText(37, "1. Reset tries for a client"));
            Console.WriteLine("|{0}|", AlignText(37, "2. Change PIN for a client"));
            Console.WriteLine("|{0}|", AlignText(37, "3. Add a currency for a client"));
            Console.WriteLine("|{0}|", AlignText(37, "4. Block a client"));
            Console.WriteLine("|{0}|", AlignText(37, "5. Unblock a client"));
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
                        ResetTries();
                        break;
                    case 2:
                        ChangePIN();
                        break;
                    case 3:
                        AddCurrency();
                        break;
                    case 4:
                        BlockClient();
                        break;
                    case 5:
                        UnblockClient();
                        break;
                    default:
                        Console.WriteLine("Incorrect choice, please try again.");
                        break;
                }
                Console.ReadKey();
                Console.Clear();
            }
        }

        public void ResetTries()
        {
            databaseObject.OpenConnection();

            Console.Write("Enter the GUID of the client you wish to rest the tries for: ");
            string GUIDtoResetTries = Console.ReadLine();

            string query = "UPDATE clients SET nbrTries=0 WHERE GUID='" + GUIDtoResetTries + "'";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            var result = myCommand.ExecuteNonQuery();

            //check that the value has been correctly added :
            Console.WriteLine("Rows updated: {0}", result);

            databaseObject.CloseConnection();
            AdminMenu();
        }

        public void ChangePIN()
        {
            databaseObject.OpenConnection();

            Console.Write("Enter the GUID of the client you wish to change the PIN of: ");
            string GUIDtoChangePIN = Console.ReadLine();
            Console.Write("Enter the new PIN: ");
            int newPIN = Convert.ToInt32(Console.ReadLine());

            string query = "UPDATE clients SET PIN='" + newPIN + "' WHERE GUID= '" + GUIDtoChangePIN + "'";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            var result = myCommand.ExecuteNonQuery();

            //check that the value has been correctly added :
            Console.WriteLine("Rows updated: {0}", result);

            databaseObject.CloseConnection();
            AdminMenu();
        }

        public void AddCurrency()
        {
            string query = "INSERT INTO currencies ('GUID', 'currency') VALUES (@GUID, @currency)";

            //string CheckGUID = "SELECT COUNT(*) FROM clients WHERE GUID='" + GUID + "'";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            databaseObject.OpenConnection();

            Console.WriteLine("Please enter the GUID of the client: ");
            string GUID = Console.ReadLine();
            myCommand.Parameters.AddWithValue("@GUID", GUID);

            Console.Write("Enter the currency: ");
            string currency = Console.ReadLine();
            myCommand.Parameters.AddWithValue("@currency", currency);


            var result = myCommand.ExecuteNonQuery();

            databaseObject.CloseConnection();

            //check that the value has been correctly added :
            Console.WriteLine("Rows added: {0}", result);
            AdminMenu();
        }

        public void VerifyTransactions()
        {
            //change value in the transactions database
            //if verified : bool true ; if not verified : bool false
            Console.WriteLine("This function isn't functional yet.");
        }

        public void BlockClient()
        {
            databaseObject.OpenConnection();

            Console.WriteLine("Please enter the GUID of the client you wish to block: ");
            string GUIDtoBlock = Console.ReadLine();

            string query = "UPDATE clients SET isBlocked=1 WHERE GUID= '" + GUIDtoBlock + "'";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            var result = myCommand.ExecuteNonQuery();

            //check that the value has been correctly added :
            Console.WriteLine("Rows updated: {0}", result);

            databaseObject.CloseConnection();
            AdminMenu();
        }

        public void UnblockClient()
        {
            databaseObject.OpenConnection();

            Console.WriteLine("Please enter the GUID of the client you wish to unblock: ");
            string GUIDtoUnblock = Console.ReadLine();

            string query = "UPDATE clients SET isBlocked=0 WHERE GUID= '" + GUIDtoUnblock + "'";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            var result = myCommand.ExecuteNonQuery();

            //check that the value has been correctly added :
            Console.WriteLine("Rows updated: {0}", result);

            databaseObject.CloseConnection();
            AdminMenu();
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
                    Console.WriteLine("GUID: {0} - First Name: {1} - Last Name: {2}", result["GUID"], result["FirstName"], result["LastName"]);
                }
            }

            databaseObject.CloseConnection();
            AdminMenu();
        }

        public void LogOutAdmin()
        {
            Console.Clear();
            MainMenu();
        }


        /* --- CLIENT --- */

        private void LogInClient()
        {
            databaseObject.OpenConnection();

            Console.Write("Please enter your GUID: ");
            string GUIDClient = Console.ReadLine();
            Console.Write("Please enter your PIN: ");
            string PINClient = Console.ReadLine();

            string queryLogin = "SELECT GUID FROM clients WHERE GUID='" + GUIDClient + "' AND PIN='" + PINClient + "'";
            string queryUpdateTries = "UPDATE clients SET nbrTries=nbrTries+1 WHERE GUID='" + GUIDClient + "'";
            string queryGetAmountTries = "SELECT nbrTries FROM clients WHERE GUID='" + GUIDClient + "'";
            string queryBlocked = "SELECT isBlocked FROM clients WHERE GUID='" + GUIDClient + "'";

            SQLiteCommand myCommandLogin = new SQLiteCommand(queryLogin, databaseObject.myConnection);
            SQLiteCommand myCommandUpdateTries = new SQLiteCommand(queryUpdateTries, databaseObject.myConnection);
            SQLiteCommand myCommandGetAmountTries = new SQLiteCommand(queryGetAmountTries, databaseObject.myConnection);
            SQLiteCommand myCommandBlocked = new SQLiteCommand(queryBlocked, databaseObject.myConnection);

            SQLiteDataReader result = myCommandLogin.ExecuteReader();
            var resultTries = myCommandGetAmountTries.ExecuteNonQuery();
            SQLiteDataReader resultBlocked = myCommandBlocked.ExecuteReader();
            int nbrTries = CheckIfThreeTries(GUIDClient);

            //check if not blocked: blocked = by bank advisor or if more than 3 tries
            if (resultBlocked.HasRows)
            {
                while (resultBlocked.Read())
                {
                    //Console.WriteLine("{0}", resultBlocked["isBlocked"]);
                    int isBlocked = resultBlocked.GetInt32(0);
                    if (isBlocked == 0 && nbrTries<3)
                    {
                        if (result.HasRows)
                        {
                            ClientMenu(GUIDClient);
                        }
                        else
                        {
                            Console.WriteLine("Wrong input. Please try again.");
                            SQLiteDataReader updateTries = myCommandUpdateTries.ExecuteReader();
                            resultTries++;
                            LogInClient();
                        }
                    }
                    else
                    {
                        Console.WriteLine("You are blocked. Please contact your bank advisor.");
                    }
                }
            }
            databaseObject.CloseConnection();
        }

        public int CheckIfThreeTries(string GUIDClient)
        {
            databaseObject.OpenConnection();

            string queryGetAmountTries = "SELECT nbrTries FROM clients WHERE GUID='" + GUIDClient + "'";

            SQLiteCommand myCommandGetAmountTries = new SQLiteCommand(queryGetAmountTries, databaseObject.myConnection);

            SQLiteDataReader resultNbrTries = myCommandGetAmountTries.ExecuteReader();

            while (resultNbrTries.Read())
            {
                Console.WriteLine("Number of tries: {0}", resultNbrTries["nbrTries"]);
                int nbrTries = resultNbrTries.GetInt32(0);
                Console.WriteLine("Number of tries returned: {0}", nbrTries);
                return nbrTries;
            }

            databaseObject.CloseConnection();

            return 0;
        }

        public void ClientMenu(string GUIDClient)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            DrawLine();
            Console.WriteLine("|{0}|", AlignText(0, ""));
            Console.WriteLine("|{0}|", AlignText(37, "1. Deposit Money"));
            Console.WriteLine("|{0}|", AlignText(37, "2. Withdraw Money"));
            Console.WriteLine("|{0}|", AlignText(37, "3. View total amount"));
            Console.WriteLine("|{0}|", AlignText(37, "4. Change PIN"));
            Console.WriteLine("|{0}|", AlignText(37, "5. Change currency"));
            Console.WriteLine("|{0}|", AlignText(37, "6. Add a currency to the list"));
            Console.WriteLine("|{0}|", AlignText(37, "7. Show My Account Details"));
            Console.WriteLine("|{0}|", AlignText(37, "8. Logout"));
            Console.WriteLine("|{0}|", AlignText(0, ""));
            DrawLine();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("\n{0}", AlignText(38, "Enter your choice : ", "L"));
            int choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    DepositMoney(GUIDClient);
                    break;
                case 2:
                    WithdrawMoney(GUIDClient);
                    break;
                case 3:
                    ViewTotalMoney(GUIDClient);
                    break;
                case 4:
                    ChangePINClient(GUIDClient);
                    break;
                case 5:
                    ChangeCurrency(GUIDClient);
                    break;
                case 6:
                    AddCurrencyClient(GUIDClient);
                    break;
                case 7:
                    AboutClient(GUIDClient);
                    break;
                case 8:
                    LogOutClient();
                    break;
                default:
                    Console.WriteLine("Incorrect choice, please try again.");
                    break;
            }
        }

        private void DepositMoney(string GUIDClient)
        {
            databaseObject.OpenConnection();

            Console.WriteLine("Please enter the amount of money you would like to deposit: ");
            int amountToDeposit = Convert.ToInt32(Console.ReadLine());

            string query = "UPDATE clients SET moneyAmount=moneyAmount+'" + amountToDeposit + "' WHERE GUID= '" + GUIDClient + "'";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            var result = myCommand.ExecuteNonQuery();

            //check that the value has been correctly added :
            Console.WriteLine("Rows updated: {0}", result);

            databaseObject.CloseConnection();
            ClientMenu(GUIDClient);
        }

        private void WithdrawMoney(string GUIDClient)
        {
            databaseObject.OpenConnection();

            Console.WriteLine("Please enter the amount of money you would like to withdraw: ");
            double amountToWithdraw = double.Parse(Console.ReadLine());

            string query = "UPDATE clients SET moneyAmount=moneyAmount-'" + amountToWithdraw + "' WHERE GUID= '" + GUIDClient + "'";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            var result = myCommand.ExecuteNonQuery();

            //check that the value has been correctly added :
            Console.WriteLine("Rows updated: {0}", result);

            databaseObject.CloseConnection();
            ClientMenu(GUIDClient);
        }

        private void ViewTotalMoney(string GUIDClient)
        {
            databaseObject.OpenConnection();

            string query = "SELECT MainCurrency, moneyAmount FROM clients WHERE GUID='" + GUIDClient + "'";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            SQLiteDataReader result = myCommand.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    Console.WriteLine("The total amount in your account is: {0} {1}", result["moneyAmount"], result["MainCurrency"]);
                }
            }

            databaseObject.CloseConnection();
            ClientMenu(GUIDClient);
        }

        private void ChangePINClient(string GUIDClient)
        {
            databaseObject.OpenConnection();

            Console.WriteLine("Please enter the new PIN: ");
            int newPIN = Convert.ToInt32(Console.ReadLine());

            string query = "UPDATE clients SET PIN='" + newPIN + "' WHERE GUID= '" + GUIDClient + "'";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            var result = myCommand.ExecuteNonQuery();

            //check that the value has been correctly added :
            Console.WriteLine("Rows updated: {0}", result);

            databaseObject.CloseConnection();
            ClientMenu(GUIDClient);
        }

        private void AddCurrencyClient(string GUIDClient)
        {
            string query = "INSERT INTO currencies ('GUID', 'currency') VALUES (@GUID, @currency)";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            databaseObject.OpenConnection();

            myCommand.Parameters.AddWithValue("@GUID", GUIDClient);

            Console.Write("Enter the currency to add to the list: ");
            string currency = Console.ReadLine();
            myCommand.Parameters.AddWithValue("@currency", currency);

            var result = myCommand.ExecuteNonQuery();

            databaseObject.CloseConnection();

            //check that the value has been correctly added :
            Console.WriteLine("Rows added: {0}", result);
        }


        private void ChangeCurrency(string GUIDClient)
        {
            databaseObject.OpenConnection();

            string queryCurrencies = "SELECT currency FROM currencies WHERE GUID= '" + GUIDClient + "'";

            SQLiteCommand myCommandCurrencies = new SQLiteCommand(queryCurrencies, databaseObject.myConnection);

            SQLiteDataReader resultCurrencies = myCommandCurrencies.ExecuteReader();

            Console.WriteLine("Please enter your new preferred currency from the following list: ");
            if (resultCurrencies.HasRows)
            {
                Console.WriteLine("List of currencies: ");
                while (resultCurrencies.Read())
                {
                    Console.WriteLine("{0}", resultCurrencies["currency"]);
                }
            }
            string newMainCurrency = Console.ReadLine();

            string query = "UPDATE clients SET MainCurrency='" + newMainCurrency + "' WHERE GUID= '" + GUIDClient + "'";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            var result = myCommand.ExecuteNonQuery();

            databaseObject.CloseConnection();

            //check that the value has been correctly added :
            Console.WriteLine("Rows added: {0}", result);

        }

        private void AboutClient(string GUIDClient)
        {
            string query = "SELECT GUID, MainCurrency FROM clients WHERE GUID= '" + GUIDClient + "'";
            string queryCurrencies = "SELECT currency FROM currencies WHERE GUID= '" + GUIDClient + "'";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);
            SQLiteCommand myCommandCurrencies = new SQLiteCommand(queryCurrencies, databaseObject.myConnection);

            databaseObject.OpenConnection();

            SQLiteDataReader result = myCommand.ExecuteReader();
            SQLiteDataReader resultCurrencies = myCommandCurrencies.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    Console.WriteLine("GUID: {0} - Main Currency: {1}", result["GUID"], result["MainCurrency"]);

                    if (resultCurrencies.HasRows)
                    {
                        Console.WriteLine("List of currencies: ");
                        while (resultCurrencies.Read())
                        {
                            Console.WriteLine("{0}", resultCurrencies["currency"]);
                        }
                    }
                }
            }

            databaseObject.CloseConnection();
            ClientMenu(GUIDClient);
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
            Console.WriteLine("|{0}|", AlignText(35, "Teacher: Mikael CHAAYA"));
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