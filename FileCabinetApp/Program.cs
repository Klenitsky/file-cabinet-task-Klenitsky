﻿using System;
using System.Globalization;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// The class of the programm.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Konstantin Klenitsky";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static bool isRunning = true;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "prints the statistics", "The 'stat' command prints the statistics." },
            new string[] { "create", "creates a new record", "The 'create' command creates a new record." },
            new string[] { "list", "returns a list of records", "The 'list' command returns a list of recordscre." },
            new string[] { "edit", "edits the record with entered id.", "The 'edit' command edits the record with entered id" },
            new string[] { "find", "finds the record according to the parameters.", "The 'edit' command finds the record according to the parameters." },
        };

        private static FileCabinetService fileCabinetService = new FileCabinetService(new DefaultValidator());

        /// <summary>
        /// The main function of the application.
        /// </summary>
        /// <param name="args">Array of arguments.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            bool isCustom = false;
            if (args != null && args.Length > 0)
            {
                if (args[0] == "-v" && args[1].ToLower(CultureInfo.CurrentCulture) == "custom")
                {
                    fileCabinetService = new FileCabinetService(new CustomValidator());
                    isCustom = true;
                }

                if (args[0].Contains("--validation-rules=", StringComparison.InvariantCulture) && args[0][19..].ToLower(CultureInfo.CurrentCulture) == "custom")
                {
                    fileCabinetService = new FileCabinetService(new CustomValidator());
                    isCustom = true;
                }
            }

            if (isCustom)
            {
                Console.WriteLine($"Using custom validation rules.");
            }
            else
            {
                Console.WriteLine($"Using default validation rules.");
            }

            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.OrdinalIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }

        private static void Stat(string parameters)
        {
            var recordsCount = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create(string parameters)
        {
            string firstName = string.Empty, lastName = string.Empty;
            DateTime dateTime = default(DateTime);
            short height = 0;
            decimal weight = 0;
            char drivingLicenseCategory = ' ';

            EnterParameters(out firstName, out lastName, out dateTime, out height, out weight, out drivingLicenseCategory);

            try
            {
                    Program.fileCabinetService.CreateRecord(new Arguments(firstName, lastName, dateTime, height, weight, drivingLicenseCategory));
            }
            catch (ArgumentException)
            {
                    Console.WriteLine("Data is invalid");
                    return;
            }

            Console.WriteLine($"\nRecord #" + Program.fileCabinetService.GetStat().ToString(CultureInfo.CurrentCulture) + " created ");
        }

        private static void List(string parameters)
        {
            FileCabinetRecord[] arrayOfRecords = Program.fileCabinetService.GetRecords();
            foreach (FileCabinetRecord record in arrayOfRecords)
            {
                Console.WriteLine(record.ToString());
            }
        }

        private static void Edit(string parameters)
        {
            int id;
            bool success = int.TryParse(parameters, out id);
            if (!success)
            {
                Console.WriteLine("Invalid Id");
                return;
            }

            try
            {
                Program.fileCabinetService.EditRecord(id, new Arguments("ex", "ex", DateTime.Today, 0, 0, 'A'));
            }
            catch (ArgumentException)
            {
                Console.WriteLine($"#" + id + " record is not found");
            }

            string firstName = string.Empty, lastName = string.Empty;
            DateTime dateTime = default(DateTime);
            short height = 0;
            decimal weight = 0;
            char drivingLicenseCategory = ' ';

            EnterParameters(out firstName, out lastName, out dateTime, out height, out weight, out drivingLicenseCategory);

            try
            {
                Program.fileCabinetService.EditRecord(id, new Arguments(firstName, lastName, dateTime, height, weight, drivingLicenseCategory));
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Data is invalid");
                return;
            }

            Console.WriteLine($"Record #" + id + "is updated");
        }

        private static void Find(string parameters)
        {
            int index = parameters.IndexOf(' ', StringComparison.InvariantCulture);
            StringBuilder property = new StringBuilder(parameters, 0, index, char.MaxValue);
            FileCabinetRecord[] result = Array.Empty<FileCabinetRecord>();

            if (property.ToString().ToLower(CultureInfo.CurrentCulture) == "firstname".ToLower(CultureInfo.CurrentCulture))
            {
                StringBuilder name = new StringBuilder(parameters, index + 2, parameters.Length - (index + 3), 255);
                while ((name.Length < 2) || (name.Length > 60))
                {
                    Console.WriteLine("First name is invalid, try again: ");
                    Console.Write("First Name: ");
                    name = new StringBuilder(Console.ReadLine());
                }

                result = Program.fileCabinetService.FindByFirstName(name.ToString());
            }

            if (property.ToString().ToLower(CultureInfo.CurrentCulture) == "lastname".ToLower(CultureInfo.CurrentCulture))
            {
                StringBuilder name = new StringBuilder(parameters, index + 2, parameters.Length - index - 3, 255);
                while ((name.Length < 2) || (name.Length > 60))
                {
                    Console.WriteLine("First name is invalid, try again: ");
                    Console.Write("Last Name: ");
                    name = new StringBuilder(Console.ReadLine());
                }

                result = Program.fileCabinetService.FindByLastName(name.ToString());
            }

            if (property.ToString().ToLower(CultureInfo.CurrentCulture) == "dateofbirth".ToLower(CultureInfo.CurrentCulture))
            {
                StringBuilder date = new StringBuilder(parameters, index + 2, parameters.Length - index - 3, 255);
                DateTime dateTime;
                bool success = DateTime.TryParse(date.ToString(), out dateTime);
                while (!success)
                {
                    Console.WriteLine("Date  is invalid, try again: ");
                    Console.Write("Date: ");
                    date = new StringBuilder(Console.ReadLine());
                    success = DateTime.TryParse(date.ToString(), out dateTime);
                }

                result = Program.fileCabinetService.FindByDateOfBirth(dateTime);
            }

            if (result.Length == 0)
            {
                Console.WriteLine("No elements with such property");
            }

            foreach (FileCabinetRecord record in result)
            {
                Console.WriteLine(record.ToString());
            }
        }

        private static void EnterParameters(out string firstName, out string lastName, out DateTime dateOfBirth, out short height, out decimal weight, out char drivingLicenseCategory)
        {
            Console.Write("First Name: ");
            firstName = Console.ReadLine();
            while ((firstName.Length < 2) || (firstName.Length > 60) || string.IsNullOrWhiteSpace(firstName))
            {
                Console.WriteLine("First name is invalid, try again: ");
                Console.Write("First Name: ");
                firstName = Console.ReadLine();
            }

            Console.Write("Last Name: ");
            lastName = Console.ReadLine();
            while ((lastName.Length < 2) || (lastName.Length > 60) || string.IsNullOrWhiteSpace(lastName))
            {
                Console.WriteLine("Last name is invalid, try again: ");
                Console.Write("Last Name: ");
                lastName = Console.ReadLine();
            }

            Console.Write("Date of birth: ");
            string date = Console.ReadLine();
            bool success = DateTime.TryParse(date, out dateOfBirth);
            while (!success || (DateTime.Compare(dateOfBirth, DateTime.Today) > 0) || DateTime.Compare(dateOfBirth, new DateTime(1950, 1, 1)) < 0)
            {
                Console.WriteLine("Date  is invalid, try again: ");
                Console.Write("Date: ");
                date = Console.ReadLine();
                success = DateTime.TryParse(date, out dateOfBirth);
            }

            Console.Write("Height: ");
            string nextLine = Console.ReadLine();
            success = short.TryParse(nextLine, out height);
            while (!success || height < 0 || height > 250)
            {
                Console.WriteLine("Height is invalid, try again: ");
                Console.Write("Height: ");
                nextLine = Console.ReadLine();
                success = short.TryParse(nextLine, out height);
            }

            Console.Write("Weight: ");
            nextLine = Console.ReadLine();
            success = decimal.TryParse(nextLine, out weight);
            while (!success || weight < 0 || weight > 180)
            {
                Console.WriteLine("Weight is invalid, try again: ");
                Console.Write("Weight: ");
                nextLine = Console.ReadLine();
                success = decimal.TryParse(nextLine, out weight);
            }

            Console.Write("Driving license category: ");
            nextLine = Console.ReadLine();
            success = char.TryParse(nextLine, out drivingLicenseCategory);
            while (!success || ((drivingLicenseCategory != 'A') && (drivingLicenseCategory != 'B') && (drivingLicenseCategory != 'C') && (drivingLicenseCategory != 'D')))
            {
                Console.WriteLine("License category is invalid, try again: ");
                Console.Write("Driving license category: ");
                nextLine = Console.ReadLine();
                success = char.TryParse(nextLine, out drivingLicenseCategory);
            }
        }
    }
}