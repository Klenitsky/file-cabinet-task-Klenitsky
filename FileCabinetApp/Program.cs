using System;
using System.Globalization;
using System.Text;

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
                while ((firstname.Length < 2) || (firstname.Length > 60) || string.IsNullOrWhiteSpace(firstname))
                {
                    Console.Write("First name is invalid, try again: ");
                    Console.Write("First Name: ");
                    firstname = Console.ReadLine();
                }

                Console.Write("Last Name: ");
                string lastname = Console.ReadLine();
                while ((lastname.Length < 2) || (lastname.Length > 60) || string.IsNullOrWhiteSpace(lastname))
                {
                     Console.WriteLine("Last name is invalid, try again: ");
                     Console.Write("Last Name: ");
                     lastname = Console.ReadLine();
                }

                Console.Write("Date of birth: ");
                string date = Console.ReadLine();
                DateTime dateTime;
                bool success = DateTime.TryParse(date, out dateTime);
                while (!success || (DateTime.Compare(dateTime, DateTime.Today) > 0) || DateTime.Compare(dateTime, new DateTime(1950, 1, 1)) < 0)
                {
                    Console.WriteLine("Date  is invalid, try again: ");
                    Console.Write("Date: ");
                    date = Console.ReadLine();
                    success = DateTime.TryParse(date, out dateTime);
                }

                Console.Write("Height: ");
                string tmp = Console.ReadLine();
                short height;
                success = short.TryParse(tmp, out height);
                while (!success || height < 0 || height > 250)
                {
                     Console.WriteLine("Height is invalid, try again: ");
                     Console.Write("Height: ");
                     tmp = Console.ReadLine();
                     success = short.TryParse(tmp, out height);
                }

                Console.Write("Weight: ");
                tmp = Console.ReadLine();
                decimal weight;
                success = decimal.TryParse(tmp, out weight);
                while (!success || weight < 0 || weight > 180)
                {
                    Console.WriteLine("Weight is invalid, try again: ");
                    Console.Write("Weight: ");
                    tmp = Console.ReadLine();
                    success = decimal.TryParse(tmp, out weight);
                }

                Console.Write("Driving license category: ");
                tmp = Console.ReadLine();
                char category;
                success = char.TryParse(tmp, out category);
                while (!success || ((category != 'A') && (category != 'B') && (category != 'C') && (category != 'D')))
                {
                Console.WriteLine("License category is invalid, try again: ");
                Console.Write("Driving license category: ");
                tmp = Console.ReadLine();
                success = char.TryParse(tmp, out category);
                }

                try
                {
                    Program.fileCabinetService.CreateRecord(firstname, lastname, dateTime, height, weight, category);
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
            FileCabinetRecord[] tmpArray = Program.fileCabinetService.GetRecords();
            foreach (FileCabinetRecord tmp in tmpArray)
            {
                Console.Write($"#" + tmp.Id + ", ");
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

        private static void Edit(string parameters)
        {
            int id;
            bool success = int.TryParse(parameters, out id);
            if (!success)
            {
                Console.WriteLine("Not a number");
                return;
            }

            try
            {
                Program.fileCabinetService.EditRecord(id, "tmp", "tmp", DateTime.Today, 0, 0, 'A');
            }
            catch (ArgumentException)
            {
                Console.WriteLine($"#{1} record is not found", id);
            }

            Console.Write("First Name: ");
            string firstname = Console.ReadLine();
            while ((firstname.Length < 2) || (firstname.Length > 60) || string.IsNullOrWhiteSpace(firstname))
            {
                Console.Write("First name is invalid, try again: ");
                Console.Write("First Name: ");
                firstname = Console.ReadLine();
            }

            Console.Write("Last Name: ");
            string lastname = Console.ReadLine();
            while ((lastname.Length < 2) || (lastname.Length > 60) || string.IsNullOrWhiteSpace(lastname))
            {
                Console.WriteLine("Last name is invalid, try again: ");
                Console.Write("Last Name: ");
                lastname = Console.ReadLine();
            }

            Console.Write("Date of birth: ");
            string date = Console.ReadLine();
            DateTime dateTime;
            success = DateTime.TryParse(date, out dateTime);
            while (!success || (DateTime.Compare(dateTime, DateTime.Today) > 0) || DateTime.Compare(dateTime, new DateTime(1950, 1, 1)) < 0)
            {
                Console.WriteLine("Date  is invalid, try again: ");
                Console.Write("Date: ");
                date = Console.ReadLine();
                success = DateTime.TryParse(date, out dateTime);
            }

            Console.Write("Height: ");
            string tmp = Console.ReadLine();
            short height;
            success = short.TryParse(tmp, out height);
            while (!success || height < 0 || height > 250)
            {
                Console.WriteLine("Height is invalid, try again: ");
                Console.Write("Height: ");
                tmp = Console.ReadLine();
                success = short.TryParse(tmp, out height);
            }

            Console.Write("Weight: ");
            tmp = Console.ReadLine();
            decimal weight;
            success = decimal.TryParse(tmp, out weight);
            while (!success || weight < 0 || weight > 180)
            {
                Console.WriteLine("Weight is invalid, try again: ");
                Console.Write("Weight: ");
                tmp = Console.ReadLine();
                success = decimal.TryParse(tmp, out weight);
            }

            Console.Write("Driving license category: ");
            tmp = Console.ReadLine();
            char category;
            success = char.TryParse(tmp, out category);
            while (!success || ((category != 'A') && (category != 'B') && (category != 'C') && (category != 'D')))
            {
                Console.WriteLine("License category is invalid, try again: ");
                Console.Write("Driving license category: ");
                tmp = Console.ReadLine();
                success = char.TryParse(tmp, out category);
            }

            try
            {
                Program.fileCabinetService.EditRecord(id, firstname, lastname, dateTime, height, weight, category);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Data is invalid");
                return;
            }

            Console.WriteLine($"Record #{1} is updated", id);
        }

        private static void Find(string parameters)
        {
            int index = parameters.IndexOf(' ', StringComparison.InvariantCulture);
            StringBuilder property = new StringBuilder(parameters, 0, index, 255);
            FileCabinetRecord[] result = Array.Empty<FileCabinetRecord>();
            if (property.ToString().ToLower(CultureInfo.CurrentCulture) == "firstname".ToLower(CultureInfo.CurrentCulture))
            {
                StringBuilder name = new StringBuilder(parameters, index + 2, parameters.Length - index - 3, 255);
                result = Program.fileCabinetService.FindByFirstName(name.ToString());
            }

            if (property.ToString().ToLower(CultureInfo.CurrentCulture) == "lastname".ToLower(CultureInfo.CurrentCulture))
            {
                StringBuilder name = new StringBuilder(parameters, index + 2, parameters.Length - index - 3, 255);
                result = Program.fileCabinetService.FindByLastName(name.ToString());
            }

            if (result.Length == 0)
            {
                Console.WriteLine("No elements with such property");
            }

            foreach (FileCabinetRecord tmp in result)
            {
                Console.Write($"#" + tmp.Id + ", ");
                Console.Write(tmp.FirstName);
                Console.Write(", ");
                Console.Write(tmp.LastName);
                Console.Write(", ");
                Console.Write(tmp.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture));
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