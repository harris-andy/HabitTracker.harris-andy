// // using System;
// // using System.Collections.Generic;
// // using System.Linq;
// // using System.Threading.Tasks;
// using System.Globalization;
// using Microsoft.Data.Sqlite;

// namespace HabitTracker.harris_andy
// {
//     public class DatabaseInteractions
//     {
//         public void GetUserInput()
//         {
//             Console.Clear();
//             bool closeApp = false;
//             while (closeApp == false)
//             {
//                 Console.WriteLine("\n\nMAIN MENU");
//                 Console.WriteLine("\nWhat would you like to do?");
//                 Console.WriteLine("Type 0 to Close Application");
//                 Console.WriteLine("Type 1 to View All Records");
//                 Console.WriteLine("Type 2 to Insert Record");
//                 Console.WriteLine("Type 3 to Delete Record");
//                 Console.WriteLine("Type 4 to Update Record");
//                 Console.WriteLine("--------------------------------------------------\n");

//                 string? command = Console.ReadLine();
//                 int tempNumber;
//                 if (int.TryParse(command, out tempNumber))
//                 {
//                     switch (tempNumber)
//                     {
//                         case 0:
//                             Console.WriteLine("\nGoodbye!\n");
//                             closeApp = true;
//                             Environment.Exit(0);
//                             break;
//                         case 1:
//                             GetAllRecords();
//                             break;
//                         case 2:
//                             Insert();
//                             break;
//                         case 3:
//                             Delete();
//                             break;
//                         case 4:
//                             Update();
//                             break;
//                         default:
//                             Console.WriteLine("\nInvalid Command. Give me number!");
//                             break;
//                     }
//                 }
//             }
//         }

//         public void Insert()
//         {
//             string date = GetDateInput();
//             string hobby = GetHobby();
//             string units = GetUnitsInput();
//             int quantity = GetQuantityInput();

//             using (var connection = new SqliteConnection(connectionString))
//             {
//                 connection.Open();
//                 var tableCmd = connection.CreateCommand();

//                 tableCmd.CommandText = "";

//                 tableCmd.ExecuteNonQuery();

//                 connection.Close();
//             }



//         }

//         public string GetDateInput()
//         {
//             string? date = null;
//             while (!DateTime.TryParseExact(date, format: "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
//             {
//                 Console.WriteLine("Enter date in format dd-mm-yy. Press 0 to return to Main Menu");
//                 date = Console.ReadLine();
//             }
//             if (date == "0") GetUserInput();

//             return date;
//         }

//         public string GetUnitsInput()
//         {
//             string units = "";

//             while (string.IsNullOrWhiteSpace(units))
//             {
//                 Console.WriteLine("Enter whatever unit of measure you'd like to use:");
//                 string? temp = Console.ReadLine();
//                 if (temp != null)
//                 {
//                     units = temp.Trim();
//                 }
//             }
//             return units;
//         }

//         public int GetQuantityInput()
//         {
//             int quantity = 0;

//             while (quantity == 0)
//             {
//                 Console.WriteLine("Enter amount of activity done/consumed/lost/whatever (greater than 0):");
//                 string? temp = Console.ReadLine();
//                 if (!int.TryParse(temp, out quantity))
//                 {
//                     Console.WriteLine("Try again. A quantity is a number. Like 14, 42, 900, etc. You get the idea.");
//                 }
//             }
//             return quantity;
//         }

//         public string GetHobby()
//         {
//             string? hobby = "";
//             while (string.IsNullOrWhiteSpace(hobby))
//             {
//                 Console.WriteLine("Enter name of activity (keep it short):");
//                 string? temp = Console.ReadLine();
//                 hobby = temp?.Trim();
//             }
//             return hobby;
//         }

//         public void GetAllRecords()
//         { }

//         public void Delete()
//         {
//             Console.Clear();
//         }

//         public void Update()
//         {
//             Console.Clear();

//         }
//     }
// }