using System;
using System.Data.SQLite;
using Api;

namespace ATM
{
    class Program
    {
        Database databaseObject = new Database();

        public Program()
        {
            Console.Title = "FQC's ATM system";
        }

        private void MainMenu()
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
        private void AdminMenu()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            DrawLine();
            Console.WriteLine("|{0}|", AlignText(0, ""));
            Console.WriteLine("|{0}|", AlignText(37, "1. Create a client"));
            Console.WriteLine("|{0}|", AlignText(37, "2. Delete a client"));
            Console.WriteLine("|{0}|", AlignText(37, "3. Manage a client"));
            Console.WriteLine("|{0}|", AlignText(37, "4. Verify user transactions"));
            Console.WriteLine("|{0}|", AlignText(37, "5. View all clients"));
            Console.WriteLine("|{0}|", AlignText(37, "6. View messages from the clients"));
            Console.WriteLine("|{0}|", AlignText(37, "7. Logout"));
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
                        ViewMessages();
                        break;
                    case 7:
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

        private void LogInAdmin()
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

        private Random randomPIN = new Random();

        private void CreateClient()
        {
            string query = "INSERT INTO clients ('GUID', 'FirstName', 'LastName', 'PIN', 'MainCurrency', 'isBlocked', 'nbrTries', 'moneyAmount') VALUES (@GUID, @FirstName, @LastName, @PIN, @MainCurrency, @isBlocked, @nbrTries, @moneyAmount)";
            string queryAddCurrency = "INSERT INTO clientsCurrencies ('GUID', 'currency') VALUES (@GUID, @currency)";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);
            SQLiteCommand myCommandAddCurrency = new SQLiteCommand(queryAddCurrency, databaseObject.myConnection);

            databaseObject.OpenConnection();

            //the GUID is generated randomly
            var GUID = Guid.NewGuid();
            string GUIDClient = GUID.ToString();
            myCommand.Parameters.AddWithValue("@GUID", GUIDClient);
            myCommandAddCurrency.Parameters.AddWithValue("@GUID", GUIDClient);

            Console.Write("Enter the First Name of the client: ");
            string FirstName = Console.ReadLine();
            myCommand.Parameters.AddWithValue("@FirstName", FirstName);

            Console.Write("Enter the Last Name of the client: ");
            string LastName = Console.ReadLine();
            myCommand.Parameters.AddWithValue("@LastName", LastName);

            string PINClient =  randomPIN.Next(0, 9999).ToString("D4");
            myCommand.Parameters.AddWithValue("@PIN", PINClient);

            Console.Write("Enter the Main Currency of the client: ");
            string MainCurrency = Console.ReadLine();
            myCommand.Parameters.AddWithValue("@MainCurrency", MainCurrency);
            myCommandAddCurrency.Parameters.AddWithValue("@currency", MainCurrency);

            //we assume that we don't block a brand new client from the start: all clients start off as not blocked
            //0: not blocked; 1: blocked
            bool isBlocked = false;
            myCommand.Parameters.AddWithValue("@isBlocked", isBlocked);

            //we initialize the number of tries as 0
            int nbrTries = 0;
            myCommand.Parameters.AddWithValue("@nbrTries", nbrTries);

            //we initialize the amount of money to 0
            double moneyAmount = 0;
            myCommand.Parameters.AddWithValue("@moneyAmount", moneyAmount);

            var result = myCommand.ExecuteNonQuery();
            var resultAddCurrency = myCommandAddCurrency.ExecuteNonQuery();

            databaseObject.CloseConnection();

            //check that the value has been correctly added :
            Console.WriteLine("Rows added in clients: {0}", result);
            Console.WriteLine("Rows added in clientsCurrencies: {0}", resultAddCurrency);

            AdminMenu();
        }

        private void DeleteClient()
        {
            databaseObject.OpenConnection();

            Console.Write("Enter the GUID of the client you wish to remove from the database:");
            string GUIDtodelete = Console.ReadLine();

            string queryClient = "DELETE FROM clients WHERE GUID='" + GUIDtodelete + "'";
            string queryCurrencies = "DELETE FROM clientsCurrencies WHERE GUID='" + GUIDtodelete + "'";

            SQLiteCommand myCommandClient = new SQLiteCommand(queryClient, databaseObject.myConnection);
            SQLiteCommand myCommandCurrencies = new SQLiteCommand(queryCurrencies, databaseObject.myConnection);

            var resultClient = myCommandClient.ExecuteNonQuery();
            var resultCurrencies = myCommandCurrencies.ExecuteNonQuery();

            //check that the value has been correctly deleted :
            Console.WriteLine("Rows deleted in clients: {0}", resultClient);
            Console.WriteLine("Rows deleted in currencies: {0}", resultCurrencies);

            databaseObject.CloseConnection();
            AdminMenu();
        }

        private void ManageClient()
        {
            Console.WriteLine("|{0}|", AlignText(0, ""));
            Console.WriteLine("|{0}|", AlignText(37, "Please choose what you want to do:"));
            Console.WriteLine("|{0}|", AlignText(37, "1. Reset tries for a client"));
            Console.WriteLine("|{0}|", AlignText(37, "2. Change PIN for a client"));
            Console.WriteLine("|{0}|", AlignText(37, "3. Add a currency for a client"));
            Console.WriteLine("|{0}|", AlignText(37, "4. Block a client"));
            Console.WriteLine("|{0}|", AlignText(37, "5. Unblock a client"));
            Console.WriteLine("|{0}|", AlignText(37, "6.Back to menu"));
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
                    case 6:
                        AdminMenu();
                        break;
                    default:
                        Console.WriteLine("Incorrect choice, please try again.");
                        break;
                }
                Console.ReadKey();
                Console.Clear();
            }
        }

        private void ResetTries()
        {
            databaseObject.OpenConnection();

            Console.Write("Enter the GUID of the client you wish to rest the tries for: ");
            string GUIDtoResetTries = Console.ReadLine();

            string query = "UPDATE clients SET nbrTries=0 WHERE GUID='" + GUIDtoResetTries + "'";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            var result = myCommand.ExecuteNonQuery();

            //check that the value has been correctly updated :
            Console.WriteLine("Rows updated in clients: {0}", result);

            databaseObject.CloseConnection();
            AdminMenu();
        }

        private void ChangePIN()
        {
            databaseObject.OpenConnection();

            Console.Write("Enter the GUID of the client you wish to change the PIN of: ");
            string GUIDtoChangePIN = Console.ReadLine();
            string newPIN = randomPIN.Next(0, 9999).ToString("D4");

            string query = "UPDATE clients SET PIN='" + newPIN + "' WHERE GUID= '" + GUIDtoChangePIN + "'";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            var result = myCommand.ExecuteNonQuery();

            //check that the value has been correctly added :
            Console.WriteLine("Rows updated in clients: {0}", result);

            //return new PIN
            string queryGetNewPin = "SELECT PIN FROM clients WHERE GUID= '" + GUIDtoChangePIN + "'";
            SQLiteCommand myCommandGetNewPin = new SQLiteCommand(queryGetNewPin, databaseObject.myConnection);

            SQLiteDataReader resultGetNewPin = myCommandGetNewPin.ExecuteReader();

            if (resultGetNewPin.HasRows)
            {
                while (resultGetNewPin.Read())
                {
                    Console.WriteLine("New PIN: {0}", resultGetNewPin["PIN"]);
                }
            }

            databaseObject.CloseConnection();
            AdminMenu();
        }

        private void AddCurrency()
        {
            string query = "INSERT INTO clientsCurrencies ('GUID', 'currency') VALUES (@GUID, @currency)";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            databaseObject.OpenConnection();

            Console.Write("Please enter the GUID of the client: ");
            string GUID = Console.ReadLine();
            myCommand.Parameters.AddWithValue("@GUID", GUID);

            Console.Write("Enter the currency to add to the list of currencies by writing the currency code: ");
            string currency = Console.ReadLine();
            myCommand.Parameters.AddWithValue("@currency", currency);


            var result = myCommand.ExecuteNonQuery();

            databaseObject.CloseConnection();

            //check that the value has been correctly added :
            Console.WriteLine("Rows added in clientsCurrencies: {0}", result);
            AdminMenu();
        }

        private void VerifyTransactions()
        {
            string query = "SELECT * FROM transactions WHERE isVerified=0";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            databaseObject.OpenConnection();

            SQLiteDataReader result = myCommand.ExecuteReader();

            Console.WriteLine("List of the transactions yet to be verified: ");
            if (result.HasRows)
            {
                while (result.Read())
                {
                    Console.WriteLine("Id of the transaction: {0} - Client's GUID: {1} - Type: {2} - Amount: {3} {4} - Date: {5} ", result["id"], result["GUID"], result["type"], result["amount"], result["currency"], result["date"]);
                }
            }

            Console.Write("Please enter the ID of the transaction you would like to verify: ");
            int id = Convert.ToInt32(Console.ReadLine());

            string queryVerify = "UPDATE transactions SET isVerified=1 WHERE id='" + id +"'";
            SQLiteCommand myCommandVerify = new SQLiteCommand(queryVerify, databaseObject.myConnection);
            var resultVerify = myCommandVerify.ExecuteNonQuery();

            //check that the value has been correctly updated :
            Console.WriteLine("Rows updated in transactions: {0}", resultVerify);

            databaseObject.CloseConnection();
            AdminMenu();
        }

        private void BlockClient()
        {
            databaseObject.OpenConnection();

            Console.Write("Please enter the GUID of the client you wish to block: ");
            string GUIDtoBlock = Console.ReadLine();

            string query = "UPDATE clients SET isBlocked=1 WHERE GUID= '" + GUIDtoBlock + "'";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            var result = myCommand.ExecuteNonQuery();

            //check that the value has been correctly updated :
            Console.WriteLine("Rows updated in clients: {0}", result);

            databaseObject.CloseConnection();
            AdminMenu();
        }

        private void UnblockClient()
        {
            databaseObject.OpenConnection();

            Console.WriteLine("Please enter the GUID of the client you wish to unblock: ");
            string GUIDtoUnblock = Console.ReadLine();

            string query = "UPDATE clients SET isBlocked=0 WHERE GUID= '" + GUIDtoUnblock + "'";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            var result = myCommand.ExecuteNonQuery();

            //check that the value has been correctly updated :
            Console.WriteLine("Rows updated in clients: {0}", result);

            databaseObject.CloseConnection();
            AdminMenu();
        }

        private void GetAll()
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
            AdminMenu();
        }

        private void ViewMessages()
        {
            Console.WriteLine("|{0}|", AlignText(0, ""));
            Console.WriteLine("|{0}|", AlignText(37, "Please choose what you want to do:"));
            Console.WriteLine("|{0}|", AlignText(37, "1. See all messages"));
            Console.WriteLine("|{0}|", AlignText(37, "2. See unread messages"));
            Console.WriteLine("|{0}|", AlignText(37, "3. Set a message to read"));
            Console.WriteLine("|{0}|", AlignText(37, "4. Back to menu"));
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
                        string queryAll = "SELECT * FROM messages";

                        SQLiteCommand myCommandAll = new SQLiteCommand(queryAll, databaseObject.myConnection);

                        databaseObject.OpenConnection();

                        SQLiteDataReader resultAll = myCommandAll.ExecuteReader();

                        if (resultAll.HasRows)
                        {
                            while (resultAll.Read())
                            {
                                Console.WriteLine("Id message: {0} - Client's GUID: {1} - Contenu: {2} - Read yet: {3}", resultAll["id"], resultAll["GUIDClient"], resultAll["contenu"], resultAll["wasRead"]);
                            }
                        }

                        databaseObject.CloseConnection();
                        ViewMessages();
                        break;
                    case 2:
                        string queryUnread = "SELECT * FROM messages WHERE wasRead=0";

                        SQLiteCommand myCommandUnread = new SQLiteCommand(queryUnread, databaseObject.myConnection);

                        databaseObject.OpenConnection();

                        SQLiteDataReader resultUnread = myCommandUnread.ExecuteReader();

                        if (resultUnread.HasRows)
                        {
                            while (resultUnread.Read())
                            {
                                Console.WriteLine("Id message: {0} - Client's GUID: {1} - Contenu: {2}", resultUnread["id"], resultUnread["GUIDClient"], resultUnread["contenu"]);
                            }
                        }

                        databaseObject.CloseConnection();
                        ViewMessages();
                        break;
                    case 3:
                        Console.WriteLine("Please enter the id of the message you wish to set as read: ");
                        int idSetRead = Convert.ToInt32(Console.ReadLine());
                        string queryMarkRead = "UPDATE messages SET wasRead=1 WHERE id= '" + idSetRead + "'";

                        SQLiteCommand myCommandMarkRead = new SQLiteCommand(queryMarkRead, databaseObject.myConnection);

                        databaseObject.OpenConnection();

                        SQLiteDataReader resultMarkRead = myCommandMarkRead.ExecuteReader();

                        databaseObject.CloseConnection();
                        ViewMessages();
                        break;
                    case 4:
                        AdminMenu();
                        break;
                    default:
                        Console.WriteLine("Incorrect choice, please try again.");
                        break;
                }
                Console.ReadKey();
                Console.Clear();
            }
        }

        private void LogOutAdmin()
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
            string queryGetAmountTries = "SELECT nbrTries FROM clients WHERE GUID='" + GUIDClient + "'";
            string queryUpdateTries = "UPDATE clients SET nbrTries=nbrTries+1 WHERE GUID='" + GUIDClient + "'";
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
                    int isBlocked = resultBlocked.GetInt32(0);
                    if (isBlocked == 0 && nbrTries<4)
                    {
                        if (result.HasRows)
                        {
                            ClientMenu(GUIDClient);
                            ResetTriesClient(GUIDClient);
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

        private int CheckIfThreeTries(string GUIDClient)
        {
            databaseObject.OpenConnection();

            string queryGetAmountTries = "SELECT nbrTries FROM clients WHERE GUID='" + GUIDClient + "'";

            SQLiteCommand myCommandGetAmountTries = new SQLiteCommand(queryGetAmountTries, databaseObject.myConnection);

            SQLiteDataReader resultNbrTries = myCommandGetAmountTries.ExecuteReader();

            while (resultNbrTries.Read())
            {
                int nbrTries = resultNbrTries.GetInt32(0);
                return nbrTries;
            }

            databaseObject.CloseConnection();
            return 0;
        }

        private void ResetTriesClient(string GUIDClient)
        {
            databaseObject.OpenConnection();

            string query = "UPDATE clients SET nbrTries=0 WHERE GUID='" + GUIDClient + "'";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            var result = myCommand.ExecuteNonQuery();

            databaseObject.CloseConnection();
            AdminMenu();
        }

        private void ClientMenu(string GUIDClient)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            DrawLine();
            Console.WriteLine("|{0}|", AlignText(0, ""));
            Console.WriteLine("|{0}|", AlignText(37, " 1. Deposit Money"));
            Console.WriteLine("|{0}|", AlignText(37, " 2. Withdraw Money"));
            Console.WriteLine("|{0}|", AlignText(37, " 3. Send Money to another account"));
            Console.WriteLine("|{0}|", AlignText(37, " 4. View total amount"));
            Console.WriteLine("|{0}|", AlignText(37, " 5. Change PIN"));
            Console.WriteLine("|{0}|", AlignText(37, " 6. Change currency"));
            Console.WriteLine("|{0}|", AlignText(37, " 7. Add a currency to the list"));
            Console.WriteLine("|{0}|", AlignText(37, " 8. Show My Account Details"));
            Console.WriteLine("|{0}|", AlignText(37, " 9. Send a message to the admin"));
            Console.WriteLine("|{0}|", AlignText(37, "10. Logout"));
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
                    SendMoney(GUIDClient);
                    break;
                case 4:
                    ViewTotalMoney(GUIDClient);
                    break;
                case 5:
                    ChangePINClient(GUIDClient);
                    break;
                case 6:
                    ChangeCurrency(GUIDClient);
                    break;
                case 7:
                    AddCurrencyClient(GUIDClient);
                    break;
                case 8:
                    AboutClient(GUIDClient);
                    break;
                case 9:
                    SendMessage(GUIDClient);
                    break;
                case 10:
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
            string queryAddTransactions = "INSERT INTO transactions ('type', 'date', 'GUID', 'amount', 'currency', 'isVerified') VALUES (@type, @date, @GUID, @amount, @currency, @isVerified)";
            string queryGetCurrency = "SELECT MainCurrency FROM clients WHERE GUID='" + GUIDClient + "'";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);
            SQLiteCommand myCommandAddTransactions = new SQLiteCommand(queryAddTransactions, databaseObject.myConnection);
            SQLiteCommand myCommandGetCurrency = new SQLiteCommand(queryGetCurrency, databaseObject.myConnection);

            //add the transaction to the listing

            string type = "Deposit";
            myCommandAddTransactions.Parameters.AddWithValue("@type", type);

            DateTime date = DateTime.Now;
            myCommandAddTransactions.Parameters.AddWithValue("@date", date);

            myCommandAddTransactions.Parameters.AddWithValue("@GUID", GUIDClient);

            myCommandAddTransactions.Parameters.AddWithValue("@amount", amountToDeposit);

            SQLiteDataReader resultGetCurrency = myCommandGetCurrency.ExecuteReader();
            if (resultGetCurrency.HasRows)
            {
                while (resultGetCurrency.Read())
                {
                    string currency = resultGetCurrency.GetString(0);
                    myCommandAddTransactions.Parameters.AddWithValue("@currency", currency);
                }
            }

            int isVerified = 0;
            myCommandAddTransactions.Parameters.AddWithValue("@isVerified", isVerified);

            var result = myCommand.ExecuteNonQuery();
            var resultAddTransaction = myCommandAddTransactions.ExecuteNonQuery();

            //check that the value has been correctly added :
            Console.WriteLine("Rows updated in clients: {0}", result);
            Console.WriteLine("Rows added in transactions: {0}", resultAddTransaction);

            databaseObject.CloseConnection();

            ClientMenu(GUIDClient);
        }

        private void WithdrawMoney(string GUIDClient)
        {
            databaseObject.OpenConnection();

            Console.WriteLine("Please enter the amount of money you would like to deposit: ");
            int amountToWithdraw = Convert.ToInt32(Console.ReadLine());

            string queryGetAmount = "SELECT moneyAmount FROM clients WHERE GUID='" + GUIDClient + "'";
            string query = "UPDATE clients SET moneyAmount=moneyAmount-'" + amountToWithdraw + "' WHERE GUID= '" + GUIDClient + "'";
            string queryAddTransactions = "INSERT INTO transactions ('type', 'date', 'GUID', 'amount', 'currency', 'isVerified') VALUES (@type, @date, @GUID, @amount, @currency, @isVerified)";
            string queryGetCurrency = "SELECT MainCurrency FROM clients WHERE GUID='" + GUIDClient + "'";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);
            SQLiteCommand myCommandAddTransactions = new SQLiteCommand(queryAddTransactions, databaseObject.myConnection);
            SQLiteCommand myCommandGetCurrency = new SQLiteCommand(queryGetCurrency, databaseObject.myConnection);
            SQLiteCommand myCommandGetAmount = new SQLiteCommand(queryGetAmount, databaseObject.myConnection);


            SQLiteDataReader resultMoneyAmount = myCommandGetAmount.ExecuteReader();

            if (resultMoneyAmount.HasRows)
            {
                while (resultMoneyAmount.Read())
                {
                    int amountMoney = resultMoneyAmount.GetInt32(0);
                    if (amountMoney > amountToWithdraw)
                    {
                        //add the transaction to the listing
                        string type = "Withdraw";
                        myCommandAddTransactions.Parameters.AddWithValue("@type", type);

                        DateTime date = DateTime.Now;
                        myCommandAddTransactions.Parameters.AddWithValue("@date", date);

                        myCommandAddTransactions.Parameters.AddWithValue("@GUID", GUIDClient);

                        myCommandAddTransactions.Parameters.AddWithValue("@amount", amountToWithdraw);

                        SQLiteDataReader resultGetCurrency = myCommandGetCurrency.ExecuteReader();
                        if (resultGetCurrency.HasRows)
                        {
                            while (resultGetCurrency.Read())
                            {
                                string currency = resultGetCurrency.GetString(0);
                                myCommandAddTransactions.Parameters.AddWithValue("@currency", currency);
                            }
                        }

                        int isVerified = 0;
                        myCommandAddTransactions.Parameters.AddWithValue("@isVerified", isVerified);

                        var result = myCommand.ExecuteNonQuery();
                        var resultAddTransaction = myCommandAddTransactions.ExecuteNonQuery();

                        //check that the value has been correctly added :
                        Console.WriteLine("Rows updated in clients: {0}", result);
                        Console.WriteLine("Rows added in transactions: {0}", resultAddTransaction);
                    }
                    else
                    {
                        Console.WriteLine("Please try again later.");
                    }
                }
            }

            databaseObject.CloseConnection();
            ClientMenu(GUIDClient);
        }

        private void SendMoney(string GUIDClient)
        {
            databaseObject.OpenConnection();

            Console.Write("Please enter the GUID of the client you wish to send money to: ");
            string GUIDReceiver = Console.ReadLine();

            Console.Write("Please enter the amount of money you would like to send: ");
            int amountToSend = Convert.ToInt32(Console.ReadLine());

            string querySendMoney = "UPDATE clients SET moneyAmount=moneyAmount+'" + amountToSend + "' WHERE GUID= '" + GUIDReceiver + "'";
            string queryRemoveMoney = "UPDATE clients SET moneyAmount=moneyAmount-'" + amountToSend + "' WHERE GUID= '" + GUIDClient + "'";
            string queryCheckMainCurrencySender = "SELECT MainCurrency FROM clients WHERE GUID= '" + GUIDClient + "'";
            string queryCheckMainCurrencyReceiver = "SELECT MainCurrency FROM clients WHERE GUID= '" + GUIDReceiver + "'";
            string queryCheckMoneyAmount = "SELECT moneyAmount FROM clients WHERE GUID= '" + GUIDClient + "'";
            string queryAddTransactions = "INSERT INTO transactions ('type', 'date', 'GUID', 'amount', 'currency', 'isVerified') VALUES (@type, @date, @GUID, @amount, @currency, @isVerified)";

            //check main currencies and check that they match
            SQLiteCommand myCommandMainCurrencySender = new SQLiteCommand(queryCheckMainCurrencySender, databaseObject.myConnection);
            SQLiteDataReader resultMainCurrencySender = myCommandMainCurrencySender.ExecuteReader();

            SQLiteCommand myCommandMainCurrencyReceiver = new SQLiteCommand(queryCheckMainCurrencyReceiver, databaseObject.myConnection);
            SQLiteDataReader resultMainCurrencyReceiver = myCommandMainCurrencyReceiver.ExecuteReader();

            if (resultMainCurrencySender.HasRows)
            {
                if (resultMainCurrencyReceiver.HasRows)
                {
                    while (resultMainCurrencySender.Read())
                    {
                        while (resultMainCurrencyReceiver.Read())
                        {
                            string MainCurrencySender = resultMainCurrencySender.GetString(0);
                            string MainCurrencyReceiver = resultMainCurrencyReceiver.GetString(0);

                            //check that the main currencies match
                            if (String.Equals(MainCurrencySender, MainCurrencyReceiver))
                            {
                                SQLiteCommand myCommandCheckMoneyAmount = new SQLiteCommand(queryCheckMoneyAmount, databaseObject.myConnection);
                                SQLiteDataReader resultCheckMoneyAmount = myCommandCheckMoneyAmount.ExecuteReader();

                                //check that the send has enough money to send the desired amount
                                if (resultCheckMoneyAmount.HasRows)
                                {
                                    while (resultCheckMoneyAmount.Read())
                                    {
                                        int amountMoney = resultCheckMoneyAmount.GetInt32(0);
                                        if (amountMoney > amountToSend)
                                        {
                                            //send money from the sender to the receiver'account
                                            SQLiteCommand myCommandSendMoney = new SQLiteCommand(querySendMoney, databaseObject.myConnection);
                                            var resultSendMoney = myCommandSendMoney.ExecuteNonQuery();

                                            //check that the value has been correctly update
                                            Console.WriteLine("The money has been sent to the receiver: {0}", resultSendMoney);

                                            //remove money from the sender's account
                                            SQLiteCommand myCommandRemoveMoney = new SQLiteCommand(queryRemoveMoney, databaseObject.myConnection);
                                            var resultRemoveMoney = myCommandRemoveMoney.ExecuteNonQuery();

                                            //check that the value has been correctly update
                                            Console.WriteLine("The money has been removed from the sender's account: {0}", resultRemoveMoney);


                                            //add the transaction to the transaction table
                                            SQLiteCommand myCommandAddTransactions = new SQLiteCommand(queryAddTransactions, databaseObject.myConnection);

                                            //add the transaction to the listing

                                            string type = "Transfer";
                                            myCommandAddTransactions.Parameters.AddWithValue("@type", type);

                                            DateTime date = DateTime.Now;
                                            myCommandAddTransactions.Parameters.AddWithValue("@date", date);

                                            myCommandAddTransactions.Parameters.AddWithValue("@GUID", GUIDClient);

                                            myCommandAddTransactions.Parameters.AddWithValue("@amount", amountToSend);

                                            myCommandAddTransactions.Parameters.AddWithValue("@currency", MainCurrencySender);
                                     
                                            int isVerified = 0;
                                            myCommandAddTransactions.Parameters.AddWithValue("@isVerified", isVerified);

                                            var resultAddTransaction = myCommandAddTransactions.ExecuteNonQuery();

                                            //check that the value has been correctly added :
                                            Console.WriteLine("Rows added in transactions: {0}", resultAddTransaction);
                                        }
                                        else
                                        {
                                            Console.WriteLine("Sorry, you don't have the funds available.");
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Sorry, you don't have the data available.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Sorry, you must have the same main currency.");
                            }
                        }
                    }
                }
            }

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

            string newPIN = randomPIN.Next(0, 9999).ToString("D4");

            string query = "UPDATE clients SET PIN='" + newPIN + "' WHERE GUID= '" + GUIDClient + "'";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            var result = myCommand.ExecuteNonQuery();

            //check that the value has been correctly added :
            Console.WriteLine("Rows updated in clients: {0}", result);

            //return new PIN
            string queryGetNewPin = "SELECT PIN FROM clients WHERE GUID= '" + GUIDClient + "'";
            SQLiteCommand myCommandGetNewPin = new SQLiteCommand(queryGetNewPin, databaseObject.myConnection);

            SQLiteDataReader resultGetNewPin = myCommandGetNewPin.ExecuteReader();

            if (resultGetNewPin.HasRows)
            {
                while (resultGetNewPin.Read())
                {
                    Console.WriteLine("New PIN: {0}", resultGetNewPin["PIN"]);
                }
            }

            databaseObject.CloseConnection();
            ClientMenu(GUIDClient);
        }

        private void AddCurrencyClient(string GUIDClient)
        {
            string query = "INSERT INTO clientsCurrencies ('GUID', 'currency') VALUES (@GUID, @currency)";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            databaseObject.OpenConnection();

            myCommand.Parameters.AddWithValue("@GUID", GUIDClient);

            Console.Write("Enter the currency to add to the list: ");
            string currency = Console.ReadLine();
            myCommand.Parameters.AddWithValue("@currency", currency);

            var result = myCommand.ExecuteNonQuery();

            databaseObject.CloseConnection();

            //check that the value has been correctly added :
            Console.WriteLine("Rows added in clientsCurrencies: {0}", result);
        }


        private void ChangeCurrency(string GUIDClient)
        {
            databaseObject.OpenConnection();

            //set the new main currency
            string queryCurrencies = "SELECT currency FROM clientsCurrencies WHERE GUID= '" + GUIDClient + "'";

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
            else
            {
                Console.WriteLine("Error");
            }
            string newMainCurrency = Console.ReadLine();

            string queryUpdateMainCurrency = "UPDATE clients SET MainCurrency='" + newMainCurrency + "' WHERE GUID='" + GUIDClient + "'";

            SQLiteCommand myCommandUpdateMainCurrency = new SQLiteCommand(queryUpdateMainCurrency, databaseObject.myConnection);

            var result = myCommandUpdateMainCurrency.ExecuteNonQuery();

            //check that the value has been correctly updated :
            Console.WriteLine("Rows updated in clients: {0}", result);

            /*
            databaseObject.CloseConnection();

            databaseObject.OpenConnection();

            //update the amount of the bank account from the new currency's rate
            string queryMainCurrency = "SELECT MainCurrency FROM clients WHERE GUID='" + GUIDClient + "'";

            SQLiteCommand myCommandGetNewMainCurrency = new SQLiteCommand(queryMainCurrency, databaseObject.myConnection);

            SQLiteDataReader mainCurrency = myCommandGetNewMainCurrency.ExecuteReader();

            string queryRate = "SELECT rate FROM currencies WHERE currencyCode='" + mainCurrency + "'";

            if (mainCurrency.HasRows)
            {
                while (mainCurrency.Read())
                {
                    SQLiteCommand myCommandRate = new SQLiteCommand(queryRate, databaseObject.myConnection);

                    SQLiteDataReader Rate = myCommandRate.ExecuteReader();

                    string queryAmount = "UPDATE clients SET moneyAmount=moneyAmount*'" + Rate + "'";
                    if (Rate.HasRows)
                    {
                        while (Rate.Read())
                        {
                            SQLiteCommand myCommandNewAmount = new SQLiteCommand(queryAmount, databaseObject.myConnection);

                            var resultNewAmount = myCommandNewAmount.ExecuteNonQuery();

                            databaseObject.CloseConnection();

                            //check that the value has been correctly update :
                            Console.WriteLine("Rows updated in clients: {0}", resultNewAmount);

                        }
                    }
                    else
                    {
                        Console.WriteLine("Error 1.");
                    }

                }

            }
            else
            {
                Console.WriteLine("Error 2");
            }*/

            databaseObject.CloseConnection();
            return;

        }

        private void SendMessage(string GUIDClient)
        {
            databaseObject.OpenConnection();

            string query = "INSERT INTO messages ('contenu', 'GUIDClient', 'wasRead') VALUES (@contenu, @GUIDClient, @wasRead)";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            Console.WriteLine("Please enter your message to the admin: ");
            string contenu = Console.ReadLine();
            myCommand.Parameters.AddWithValue("@contenu", contenu);

            myCommand.Parameters.AddWithValue("@GUIDClient", GUIDClient);

            int wasRead=0;
            myCommand.Parameters.AddWithValue("@wasRead", wasRead);

            var result = myCommand.ExecuteNonQuery();

            databaseObject.CloseConnection();

            //check that the value has been correctly updated :
            Console.WriteLine("Rows updated in messages: {0}", result);
            ClientMenu(GUIDClient);
        }

        private void AboutClient(string GUIDClient)
        {
            string query = "SELECT GUID, MainCurrency FROM clients WHERE GUID= '" + GUIDClient + "'";
            string queryCurrencies = "SELECT currency FROM clientsCurrencies WHERE GUID= '" + GUIDClient + "'";

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
            ViewTotalMoney(GUIDClient);

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

        public void updateRates()
        {
            var data = ApiConnection.ApiFetch();

            //check that the API returns the proper data
            //Console.Write(data);

            databaseObject.OpenConnection();

            //update the rates in the currencies database table

            databaseObject.CloseConnection();

        }

        static void Main(string[] args)
        {
            Program obj = new Program();
            obj.updateRates();
            obj.MainMenu();
        }
    }
}