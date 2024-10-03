// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;

// namespace HabitTracker.harris_andy
// {
//     public class FART
//     {
//         void Insert()
//         {
//             string date = GetDateInput();

//             int quantity = GetNumberInput("\nPlease enter number of glasses or other measure (no decimals allowed)\n");

//             using (var connection = new SqliteConnection(connectionString))
//             {
//                 connection.Open();
//                 var tableCmd = connection.CreateCommand();

//                 tableCmd.CommandText = $"INSERT INTO drinking_water(date, quantity) VALUES('{date}', {quantity})";

//                 tableCmd.ExecuteNonQuery();

//                 connection.Close();
//             }
//         }



//         int GetNumberInput(string message)
//         {
//             string? numberInput = null;

//             while (numberInput == null)
//             {
//                 Console.WriteLine(message);
//                 if (numberInput == "0") GetUserInput();
//                 numberInput = Console.ReadLine();
//             }
//             int finalInput = Convert.ToInt32(numberInput);
//             return finalInput;
//         }

//         void GetAllRecords()
//         {
//             Console.Clear();
//             using (var connection = new SqliteConnection(connectionString))
//             {
//                 connection.Open();
//                 var tableCmd = connection.CreateCommand();
//                 tableCmd.CommandText = $"SELECT * FROM drinking_water";


//                 List<DrinkingWater> tableData = new();

//                 SqliteDataReader reader = tableCmd.ExecuteReader();

//                 if (reader.HasRows)
//                 {
//                     while (reader.Read())
//                     {
//                         tableData.Add(
//                         new DrinkingWater
//                         {
//                             Id = reader.GetInt32(0),
//                             Date = DateTime.ParseExact(reader.GetString(1), format: "dd-mm-yy", new CultureInfo("en-US")),
//                             Quantity = reader.GetInt32(2)
//                         }
//                         );
//                     }
//                 }

//                 connection.Close();

//                 Console.WriteLine("--------------------------------------------------\n");
//                 if (tableData.Count == 0)
//                 {
//                     Console.WriteLine("No rows found\n");
//                 }
//                 foreach (var dw in tableData)
//                 {
//                     Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MMM-yyyy")} - Quantity: {dw.Quantity}");
//                 }
//                 Console.WriteLine("--------------------------------------------------\n");
//             }
//         }

//         void Delete()
//         {
//             Console.Clear();
//             GetAllRecords();

//             var recordID = GetNumberInput("\n\nPlease type the ID of the record you want to delete. Or press 0 to return to Main Menu.");

//             if (recordID == 0) GetUserInput();

//             using (var connection = new SqliteConnection(connectionString))
//             {
//                 connection.Open();
//                 var tableCmd = connection.CreateCommand();
//                 tableCmd.CommandText = $"DELETE from drinking_water WHERE ID = '{recordID}'";
//                 int rowCount = tableCmd.ExecuteNonQuery();

//                 if (rowCount == 0)
//                 {
//                     Console.WriteLine("\n\nRecord with ID {recordID} doesn't exist. Try again.\n");
//                     Delete();
//                 }

//                 Console.WriteLine($"Record with ID {recordID} was deleted.");
//                 connection.Close();
//             }
//             GetUserInput();
//         }

//         void Update()
//         {
//             Console.Clear();
//             GetAllRecords();

//             var recordID = GetNumberInput("\n\nPlease type the ID of the record you want to update. Or press 0 to return to Main Menu.");

//             if (recordID == 0) GetUserInput();

//             using (var connection = new SqliteConnection(connectionString))
//             {
//                 connection.Open();

//                 var checkCmd = connection.CreateCommand();
//                 checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM drinking_water WHERE ID = {recordID})";
//                 int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

//                 if (checkQuery == 0)
//                 {
//                     Console.WriteLine("\nRecord with ID {recodID} doesn't exist.\n");
//                     connection.Close();
//                     Update();
//                 }

//                 string date = GetDateInput();
//                 int quantity = GetNumberInput("\nEnter number of units (no decimals)");

//                 var tableCmd = connection.CreateCommand();
//                 tableCmd.CommandText = $"UPDATE drinking_water SET date = '{date}', quantity = {quantity} WHERE Id = {recordID}";
//                 tableCmd.ExecuteNonQuery();

//                 connection.Close();
//             }
//             GetUserInput();
//         }

//         class DrinkingWater
//         {
//             public int Id { get; set; }
//             public DateTime Date { get; set; }
//             public int Quantity { get; set; }
//         }




//     }
// }
//     }
// }