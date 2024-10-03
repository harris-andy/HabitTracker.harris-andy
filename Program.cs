﻿/*
If you haven't, try using parameterized queries to make your application more secure.

Seed Data into the database automatically when the database gets created for the first time, generating a few habits and inserting a hundred records with randomly generated values. This is specially helpful during development so you don't have to reinsert data every time you create the database.

Create a report functionality where the users can view specific information (i.e. how many times the user ran in a year? how many kms?) SQL allows you to ask very interesting things from your database.
*/

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using Microsoft.Data.Sqlite;

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
                    Console.WriteLine("\nGoodbye!\n");
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
                    GetSpecificRecord();
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
    string date = GetDateInput();
    string hobby = GetHobby();
    string units = GetUnitsInput();
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
    while (!DateTime.TryParseExact(date, format: "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
    {
        Console.WriteLine("Enter date in format dd-mm-yy. Press 0 to return to Main Menu");
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
                        Date = DateTime.ParseExact(reader.GetString(1), format: "dd-mm-yy", new CultureInfo("en-US")),
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
            Console.WriteLine($"{record.Date.ToString("dd-MMM-yyyy"),-12} {record.Hobby,-15} {record.Units,-5}: {record.Quantity,-5}");
        }
        Console.WriteLine("--------------------------------------------------\n");
    }
}

void GetSpecificRecord()
{
    Console.Clear();
    string activity = GetActivity();
    Console.Clear();

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        using (var command = new SqliteCommand("SELECT Hobby, COUNT(*) AS ActivityCount, SUM(Quantity) AS TotalQuantity, Units FROM habits WHERE hobby = @activity GROUP BY Hobby, Units", connection))
        {
            command.Parameters.AddWithValue("@activity", activity);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    string hobby = reader.GetString(0);
                    int count = reader.GetInt32(1);
                    int quantity = reader.GetInt32(2);
                    string units = reader.GetString(3);
                    string howMany = count == 1 ? "time" : "times";

                    Console.WriteLine("--------------------------------------------------\n");
                    Console.WriteLine($"You did {hobby} {count} {howMany} for {quantity} {units}. Nice!");
                    Console.WriteLine("--------------------------------------------------\n");
                }
                else
                {
                    Console.WriteLine("No records found.");
                }
            }
        }
        connection.Close();
    }
}

string GetActivity()
{
    List<string> actions = new List<string>();
    string activity = "";

    using (var connection = new SqliteConnection(connectionString))
    {
        connection.Open();
        using (var fetchActivity = new SqliteCommand("SELECT DISTINCT Hobby FROM habits", connection))
        {
            using (var reader = fetchActivity.ExecuteReader())
            {
                while (reader.Read())
                {
                    actions.Add(reader.GetString(0));
                }
            };
        }
        connection.Close();

        if (actions.Count == 0)
        {
            Console.WriteLine("No records found");
        }
        else
        {
            Console.WriteLine("Enter the number for which action you'd like summary. Or press 0 to return to Main Menu");
            for (int i = 0; i < actions.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {actions[i]}");
            }
            string? temp = Console.ReadLine();
            if (int.TryParse(temp, out int number))
            {
                if (number == 0) GetUserInput();
                while (number > actions.Count)
                {
                    Console.WriteLine("Try again, enter a valid number");
                }
                activity = actions[number - 1];
            }
        }
    }
    return activity;
}

void Delete()
{
    Console.Clear();
}

void Update()
{
    Console.Clear();

}

class HobbyRecord
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string? Hobby { get; set; }
    public string? Units { get; set; }
    public int Quantity { get; set; }
}