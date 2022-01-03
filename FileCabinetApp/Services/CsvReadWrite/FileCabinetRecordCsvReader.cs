using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    /// <summary>
    /// Reads the record from csv file.
    /// </summary>
    public class FileCabinetRecordCsvReader
    {
        private const int IdIndex = 0;
        private const int FirstNameIndex = 1;
        private const int LastNameIndex = 2;
        private const int DateOfBirthIndex = 3;
        private const int HeightIndex = 4;
        private const int WeightIndex = 5;
        private const int DrivingLicenseCategoryIndex = 6;

        private readonly StreamReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvReader"/> class.
        /// </summary>
        /// <param name="reader"> Reader provided.</param>
        public FileCabinetRecordCsvReader(StreamReader reader)
        {
            this.reader = reader;
        }

        /// <summary>
        /// Reads records from the file.
        /// </summary>
        /// <returns> List of records.</returns>
        public IList<FileCabinetRecord> ReadAll()
        {
            IList<FileCabinetRecord> result = new List<FileCabinetRecord>();
            bool success = true;
            this.reader.ReadLine();
            while (!this.reader.EndOfStream)
            {
                string recordString = this.reader.ReadLine();
                string[] recordData = recordString.Split(',');
                int id;
                string firstName;
                string lastName;
                DateTime dateOfBirth;
                short height;
                decimal weight;
                char drivingLicenseCategory;
                if (!int.TryParse(recordData[IdIndex], out id))
                {
                    success = false;
                }

                firstName = recordData[FirstNameIndex];
                lastName = recordData[LastNameIndex];

                if (!DateTime.TryParse(recordData[DateOfBirthIndex], out dateOfBirth))
                {
                    success = false;
                }

                if (!short.TryParse(recordData[HeightIndex], out height))
                {
                    success = false;
                }

                if (!decimal.TryParse(recordData[WeightIndex], out weight))
                {
                    success = false;
                }

                if (!char.TryParse(recordData[DrivingLicenseCategoryIndex], out drivingLicenseCategory))
                {
                    success = false;
                }

                if (!success)
                {
                    Console.WriteLine("Error occured.");
                    continue;
                }

                Arguments arguments = new Arguments(firstName, lastName, dateOfBirth, height, weight, drivingLicenseCategory);
                IRecordValidator validator = new ValidatorBuilder().CreateDefault();
                try
                {
                    validator.ValidateParameters(arguments);
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Error occured.");
                    continue;
                }

                FileCabinetRecord record = new FileCabinetRecord
                {
                    Id = id,
                    FirstName = arguments.FirstName,
                    LastName = arguments.LastName,
                    DateOfBirth = arguments.DateOfBirth,
                    Height = arguments.Height,
                    Weight = arguments.Weight,
                    DrivingLicenseCategory = arguments.DrivingLicenseCategory,
                };
                result.Add(record);
            }

            return result;
        }
    }
}
