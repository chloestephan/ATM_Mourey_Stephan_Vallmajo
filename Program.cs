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
            try
            {

                Console.Clear();
                Center("**** Welcome to FQC ATM System ****\n");
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
            catch (Exception)
            {
                MainMenu();
            }
        }

        /* --- ADMIN --- */
        public void AdminMenu()
        {
            try
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                DrawLine();
                Console.WriteLine("|{0}|", AlignText(0, ""));
                Console.WriteLine("|{0}|", AlignText(37, "1. Create a client"));
                Console.WriteLine("|{0}|", AlignText(37, "2. Delete a client"));
                Console.WriteLine("|{0}|", AlignText(37, "3. Manage a client (reset tries or change a client's PIN)"));
                Console.WriteLine("|{0}|", AlignText(37, "4. Verify user transactions"));
                Console.WriteLine("|{0}|", AlignText(37, "6. View all clients"));
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

            catch (Exception)
            {
                AdminMenu();
            }
        }

        public void LogInAdmin()
        {
            try
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
                    Console.WriteLine("Please try again");
                    LogInAdmin();
                }
            }

            catch (Exception)
            {
                LogInAdmin();
            }
        }

        public void CreateClient()
        {
            try
            {
                string query = "INSERT INTO clients ('GUID', 'FirstName', 'LastName', 'PIN', 'MainCurrency', 'isBlocked', 'nbrTries') VALUES (@GUID, @FirstName, @LastName, @PIN, @MainCurrency, @isBlocked, @nbrTries)";

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

                //we assume that we don't block a brand new client from the start: all clients start off as not blocked
                //false: not blocked; true: blocked
                bool isBlocked = false;
                myCommand.Parameters.AddWithValue("@isBlocked", isBlocked);

                //we initialize the number of tries as 0
                int nbrTries = 0;
                myCommand.Parameters.AddWithValue("nbrTries", nbrTries);

                var result = myCommand.ExecuteNonQuery();

                databaseObject.CloseConnection();

                //check that the value has been correctly added :
                Console.WriteLine("Rows added: {0}", result);
            }

            catch (Exception)
            {
                CreateClient();
            }
        }


        public void DeleteClient()
        {
            try
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

            catch (Exception)
            {
                DeleteClient();
            }
        }

        public void ManageClient()
        {
            try
            {
                //TODO: menu to select if admin wants to reset tries or change PIN
                Console.WriteLine("|{0}|", AlignText(0, ""));
                Console.WriteLine("|{0}|", AlignText(37, "Please choose what you want to do:"));
                Console.WriteLine("|{0}|", AlignText(37, "1. Reset tries for a client"));
                Console.WriteLine("|{0}|", AlignText(37, "2. Change PIN for a client"));
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
                        default:
                            Console.WriteLine("Incorrect choice, please try again.");
                            break;
                    }
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            catch (Exception)
            {
                ManageClient();
            }
        }

        public void ResetTries()
        {
            try {
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

            catch (Exception)
            {
                ResetTries();
            }
        }

        public void ChangePIN() {

            try
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

            catch (Exception)
            {
                ChangePIN();
            }
        }

        public void VerifyTransactions()
        {
            //change value in the transactions database
            //if verified : bool true ; if not verified : bool false
        }

        public void BlockClient()
        {
            try
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

            catch (Exception)
            {
                BlockClient();
            }
        }

        public void UnblockClient()
        {
            try
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

            catch (Exception)
            {
                UnblockClient();
            }
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

        private void LogInClient()
        {
            databaseObject.OpenConnection();

            Console.WriteLine("Please enter your GUID: ");
            string GUIDClient = Console.ReadLine();
            Console.WriteLine("Please enter your PIN: ");
            string PINClient = Console.ReadLine();

            string query = "SELECT count(1) FROM clients WHERE GUID='" + GUIDClient + "' AND PIN='" + PINClient + "'";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            var result = myCommand.ExecuteNonQuery();

            //check that the value has been correctly added :
            Console.WriteLine("Rows selected: {0}", result);

            if (result == 1)
            {
                ClientMenu(GUIDClient);
            }
            else
            {
                Console.WriteLine("Please try again");
                LogInClient();
            }
            databaseObject.CloseConnection();
        }

        public void ClientMenu(string GUIDClient)
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            DrawLine();
            Console.WriteLine("|{0}|", AlignText(0, ""));
            Console.WriteLine("|{0}|", AlignText(37, "1. Deposit Money"));
            Console.WriteLine("|{0}|", AlignText(37, "2. Withdraw Money"));
            Console.WriteLine("|{0}|", AlignText(37, "3. View total amount"));
            Console.WriteLine("|{0}|", AlignText(37, "4. Change PIN"));
            Console.WriteLine("|{0}|", AlignText(37, "5. Change currency"));
            Console.WriteLine("|{0}|", AlignText(37, "6. Show My Account Details"));
            Console.WriteLine("|{0}|", AlignText(37, "7. Logout"));
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
                    AboutClient(GUIDClient);
                    break;
                case 7:
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
            double amountToDeposit = double.Parse(Console.ReadLine());

            string query = "UPDATE clients SET moneyAmount=moneyAmount+'" + amountToDeposit + "' WHERE GUID= '" + GUIDClient + "'";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            var result = myCommand.ExecuteNonQuery();

            //check that the value has been correctly added :
            Console.WriteLine("Rows updated: {0}", result);

            databaseObject.CloseConnection();
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
        }

        private void ViewTotalMoney(string GUIDClient)
        {
            databaseObject.OpenConnection();

            string queryAmount = "SELECT moneyAmount FROM clients WHERE GUID='" + GUIDClient + "'";
            string queryCurrency = "SELECT currency FROM clients WHERE GUID='" + GUIDClient + "'";

            SQLiteCommand myCommandAmount = new SQLiteCommand(queryAmount, databaseObject.myConnection);
            SQLiteCommand myCommandCurrency = new SQLiteCommand(queryCurrency, databaseObject.myConnection);

            string totalAmount = myCommandAmount.ExecuteScalar().ToString();
            string currency = myCommandCurrency.ExecuteScalar().ToString();
            Console.WriteLine("The total amount in your account is: {0}{1}", totalAmount, currency);

            databaseObject.CloseConnection();
        }

        private void ChangePINClient(string GUIDClient)
        {
            databaseObject.OpenConnection();

            Console.WriteLine("Please enter the new PIN: ");
            double newPIN = double.Parse(Console.ReadLine());

            string query = "UPDATE clients SET PIN='" + newPIN + "' WHERE GUID= '" + GUIDClient + "'";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            var result = myCommand.ExecuteNonQuery();

            //check that the value has been correctly added :
            Console.WriteLine("Rows updated: {0}", result);

            databaseObject.CloseConnection();
        }

        private void ChangeCurrency(string GUIDClient)
        {
            //TODO: add currency array in clients
        }

        private void AboutClient(string GUIDClient)
        {
            databaseObject.OpenConnection();

            string query = "SELECT GUID, FirstName, LastName, MainCurrency FROM clients WHERE GUID= '" + GUIDClient + "'";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            var result = myCommand.ExecuteScalar().ToString();
            Console.WriteLine("Your credentials are the following: {0}", result);

            databaseObject.CloseConnection();
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

