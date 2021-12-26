using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Handler of Create command.
    /// </summary>
    public class CreateCommandHandler : CommandHandlerBase
    {
        private static IFileCabinetService fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Service provided.</param>
        public CreateCommandHandler(IFileCabinetService fileCabinetService)
        {
            CreateCommandHandler.fileCabinetService = fileCabinetService;
        }

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

            if (request.Command == "create")
            {
                Create(request.Parameters);
            }
            else
            {
                this.nextHandler.Handle(request);
            }
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
                fileCabinetService.CreateRecord(new Arguments(firstName, lastName, dateTime, height, weight, drivingLicenseCategory));
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Data is invalid");
                return;
            }

            Console.WriteLine($"\nRecord #" + fileCabinetService.GetID().ToString(CultureInfo.InvariantCulture) + " created ");
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
