using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace ATM
{
    class Client
    {

        Database databaseObject = new Database();
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

        public void CreateUser()
        {

            string query = "INSERT INTO clients ('GUID', 'FirstName', 'LastName', 'PIN', 'MainCurrency') VALUES (@GUID, @FirstName, @LastName, @PIN, @MainCurrency)";

            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            databaseObject.OpenConnection();
            
            myCommand.Parameters.AddWithValue("@GUID", "4E21955A-7707-4F65-BD67-7341CC6A5051");
            myCommand.Parameters.AddWithValue("@FirstName", "Quentin");
            myCommand.Parameters.AddWithValue("@LastName", "Mourey");
            myCommand.Parameters.AddWithValue("@PIN", "9101");
            myCommand.Parameters.AddWithValue("@MainCurrency", "Euro");
            var result = myCommand.ExecuteNonQuery();

            databaseObject.CloseConnection();

            Console.WriteLine("Rows added: {0}", result);
        }

        public void GetClient(int id)
        {

        }

        public void UpdateClient(Client c)
        {

        }

        public void DeleteClient(int id)
        {

        }

        /*
        private void DepositMoney()
        {
            Console.Clear();
            Center("**** GTBPI Banking System | Deposit Money ****\n");
            DrawLine();
            Console.Write("{0}", AlignText(30, "Enter amount you want to deposit : ", "L"));
            Double DepositAmount = Double.Parse(Console.ReadLine());
            User.Total_Balance += DepositAmount;
            Console.WriteLine("\n");
            Center("Amount deposited in your account successfully!");
            User.UpdatePassbook(DepositAmount, "Deposit");
            UpdatedBalance();
        }
        private void WithdrawMoney()
        {
            Console.Clear();
            Center("**** GTBPI Banking System | Withdraw Money ****\n");
            DrawLine();
            Console.Write("{0}", AlignText(30, "Enter amount you want to withdraw : ", "L"));
            Double WithDrawalAmount = Double.Parse(Console.ReadLine());
            Console.WriteLine("\n");
            if (WithDrawalAmount <= User.Total_Balance)
            {
                User.Total_Balance -= WithDrawalAmount;
                Center("Amount withdrawal from your account was successfull!");
                User.UpdatePassbook(WithDrawalAmount, "Withdrawal");
                UpdatedBalance();
            }
            else
                Center("You don't have sufficient balance in your account to complete this transaction!");
        }
        private void UpdatedBalance()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("\n");
            DrawLine();
            Console.WriteLine("|{0}|", AlignText(25, "Updated balance in your account is Rs. " + User.Total_Balance));
            DrawLine();
            Console.BackgroundColor = ConsoleColor.Black;
        }
        private void TransferMoney()
        {
            Console.Clear();
            Center("**** GTBPI Banking System | Transfer Money ****\n");
            DrawLine();
            Console.Write("{0}", AlignText(20, "Enter amount you want to transfer               : ", "L"));
            Double TransferAmount = Double.Parse(Console.ReadLine());
            if (TransferAmount <= User.Total_Balance)
            {
                ReadAndWriteDataBase Transfer = new ReadAndWriteDataBase();
                Console.Write("{0}", AlignText(20, "Enter Account Number where you want to transfer : ", "L"));
                Transfer.Account_Number = UInt32.Parse(Console.ReadLine());
                if (Transfer.ReadFromDatabase())
                {
                    Console.WriteLine("\n{0}", AlignText(20, "The account number " + Transfer.Account_Number + " belongs to " + Transfer.Title + ". " + Transfer.Name, "L"));
                    Console.Write("{0}", AlignText(20, "Do you want to proceed with this transaction [y/n] ", "L"));
                    char choice = Console.ReadLine()[0];
                    Console.WriteLine("\n");
                    if (choice == 'y' || choice == 'Y')
                    {
                        Transfer.Total_Balance += TransferAmount;
                        User.Total_Balance -= TransferAmount;
                        Transfer.WriteToDatabase(4);
                        User.WriteToDatabase(3);
                        Center("Rs. " + TransferAmount + " has been successfully transfered to " + Transfer.Title + ". " + Transfer.Name + "[" + Transfer.Account_Number + "]");
                        User.UpdatePassbook(TransferAmount, "NEFT To " + Transfer.Account_Number);
                        Transfer.UpdatePassbook(TransferAmount, "NEFT From " + User.Account_Number);
                        UpdatedBalance();
                    }
                    else
                    {
                        Center("The transaction has been aborted!");
                    }
                }
                else
                {
                    Console.WriteLine("\n");
                    Center("Sorry but the account number : " + Transfer.Account_Number + " does not exist in our database");
                    Center("Please check the account number and try again!");
                }
                Transfer.CloseConnection();
            }
            else
            {
                Console.WriteLine("\n");
                Center("You don't have sufficient balance in your account to complete this transaction");
            }
        }*/
    }
}
