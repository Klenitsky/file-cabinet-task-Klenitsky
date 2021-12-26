using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Handler of all commands.
    /// </summary>
    public class CommandHandler : CommandHandlerBase
    {
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "prints the statistics", "The 'stat' command prints the statistics." },
            new string[] { "create", "creates a new record", "The 'create' command creates a new record." },
            new string[] { "list", "returns a list of records", "The 'list' command returns a list of recordscre." },
            new string[] { "edit", "edits the record with entered id.", "The 'edit' command edits the record with entered id" },
            new string[] { "find", "finds the record according to the parameters.", "The 'edit' command finds the record according to the parameters." },
            new string[] { "export", "exports the data into file", "The 'export' command exports the data." },
            new string[] { "import", "imports the data from file", "The 'import' command imports the data." },
            new string[] { "remove", "removes the record with selected id", "The 'remove' command removes records." },
            new string[] { "purge", "purges deleted records", "The 'purge' command purges deleted records." },
        };

        /// <summary>
        /// Handles the request.
        /// </summary>
        /// <param name="request">Request provided.</param>
        public override void Handle(AppCommandRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            switch (request.Command)
            {
                case "help":
                    PrintHelp(request.Parameters);
                    break;

                case "exit":
                    Exit(request.Parameters);
                    break;
                case "stat":
                    Stat(request.Parameters);
                    break;
                case "create":
                    Create(request.Parameters);
                    break;
                case "list":
                    List(request.Parameters);
                    break;
                case "edit":
                    Edit(request.Parameters);
                    break;
                case "find":
                    Find(request.Parameters);
                    break;
                case "export":
                    Export(request.Parameters);
                    break;
                case "import":
                    Import(request.Parameters);
                    break;
                case "remove":
                    Remove(request.Parameters);
                    break;
                case "purge":
                    Purge(request.Parameters);
                    break;

                default:
                    throw new ArgumentException("Invalid command", nameof(request));
            }
        }

        private static void PrintHelp(string parameters)
    {
        if (!string.IsNullOrEmpty(parameters))
        {
            var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
            if (index >= 0)
            {
                Console.WriteLine(helpMessages[index][ExplanationHelpIndex]);
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
                Console.WriteLine("\t{0}\t- {1}", helpMessage[CommandHelpIndex], helpMessage[DescriptionHelpIndex]);
            }
        }

        Console.WriteLine();
    }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            Program.isRunning = false;
            Program.fileStream.Close();
        }

        private static void Stat(string parameters)
        {
            var recordsCount = Program.fileCabinetService.GetStat();
            int deletedCount = Program.fileCabinetService.GetDeletedStat();
            Console.WriteLine($"{recordsCount} record(s).{deletedCount} records were deleted");
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

            Console.WriteLine($"\nRecord #" + Program.fileCabinetService.GetID().ToString(CultureInfo.InvariantCulture) + " created ");
        }

        private static void List(string parameters)
        {
            IReadOnlyCollection<FileCabinetRecord> arrayOfRecords = Program.fileCabinetService.GetRecords();
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
            IReadOnlyCollection<FileCabinetRecord> result = Array.Empty<FileCabinetRecord>();

            if (property.ToString().ToLower(CultureInfo.CurrentCulture) == "firstname".ToLower(CultureInfo.CurrentCulture))
            {
                StringBuilder name = new StringBuilder(parameters, index + 1, parameters.Length - (index + 1), 255);
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
                StringBuilder name = new StringBuilder(parameters, index + 1, parameters.Length - index - 1, 255);
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
                StringBuilder date = new StringBuilder(parameters, index + 1, parameters.Length - index - 1, 255);
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

            if (result.Count == 0)
            {
                Console.WriteLine("No elements with such property");
            }

            foreach (FileCabinetRecord record in result)
            {
                Console.WriteLine(record.ToString());
            }
        }

        private static void Export(string parameters)
        {
            if (parameters[0..3] == "csv")
            {
                string filename = parameters[4..];
                try
                {
                    if (File.Exists(filename))
                    {
                        Console.WriteLine("File is exist - rewrite " + filename + "?[Y/n]");
                        if (Console.ReadLine().ToUpperInvariant() == "y".ToUpperInvariant())
                        {
                            StreamWriter writer = new StreamWriter(filename);
                            Program.fileCabinetService.MakeSnapshot().SaveToCSV(writer);
                            writer.Close();
                            Console.WriteLine("All records are exported to file " + filename + ".");
                        }
                    }
                    else
                    {
                        StreamWriter writer = new StreamWriter(filename);
                        Program.fileCabinetService.MakeSnapshot().SaveToCSV(writer);
                        writer.Close();
                        Console.WriteLine("All records are exported to file " + filename + ".");
                    }
                }
                catch (DirectoryNotFoundException)
                {
                    Console.WriteLine("Export failed: can't open file " + filename + ".");
                }
            }

            if (parameters[0..3] == "xml")
            {
                string filename = parameters[4..];
                try
                {
                    if (File.Exists(filename))
                    {
                        Console.WriteLine("File is exist - rewrite " + filename + "?[Y/n]");
                        if (Console.ReadLine().ToUpperInvariant() == "y".ToUpperInvariant())
                        {
                            StreamWriter writer = new StreamWriter(filename);
                            Program.fileCabinetService.MakeSnapshot().SaveToXml(writer);
                            writer.Close();
                            Console.WriteLine("All records are exported to file " + filename + ".");
                        }
                    }
                    else
                    {
                        StreamWriter writer = new StreamWriter(filename);
                        Program.fileCabinetService.MakeSnapshot().SaveToXml(writer);
                        writer.Close();
                        Console.WriteLine("All records are exported to file " + filename + ".");
                    }
                }
                catch (DirectoryNotFoundException)
                {
                    Console.WriteLine("Export failed: can't open file " + filename + ".");
                }
            }
        }

        private static void Import(string parameters)
        {
            string[] parametersArray = parameters.Split(' ');
            if (parametersArray[0] == "csv")
            {
                StreamReader file = null;
                try
                {
                    file = new StreamReader(parametersArray[1]);
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("Import error: file  " + parametersArray[1] + " is not exist.");
                }

                FileCabinetServiceSnapshot snapshot = new FileCabinetServiceSnapshot(new List<FileCabinetRecord>());
                snapshot.LoadFromCsv(file);
                Program.fileCabinetService.Restore(snapshot);
                file.Close();
                Console.WriteLine(snapshot.Records.Count + " records were imported from " + parametersArray[1]);
            }

            if (parametersArray[0] == "xml")
            {
                StreamReader file = null;
                try
                {
                    file = new StreamReader(parametersArray[1]);
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("Import error: file  " + parametersArray[1] + " is not exist.");
                }

                FileCabinetServiceSnapshot snapshot = new FileCabinetServiceSnapshot(new List<FileCabinetRecord>());
                snapshot.LoadFromXml(file);
                Program.fileCabinetService.Restore(snapshot);
                file.Close();
                Console.WriteLine(snapshot.Records.Count + " records were imported from " + parametersArray[1]);
            }
        }

        private static void Remove(string parameters)
        {
            int removeId;
            bool success = int.TryParse(parameters, out removeId);
            if (!success)
            {
                Console.WriteLine("Error occured: Invalid Id");
            }

            success = Program.fileCabinetService.Remove(removeId);
            if (success)
            {
                Console.WriteLine("Record #" + removeId + " is removed.");
            }
            else
            {
                Console.WriteLine("Record #" + removeId + " doesn't exist.");
            }
        }

        private static void Purge(string parameters)
        {
            int stat = Program.fileCabinetService.GetStat();
            int result = Program.fileCabinetService.Purge();
            Console.WriteLine("Data file processing is completed:" + result + " of " + stat + "  records were purged.");
        }

        private static void EnterParameters(out string firstName, out string lastName, out DateTime dateOfBirth, out short height, out decimal weight, out char drivingLicenseCategory)
        {
            Console.Write("First name: ");
            firstName = ReadInput(StringConverter, FirstNameValidator);

            Console.Write("Last Name: ");
            lastName = ReadInput(StringConverter, LastNameValidator);

            Console.Write("Date of birth: ");
            dateOfBirth = ReadInput(DateTimeConverter, DateOfBirthValidator);

            Console.Write("Height: ");
            height = ReadInput(ShortConverter, HeightValidator);

            Console.Write("Weight: ");
            weight = ReadInput(DecimalConverter, WeightValidator);

            Console.Write("Driving license category: ");
            drivingLicenseCategory = ReadInput(CharConverter, LicenseCategoryValidator);
        }

        private static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }

        private static Tuple<bool, string, string> StringConverter(string input)
        {
            return new Tuple<bool, string, string>(true, string.Empty, input);
        }

        private static Tuple<bool, string, DateTime> DateTimeConverter(string input)
        {
            DateTime dateOfBirth;
            bool success = DateTime.TryParse(input, out dateOfBirth);
            return new Tuple<bool, string, DateTime>(success, string.Empty, dateOfBirth);
        }

        private static Tuple<bool, string, short> ShortConverter(string input)
        {
            short height;
            bool success = short.TryParse(input, out height);
            return new Tuple<bool, string, short>(success, string.Empty, height);
        }

        private static Tuple<bool, string, decimal> DecimalConverter(string input)
        {
            decimal weight;
            bool success = decimal.TryParse(input, out weight);
            return new Tuple<bool, string, decimal>(success, string.Empty, weight);
        }

        private static Tuple<bool, string, char> CharConverter(string input)
        {
            char symbol;
            bool success = char.TryParse(input, out symbol);
            return new Tuple<bool, string, char>(success, string.Empty, symbol);
        }

        private static Tuple<bool, string> FirstNameValidator(string firstName)
        {
            bool result = true;
            if (Program.isCustom)
            {
                if ((firstName.Length < 1) || (firstName.Length > 8) || string.IsNullOrWhiteSpace(firstName))
                {
                    result = false;
                }
            }
            else
            {
                if ((firstName.Length < 2) || (firstName.Length > 60) || string.IsNullOrWhiteSpace(firstName))
                {
                    result = false;
                }
            }

            return new Tuple<bool, string>(result, string.Empty);
        }

        private static Tuple<bool, string> LastNameValidator(string firstName)
        {
            bool result = true;
            if (Program.isCustom)
            {
                if ((firstName.Length < 1) || (firstName.Length > 7) || string.IsNullOrWhiteSpace(firstName))
                {
                    result = false;
                }
            }
            else
            {
                if ((firstName.Length < 2) || (firstName.Length > 60) || string.IsNullOrWhiteSpace(firstName))
                {
                    result = false;
                }
            }

            return new Tuple<bool, string>(result, string.Empty);
        }

        private static Tuple<bool, string> DateOfBirthValidator(DateTime dateOfBirth)
        {
            bool result = true;
            if (Program.isCustom)
            {
                if ((DateTime.Compare(dateOfBirth, DateTime.Today) > 0) || (DateTime.Compare(dateOfBirth, new DateTime(1800, 1, 1)) < 0))
                {
                    result = false;
                }
            }
            else
            {
                if ((DateTime.Compare(dateOfBirth, DateTime.Today) > 0) || (DateTime.Compare(dateOfBirth, new DateTime(1950, 1, 1)) < 0))
                {
                    result = false;
                }
            }

            return new Tuple<bool, string>(result, string.Empty);
        }

        private static Tuple<bool, string> HeightValidator(short height)
        {
            bool result = true;
            if (Program.isCustom)
            {
                if (height < 10 || height > 20)
                {
                    result = false;
                }
            }
            else
            {
                if (height < 0 || height > 250)
                {
                    result = false;
                }
            }

            return new Tuple<bool, string>(result, string.Empty);
        }

        private static Tuple<bool, string> WeightValidator(decimal weight)
        {
            bool result = true;
            if (Program.isCustom)
            {
                if (weight < 10 || weight > 20)
                {
                    result = false;
                }
            }
            else
            {
                if (weight < 0 || weight > 180)
                {
                    result = false;
                }
            }

            return new Tuple<bool, string>(result, string.Empty);
        }

        private static Tuple<bool, string> LicenseCategoryValidator(char drivingLicenseCategory)
        {
            bool result = true;
            if (Program.isCustom)
            {
                if ((drivingLicenseCategory != 'A') && (drivingLicenseCategory != 'B') && (drivingLicenseCategory != 'C') && (drivingLicenseCategory != 'Q'))
                {
                    result = false;
                }
            }
            else
            {
                if ((drivingLicenseCategory != 'A') && (drivingLicenseCategory != 'B') && (drivingLicenseCategory != 'C') && (drivingLicenseCategory != 'D'))
                {
                    result = false;
                }
            }

            return new Tuple<bool, string>(result, string.Empty);
        }
    }
}