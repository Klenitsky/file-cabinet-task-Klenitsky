using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Stores a list of records.
    /// </summary>
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly IRecordValidator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetService"/> class.
        /// Creates a new Record.
        /// </summary>
        /// <param name="validator">Validator probidet.</param>
        public FileCabinetService(IRecordValidator validator)
        {
            this.validator = validator;
        }

        /// <summary>
        /// Creates a new Record.
        /// </summary>
        /// <param name="arguments">Properties of the record.</param>
        /// <returns>New record's Id.</returns>
        /// <exception cref="ArgumentNullException"> Parameters are null.</exception>
        /// <exception cref="ArgumentException">One of the parameters is not valid.</exception>
        public int CreateRecord(Arguments arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            this.validator.ValidateParameters(arguments);
            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = arguments.FirstName,
                LastName = arguments.LastName,
                DateOfBirth = arguments.DateOfBirth,
                Height = arguments.Height,
                Weight = arguments.Weight,
                DrivingLicenseCategory = arguments.DrivingLicenseCategory,
            };

            if (!this.firstNameDictionary.ContainsKey(record.FirstName))
            {
                this.firstNameDictionary.Add(record.FirstName, new List<FileCabinetRecord>());
            }

            this.firstNameDictionary[record.FirstName].Add(record);

            if (!this.lastNameDictionary.ContainsKey(record.LastName))
            {
                this.lastNameDictionary.Add(record.LastName, new List<FileCabinetRecord>());
            }

            this.lastNameDictionary[record.LastName].Add(record);

            if (!this.dateOfBirthDictionary.ContainsKey(record.DateOfBirth.ToString(CultureInfo.CurrentCulture)))
            {
                this.dateOfBirthDictionary.Add(record.DateOfBirth.ToString(CultureInfo.CurrentCulture), new List<FileCabinetRecord>());
            }

            this.dateOfBirthDictionary[record.DateOfBirth.ToString(CultureInfo.CurrentCulture)].Add(record);

            this.list.Add(record);

            return record.Id;
        }

        /// <summary>
        /// Gets a copy of the list.
        /// </summary>
        /// <returns>Array of records.</returns>
        public FileCabinetRecord[] GetRecords()
        {
            FileCabinetRecord[] result = new FileCabinetRecord[this.list.Count];
            this.list.CopyTo(result);
            return result;
        }

        /// <summary>
        /// Gets a number of records.
        /// </summary>
        /// <returns>Number of records.</returns>
        public int GetStat()
        {
            return this.list.Count;
        }

        /// <summary>
        /// Edits an existing record.
        /// </summary>
        /// <param name="id">The ID of a record.</param>
        /// <param name="arguments">Properties of the record.</param>
        /// <exception cref="ArgumentNullException">One of the parameters is null.</exception>
        /// <exception cref="ArgumentException">One of the parameters is not valid.</exception>
        public void EditRecord(int id, Arguments arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            bool isValid = false;
            foreach (FileCabinetRecord record in this.list)
            {
                if (record.Id == id)
                {
                    isValid = true;
                    this.validator.ValidateParameters(arguments);

                    this.firstNameDictionary[record.FirstName].Remove(record);
                    this.lastNameDictionary[record.LastName].Remove(record);
                    this.dateOfBirthDictionary[record.DateOfBirth.ToString(CultureInfo.CurrentCulture)].Remove(record);
                    record.FirstName = arguments.FirstName;
                    record.LastName = arguments.LastName;
                    record.DateOfBirth = arguments.DateOfBirth;
                    record.Height = arguments.Height;
                    record.Weight = arguments.Weight;
                    record.DrivingLicenseCategory = arguments.DrivingLicenseCategory;

                    if (!this.firstNameDictionary.ContainsKey(record.FirstName))
                    {
                        this.firstNameDictionary.Add(record.FirstName, new List<FileCabinetRecord>());
                    }

                    this.firstNameDictionary[record.FirstName].Add(record);

                    if (!this.lastNameDictionary.ContainsKey(record.LastName))
                    {
                        this.lastNameDictionary.Add(record.LastName, new List<FileCabinetRecord>());
                    }

                    this.lastNameDictionary[record.LastName].Add(record);

                    if (!this.dateOfBirthDictionary.ContainsKey(record.DateOfBirth.ToString(CultureInfo.CurrentCulture)))
                    {
                        this.dateOfBirthDictionary.Add(record.DateOfBirth.ToString(CultureInfo.CurrentCulture), new List<FileCabinetRecord>());
                    }

                    this.dateOfBirthDictionary[record.DateOfBirth.ToString(CultureInfo.CurrentCulture)].Add(record);
                }
            }

            if (!isValid)
            {
                throw new ArgumentException("There is no such Id.", nameof(id));
            }
        }

        /// <summary>
        /// Finds all records with given firstname.
        /// </summary>
        /// <param name="firstName">The first name of the person.</param>
        /// <returns>A list of records found.</returns>
        /// <exception cref="ArgumentNullException">String firstName is null.</exception>
        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentNullException(nameof(firstName));
            }

            return this.firstNameDictionary[firstName].ToArray();
        }

        /// <summary>
        /// Finds all records with given lastname.
        /// </summary>
        /// <param name="lastName">The last name of the person.</param>
        /// <returns>A list of records found.</returns>
        /// <exception cref="ArgumentNullException">String firstName is null.</exception>
        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            if (string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentNullException(nameof(lastName));
            }

            return this.lastNameDictionary[lastName].ToArray();
        }

        /// <summary>
        /// Finds all records with given date of Birth.
        /// </summary>
        /// <param name="date">The date of birth of the person.</param>
        /// <returns>A list of records found.</returns>
        public FileCabinetRecord[] FindByDateOfBirth(DateTime date)
        {
            return this.dateOfBirthDictionary[date.ToString(CultureInfo.CurrentCulture)].ToArray();
        }
    }
}
