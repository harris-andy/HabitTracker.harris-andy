
This is my submission for the cSharpAcademy Habit Logger project found here: https://thecsharpacademy.com/project/12/habit-logger

##Project Description
  - It's a small console CRUD app in which the user can track hobbies which are stored in a local SQLite database.
  - Built with C#/.Net 8


##Usage
  - Follow the instructions and away you go
  - i.e. Select from the menu to perform operations such as: viewing all records, inserting, updating and deleting records.

    ![Game Menu](./images/gamemenu.png)


##Features
   - Custom data entry including hobby, date, unit and quantity. For example "1 hour of walking, 35 minutes of meditation, 2000 calories of food eaten", etc.
   - If the database doesn't already exist, one will be created and filled with 100 rows of fake data to play with.
   - Limited custom report option which summarizes data by year, hobby or unit (e.g. miles, hours, etc.). 
   - Populate fake data 100 rows at a time.
   - Delete table contents from the main menu.
   - Parameterized SQL queries used throughout.


4. More to do
  - I'd like to add more granularity to the custom reports. For example, how many miles of walking did I do in a given month? Or basic math operations like averages.


5. Questions & Comments
  - I'm not sure about the programs's structure. I initially had one GameLogic class (which I got from your video) then I wrote Program.cs as procedural code with conditional statements. It worked but I wanted to challenge myself with OOP so I added a Game class that handles the current game. However I'm not sure if it's organized well.
  - Would this have been better as a single Game class?
  - Which files should I have added to gitignore? This is my first C# program.
  - Any suggestions on how to clarify my code is greatly appreciated! Program structure is by far the most difficult part for me. Thanks!
  - This is just the kind of project I need to get me out of tutorial hell! I'm looking forward to getting better at this.


Created by Andy Harris - [GitHub Profile](https://github.com/harris-andy)