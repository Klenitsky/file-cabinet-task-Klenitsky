using System;
using System.Globalization;

namespace FileCabinetApp
{
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
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "prints the statistics", "The 'stat' command prints the statistics." },
            new string[] { "create", "creates a new record", "The 'stat' command creates a new record." },
            new string[] { "list", "returns a list of records", "The 'stat' command returns a list of recordscre." },
        };

        private static FileCabinetService fileCabinetService = new FileCabinetService();

        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
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

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
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
            Console.Write("First Name: ");
            string firstname = Console.ReadLine();
            Console.Write("Last Name: ");
            string lastname = Console.ReadLine();
            Console.Write("Date of birth: ");
            string date = Console.ReadLine();
            DateTime dateTime;
            bool success = DateTime.TryParse(date, out dateTime);
            if (!success)
            {
                return;
            }

            Console.Write("Height: ");
            string tmp = Console.ReadLine();
            short height;
            success = short.TryParse(tmp, out height);
            if (!success)
            {
                return;
            }

            Console.Write("Weight: ");
            tmp = Console.ReadLine();
            decimal weight;
            success = decimal.TryParse(tmp, out weight);
            if (!success)
            {
                return;
            }

            Console.Write("Driving license category: ");
            tmp = Console.ReadLine();
            char category;
            success = char.TryParse(tmp, out category);
            if (!success)
            {
                return;
            }

            Program.fileCabinetService.CreateRecord(firstname, lastname, dateTime, height, weight, category);
            Console.WriteLine($"\nRecord #{1} created ", Program.fileCabinetService.GetStat());
        }

        private static void List(string parameters)
        {
            FileCabinetRecord[] tmpArray = Program.fileCabinetService.GetRecords();
            foreach (FileCabinetRecord tmp in tmpArray)
            {
                Console.Write($"#{1}, ", tmp.Id);
                Console.Write(tmp.FirstName);
                Console.Write(", ");
                Console.Write(tmp.LastName);
                Console.Write(", ");
                Console.Write(tmp.DateOfBirth.ToString("yyyy-MMM-dd",  CultureInfo.InvariantCulture));
                Console.Write(", height: ");
                Console.Write(tmp.Height);
                Console.Write(", weight ");
                Console.Write(tmp.Weight);
                Console.Write(", driving license category: ");
                Console.WriteLine(tmp.DrivingLicenseCategory);
            }
        }
    }
}