/*
Seed Data into the database automatically when the database gets created for the first time, generating a few habits and inserting a hundred records with randomly generated values. This is specially helpful during development so you don't have to reinsert data every time you create the database.
*/

// using System;
using System.Data;
// using System.Data.SqlClient;
// using System.Collections.Generic;
// using System.Linq;
using System.Globalization;
using Microsoft.Data.Sqlite;
// using System.Runtime.Intrinsics.Arm;

string connectionString = @"Data Source=HabitTracker.db";

using (SqliteConnection connection = new SqliteConnection(connectionString))
{
    connection.Open();
    var tableCmd = connection.CreateCommand();

    tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS habits (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Date TEXT,
        Hobby TEXT,
        Units TEXT,
        Quantity INTEGER
        )";

    tableCmd.ExecuteNonQuery();

    using (var command = new SqliteCommand("SELECT COUNT(*) FROM habits;", connection))
    {
        var count = Convert.ToInt32(command.ExecuteScalar());
        if (count == 0)
        {
            PopulateDatabase();
        }
    }
    connection.Close();
}

GetUserInput();

void GetUserInput()
{
    Console.Clear();
    bool closeApp = false;
    while (closeApp == false)
    {
        Console.WriteLine("\n\nMAIN MENU");
        Console.WriteLine("\nWhat would you like to do?");
        Console.WriteLine("Type 0 to Close Application");
        Console.WriteLine("Type 1 to View All Records");
        Console.WriteLine("Type 2 to Insert Record");
        Console.WriteLine("Type 3 to Delete Record");
        Console.WriteLine("Type 4 to Update Record");
        Console.WriteLine("Type 5 to View A Record Summary");
        Console.WriteLine("--------------------------------------------------\n");

        string? command = Console.ReadLine();
        int tempNumber;
        if (int.TryParse(command, out tempNumber))
        {
            switch (tempNumber)
            {
                case 0:
                    Console.WriteLine("\nLater alligator!\n");
                    closeApp = true;
                    Environment.Exit(0);
                    break;
                case 1:
                    GetAllRecords();
                    break;
                case 2:
                    Insert();
                    break;
                case 3:
                    Delete();
                    break;
                case 4:
                    Update();
                    break;
                case 5:
                    GetRecordSummary();
                    break;
                default:
                    Console.WriteLine("\nInvalid Command. Give me number!");
                    break;
            }
        }
    }
}

void Insert()
{
    Console.Clear();
    string date = GetDateInput();
    Console.Clear();
    string hobby = GetHobby();
    Console.Clear();
    string units = GetUnitsInput();
    Console.Clear();
    int quantity = GetQuantityInput();

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        using (var command = new SqliteCommand("INSERT INTO habits (Date, Hobby, Units, Quantity) VALUES (@date, @hobby, @units, @quantity)", connection))
        {
            command.Parameters.AddWithValue("@date", date);
            command.Parameters.AddWithValue("@hobby", hobby);
            command.Parameters.AddWithValue("@units", units);
            command.Parameters.AddWithValue("@quantity", quantity);

            command.ExecuteNonQuery();
        }
        connection.Close();
    }
}

string GetDateInput()
{
    string? date = null;
    while (!DateTime.TryParseExact(date, format: "dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
    {
        Console.WriteLine("Enter date in format dd-mm-yyyy. Press 0 to return to Main Menu");
        date = Console.ReadLine();
        if (int.TryParse(date, out int number))
        {
            if (number == 0) GetUserInput();
        }
    }
    return date;
}

string GetUnitsInput()
{
    string units = "";

    while (string.IsNullOrWhiteSpace(units))
    {
        Console.WriteLine("Enter whatever unit of measure you'd like to use. Or press 0 to return to Main Menu");
        string? temp = Console.ReadLine();
        if (temp != null)
        {
            units = temp.Trim().ToLower();
        }
        if (units == "0") GetUserInput();
    }
    return units;
}

int GetQuantityInput()
{
    int quantity = -1;

    while (quantity < 0)
    {
        Console.WriteLine("Enter amount of activity done/consumed/lost/whatever (greater than 0). Or press 0 to return to Main Menu");
        string? temp = Console.ReadLine();
        if (!int.TryParse(temp, out quantity) || quantity < 0)
        {
            Console.WriteLine("Try again. A quantity is a positive number. Like 14, 42, 900, etc. You get the idea.");
        }
        if (quantity == 0) GetUserInput();
    }
    return quantity;
}

string GetHobby()
{
    string? hobby = "";
    while (string.IsNullOrWhiteSpace(hobby))
    {
        Console.WriteLine("Enter name of activity (keep it short). Or press 0 to return to Main Menu");
        string? temp = Console.ReadLine();
        hobby = temp?.Trim().ToLower();
    }
    if (hobby == "0") GetUserInput();
    return hobby;
}

void GetAllRecords()
{
    Console.Clear();
    using (SqliteConnection connection = new SqliteConnection(connectionString))
    {
        List<HobbyRecord> hobbiesRecord = new List<HobbyRecord>();

        connection.Open();
        using (SqliteCommand command = new SqliteCommand("SELECT * FROM habits", connection))
        {
            // command.Parameters.AddWithValue("@date", date);

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    hobbiesRecord.Add(new HobbyRecord
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.ParseExact(reader.GetString(1), format: "dd-mm-yyyy", new CultureInfo("en-US")),
                        Hobby = reader.GetString(2),
                        Units = reader.GetString(3),
                        Quantity = reader.GetInt32(4)
                    });
                }
            }
        }
        connection.Close();
        Console.WriteLine("--------------------------------------------------\n");
        Console.WriteLine("\tHere's all the fun stuff you did!\n");
        if (hobbiesRecord.Count == 0)
        {
            Console.WriteLine("No records found. Do stuff!");
        }
        var sortedHobbies = hobbiesRecord.OrderBy(record => record.Date).ToList();
        foreach (HobbyRecord record in sortedHobbies)
        {
            Console.WriteLine($"{record.Date.ToString("dd-MMM-yyyy"),-14} {record.Hobby,-14} {record.Units,-5}: {record.Quantity,-5}");
        }
        Console.WriteLine("--------------------------------------------------\n");
    }
}

void GetRecordSummary()
{
    Console.Clear();
    (string searchTerm, string searchTermCategory) = GetSearchTerm();
    Console.Clear();

    using (var connection = new SqliteConnection(connectionString))
    {
        SqliteCommand chosenCommand;
        var commandYear = new SqliteCommand("SELECT SUBSTR(Date, 7, 4) AS Year, Hobby, Units, SUM(Quantity) AS TotalQuantity FROM habits WHERE SUBSTR(Date, 7, 4) = @year GROUP BY Year, Hobby;", connection);

        var commandHobby = new SqliteCommand("SELECT Hobby, Units, COUNT(*) AS TotalCount, SUM(Quantity) AS TotalUnits FROM habits WHERE Hobby = @hobby GROUP BY Hobby, Units;", connection);
        var commandUnits = new SqliteCommand("SELECT SUBSTR(Date, 7, 4) AS Year, Hobby, Units, SUM(Quantity) AS TotalQuantity FROM habits WHERE Units = @units GROUP BY Hobby, Units;", connection);

        if (searchTermCategory == "year")
        {
            chosenCommand = commandYear;
            string year = searchTerm;
            chosenCommand.Parameters.AddWithValue("@year", year);
        }
        else if (searchTermCategory == "hobby")
        {
            chosenCommand = commandHobby;
            string hobby = searchTerm;
            chosenCommand.Parameters.AddWithValue("@hobby", hobby);
        }
        else
        {
            chosenCommand = commandUnits;
            string units = searchTerm;
            chosenCommand.Parameters.AddWithValue("@units", units);
        }

        connection.Open();
        using (chosenCommand)
        {
            using (var reader = chosenCommand.ExecuteReader())
            {
                if (searchTermCategory == "hobby")
                {
                    if (reader.Read())
                    {
                        string activity = reader.GetString(0);
                        // string formattedActivity = activity.Substring(0).ToUpper() + activity.Substring(1:);
                        string units = reader.GetString(1);
                        int count = reader.GetInt32(2);
                        int quantity = reader.GetInt32(3);
                        string howManyTimes = count == 1 ? "time" : "times";

                        Console.WriteLine("--------------------------------------------------\n");
                        Console.WriteLine($"{activity.Substring(0, 1).ToUpper() + activity.Substring(1)} done {count} {howManyTimes} for {quantity} {units}. Nice!\n");
                        Console.WriteLine("--------------------------------------------------\n");
                    }
                }
                else
                {
                    int totalQuantity = 0;
                    List<string> actionRecord = new();
                    string howManyActivities = actionRecord.Count == 1 ? "activity" : "activities";

                    while (reader.Read())
                    {
                        int year = reader.GetInt32(0);
                        string activity = reader.GetString(1);
                        string units = reader.GetString(2);
                        int quantity = reader.GetInt32(3);
                        totalQuantity += quantity;
                        actionRecord.Add($"{activity.Substring(0, 1).ToUpper() + activity.Substring(1)}: {quantity} {units}");
                    }

                    string yearOutput = $"Completed {actionRecord.Count} {howManyActivities} in {searchTerm}:\n";
                    string unitsOutput = $"{totalQuantity} {searchTerm} completed across {actionRecord.Count} {howManyActivities}:\n";

                    Console.WriteLine("\n");
                    Console.WriteLine("--------------------------------------------------\n");
                    if (searchTermCategory == "year") Console.WriteLine(yearOutput);
                    if (searchTermCategory == "units") Console.WriteLine(unitsOutput);
                    foreach (string item in actionRecord)
                    {
                        Console.WriteLine(item);
                    }
                    Console.WriteLine("\n");
                    Console.WriteLine("--------------------------------------------------\n");
                }
            }
        }
        connection.Close();
    }
}

Tuple<string, string> GetSearchTerm()
{
    Console.Clear();
    string searchTermCategory = "";
    string searchTerm = "";
    List<string> searchOptions = new List<string>();

    Console.WriteLine("Choose a summary by:");
    Console.WriteLine("1. Year");
    Console.WriteLine("2. Hobby");
    Console.WriteLine("3. Units (e.g. how many miles or hours)");
    string? temp = Console.ReadLine();

    if (int.TryParse(temp, out int number) && number >= 1 && number <= 3)
    {
        switch (number)
        {
            case 1:
                searchTermCategory = "year";
                break;
            case 2:
                searchTermCategory = "hobby";
                break;
            case 3:
                searchTermCategory = "units";
                break;
            default:
                Console.WriteLine("Invalid answer. Number between 1-3:");
                break;
        }
    }

    using (var connection = new SqliteConnection(connectionString))
    {
        var commandYear = new SqliteCommand("SELECT DISTINCT SUBSTR(Date, 7, 4) AS Year FROM habits;", connection);
        var commandHobby = new SqliteCommand("SELECT DISTINCT Hobby FROM habits;", connection);
        var commandUnits = new SqliteCommand("SELECT DISTINCT Units FROM habits;", connection);

        SqliteCommand chosenCommand = (searchTermCategory == "year") ? commandYear
            : (searchTermCategory == "hobby") ? commandHobby
            : commandUnits;

        connection.Open();

        using (chosenCommand)
        {
            using (var reader = chosenCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    searchOptions.Add(reader.GetString(0));
                }
            };
        }
        connection.Close();
        Console.Clear();
        searchOptions.Sort();

        if (searchOptions.Count == 0)
        {
            Console.WriteLine("No records found");
        }
        else
        {
            Console.WriteLine($"Enter the number for which {searchTermCategory} you'd like a summary. Or press 0 to return to Main Menu");
            for (int i = 0; i < searchOptions.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {searchOptions[i]}");
            }
            int tempNumber = -1;
            string? tempInput = Console.ReadLine();
            while (!int.TryParse(tempInput, out tempNumber) || tempNumber < 0 || tempNumber > searchOptions.Count)
            {
                Console.WriteLine("Try again, enter a valid number");
                tempInput = Console.ReadLine();
            }
            if (tempNumber == 0) GetUserInput();
            searchTerm = searchOptions[tempNumber - 1];
        }
    }
    return Tuple.Create(searchTerm, searchTermCategory);
}

void Delete()
{
    Console.Clear();
}

void Update()
{
    Console.Clear();

}

void PopulateDatabase()
{
    Random random = new Random();
    Dictionary<string, string> fakeHobbies = new()
    {
        {"walking", "miles"},
        {"hiking", "miles"},
        {"drinking", "beers"},
        {"sleeping", "hours"},
        {"gaming", "hours"},
        {"programming", "hours"}
    };
    string[] fakeActivities = ["walking", "hiking", "drinking", "sleeping", "gaming", "programming"];

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        for (int randomRecord = 0; randomRecord < 100; randomRecord++)
        {
            int randomIndex = random.Next(fakeActivities.Length);
            string date = GetRandomDate();
            string hobby = fakeActivities[randomIndex];
            string units = fakeHobbies[hobby];
            int quantity = random.Next(10);

            using (var command = new SqliteCommand("INSERT INTO habits (Date, Hobby, Units, Quantity) VALUES (@date, @hobby, @units, @quantity)", connection))
            {
                command.Parameters.AddWithValue("@date", date);
                command.Parameters.AddWithValue("@hobby", hobby);
                command.Parameters.AddWithValue("@units", units);
                command.Parameters.AddWithValue("@quantity", quantity);

                command.ExecuteNonQuery();
            }
        }
        connection.Close();
    }
}

string GetRandomDate()
{
    Random random = new Random();

    string day = Convert.ToString(random.Next(1, 31));
    string month = Convert.ToString(random.Next(1, 13));
    string year = Convert.ToString(random.Next(2023, 2025));
    return $"{day.PadLeft(2, '0')}-{month.PadLeft(2, '0')}-{year}";
}
class HobbyRecord
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string? Hobby { get; set; }
    public string? Units { get; set; }
    public int Quantity { get; set; }
}