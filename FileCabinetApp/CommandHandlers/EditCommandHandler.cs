using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.ValidationRules;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Handler of edit command.
    /// </summary>
    public class EditCommandHandler : ServiceCommandHandlerBase
    {
        private readonly ValidationRules.ValidationTypes validationRules = ValidationRulesReader.ReadRules("validation-rules.json");
        private readonly bool isCustom;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Service provided.</param>
        /// <param name="custom">validation custom flag.</param>
        public EditCommandHandler(IFileCabinetService fileCabinetService, bool custom)
        {
            this.fileCabinetService = fileCabinetService;
            this.isCustom = custom;
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

            if (request.Command == "edit")
            {
                this.Edit(request.Parameters);
            }
            else
            {
                this.nextHandler.Handle(request);
            }
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

        private void Edit(string parameters)
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
                this.fileCabinetService.EditRecord(id, new Arguments("ex", "ex", DateTime.Today, 0, 0, 'A'));
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

            this.EnterParameters(out firstName, out lastName, out dateTime, out height, out weight, out drivingLicenseCategory);

            try
            {
                this.fileCabinetService.EditRecord(id, new Arguments(firstName, lastName, dateTime, height, weight, drivingLicenseCategory));
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Data is invalid");
                return;
            }

            Console.WriteLine($"Record #" + id + "is updated");
        }

        private void EnterParameters(out string firstName, out string lastName, out DateTime dateOfBirth, out short height, out decimal weight, out char drivingLicenseCategory)
        {
            Console.Write("First name: ");
            firstName = ReadInput(this.StringConverter, this.FirstNameValidator);

            Console.Write("Last Name: ");
            lastName = ReadInput(this.StringConverter, this.LastNameValidator);

            Console.Write("Date of birth: ");
            dateOfBirth = ReadInput(this.DateTimeConverter, this.DateOfBirthValidator);

            Console.Write("Height: ");
            height = ReadInput(this.ShortConverter, this.HeightValidator);

            Console.Write("Weight: ");
            weight = ReadInput(this.DecimalConverter, this.WeightValidator);

            Console.Write("Driving license category: ");
            drivingLicenseCategory = ReadInput(this.CharConverter, this.LicenseCategoryValidator);
        }

        private Tuple<bool, string, string> StringConverter(string input)
        {
            return new Tuple<bool, string, string>(true, string.Empty, input);
        }

        private Tuple<bool, string, DateTime> DateTimeConverter(string input)
        {
            DateTime dateOfBirth;
            bool success = DateTime.TryParse(input, out dateOfBirth);
            return new Tuple<bool, string, DateTime>(success, string.Empty, dateOfBirth);
        }

        private Tuple<bool, string, short> ShortConverter(string input)
        {
            short height;
            bool success = short.TryParse(input, out height);
            return new Tuple<bool, string, short>(success, string.Empty, height);
        }

        private Tuple<bool, string, decimal> DecimalConverter(string input)
        {
            decimal weight;
            bool success = decimal.TryParse(input, out weight);
            return new Tuple<bool, string, decimal>(success, string.Empty, weight);
        }

        private Tuple<bool, string, char> CharConverter(string input)
        {
            char symbol;
            bool success = char.TryParse(input, out symbol);
            return new Tuple<bool, string, char>(success, string.Empty, symbol);
        }

        private Tuple<bool, string> FirstNameValidator(string firstName)
        {
            bool result = true;
            if (this.isCustom)
            {
                if ((firstName.Length < this.validationRules.Custom.FirstName.MinValue) || (firstName.Length > this.validationRules.Custom.FirstName.MaxValue) || string.IsNullOrWhiteSpace(firstName))
                {
                    result = false;
                }
            }
            else
            {
                if ((firstName.Length < this.validationRules.Default.FirstName.MinValue) || (firstName.Length > this.validationRules.Default.FirstName.MaxValue) || string.IsNullOrWhiteSpace(firstName))
                {
                    result = false;
                }
            }

            return new Tuple<bool, string>(result, string.Empty);
        }

        private Tuple<bool, string> LastNameValidator(string lastName)
        {
            bool result = true;
            if (this.isCustom)
            {
                if ((lastName.Length < this.validationRules.Custom.LastName.MinValue) || (lastName.Length > this.validationRules.Custom.LastName.MaxValue) || string.IsNullOrWhiteSpace(lastName))
                {
                    result = false;
                }
            }
            else
            {
                if ((lastName.Length < this.validationRules.Default.LastName.MinValue) || (lastName.Length > this.validationRules.Default.LastName.MaxValue) || string.IsNullOrWhiteSpace(lastName))
                {
                    result = false;
                }
            }

            return new Tuple<bool, string>(result, string.Empty);
        }

        private Tuple<bool, string> DateOfBirthValidator(DateTime dateOfBirth)
        {
            bool result = true;
            if (this.isCustom)
            {
                DateTime minDate;
                bool success = DateTime.TryParse(this.validationRules.Custom.DateOfBirth.Min, out minDate);
                if (!success)
                {
                    return null;
                }

                DateTime maxDate;
                success = DateTime.TryParse(this.validationRules.Custom.DateOfBirth.Max, out maxDate);

                if (!success)
                {
                    return null;
                }

                if ((DateTime.Compare(dateOfBirth, minDate) > 0) || (DateTime.Compare(dateOfBirth, maxDate) < 0))
                {
                    result = false;
                }
            }
            else
            {
                DateTime minDate;
                bool success = DateTime.TryParse(this.validationRules.Default.DateOfBirth.Min, out minDate);
                if (!success)
                {
                    return null;
                }

                DateTime maxDate;
                success = DateTime.TryParse(this.validationRules.Default.DateOfBirth.Max, out maxDate);
                if (!success)
                {
                    return null;
                }

                if ((DateTime.Compare(dateOfBirth, maxDate) > 0) || (DateTime.Compare(dateOfBirth, minDate) < 0))
                {
                    result = false;
                }
            }

            return new Tuple<bool, string>(result, string.Empty);
        }

        private Tuple<bool, string> HeightValidator(short height)
        {
            bool result = true;
            if (this.isCustom)
            {
                if (height < this.validationRules.Custom.Height.MinValue || height > this.validationRules.Custom.Height.MaxValue)
                {
                    result = false;
                }
            }
            else
            {
                if (height < this.validationRules.Default.Height.MinValue || height > this.validationRules.Default.Height.MaxValue)
                {
                    result = false;
                }
            }

            return new Tuple<bool, string>(result, string.Empty);
        }

        private Tuple<bool, string> WeightValidator(decimal weight)
        {
            bool result = true;
            if (this.isCustom)
            {
                if (weight < this.validationRules.Custom.Weight.MinValue || weight > this.validationRules.Custom.Weight.MaxValue)
                {
                    result = false;
                }
            }
            else
            {
                if (weight < this.validationRules.Default.Weight.MinValue || weight > this.validationRules.Default.Weight.MaxValue)
                {
                    result = false;
                }
            }

            return new Tuple<bool, string>(result, string.Empty);
        }

        private Tuple<bool, string> LicenseCategoryValidator(char drivingLicenseCategory)
        {
            bool result = true;
            if (this.isCustom)
            {
                bool isValid = false;
                foreach (var category in this.validationRules.Custom.DrivingLicenseCategory.Values)
                {
                    if (drivingLicenseCategory == category[0])
                    {
                        isValid = true;
                    }
                }

                result = isValid;
            }
            else
            {
                bool isValid = false;
                foreach (var category in this.validationRules.Default.DrivingLicenseCategory.Values)
                {
                    if (drivingLicenseCategory == category[0])
                    {
                        isValid = true;
                    }
                }

                result = isValid;
            }

            return new Tuple<bool, string>(result, string.Empty);
        }
    }
}
