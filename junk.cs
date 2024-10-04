// string GetActivity()
// {
//     List<string> actions = new List<string>();
//     string activity = "";

//     using (var connection = new SqliteConnection(connectionString))
//     {
//         connection.Open();
//         using (var fetchActivity = new SqliteCommand("SELECT DISTINCT Hobby FROM habits", connection))
//         {
//             using (var reader = fetchActivity.ExecuteReader())
//             {
//                 while (reader.Read())
//                 {
//                     actions.Add(reader.GetString(0));
//                 }
//             };
//         }
//         connection.Close();

//         if (actions.Count == 0)
//         {
//             Console.WriteLine("No records found");
//         }
//         else
//         {
//             Console.WriteLine("Enter the number for which action you'd like summary. Or press 0 to return to Main Menu");
//             for (int i = 0; i < actions.Count; i++)
//             {
//                 Console.WriteLine($"{i + 1}: {actions[i]}");
//             }
//             string? temp = Console.ReadLine();
//             if (int.TryParse(temp, out int number))
//             {
//                 if (number == 0) GetUserInput();
//                 while (number > actions.Count)
//                 {
//                     Console.WriteLine("Try again, enter a valid number");
//                 }
//                 activity = actions[number - 1];
//             }
//         }
//     }
//     return activity;
// }
