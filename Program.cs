using System;
using System.Data.SQLite;

namespace ATM
{
    class Program
    {
        static void Main(string[] args)
        {
            Database databaseObject = new Database();


            /**
             * Insert data into the database Clients - for each new person, manually change the values in the AddWithValue lines
             * */

            /*
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

            //check that the value has been correctly added :
            //Console.WriteLine("Rows added: {0}", result);

            */            

           /**
             * Select data into the database Clients - returns everything
             * */

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


            Console.ReadKey();
        }
    }
}
