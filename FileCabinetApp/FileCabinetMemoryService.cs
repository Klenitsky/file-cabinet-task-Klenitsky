using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using FileCabinetApp.Iterators;
using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    /// <summary>
    /// Stores a list of records.
    /// </summary>
    public class FileCabinetMemoryService : IFileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly IRecordValidator validator;
        private int id = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetMemoryService"/> class.
        /// Creates a new Record.
        /// </summary>
        /// <param name="validator">Validator probidet.</param>
        public FileCabinetMemoryService(IRecordValidator validator)
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
            if (this.list.Count > 0)
            {
                this.id = this.list[this.list.Count - 1].Id;
            }

            var record = new FileCabinetRecord
            {
                Id = this.id,
                FirstName = arguments.FirstName,
                LastName = arguments.LastName,
                DateOfBirth = arguments.DateOfBirth,
                Height = arguments.Height,
                Weight = arguments.Weight,
                DrivingLicenseCategory = arguments.DrivingLicenseCategory,
            };

            this.id++;

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
        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
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
        public IRecordIterator FindByFirstName(string firstName)
        {
            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentNullException(nameof(firstName));
            }

            MemoryIterator iterator = new MemoryIterator(this.firstNameDictionary[firstName]);
            return iterator;
        }

        /// <summary>
        /// Finds all records with given lastname.
        /// </summary>
        /// <param name="lastName">The last name of the person.</param>
        /// <returns>A list of records found.</returns>
        /// <exception cref="ArgumentNullException">String firstName is null.</exception>
        public IRecordIterator FindByLastName(string lastName)
        {
            if (string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentNullException(nameof(lastName));
            }

            MemoryIterator iterator = new MemoryIterator(this.lastNameDictionary[lastName]);
            return iterator;
        }

        /// <summary>
        /// Finds all records with given date of Birth.
        /// </summary>
        /// <param name="dateTime">The date of birth of the person.</param>
        /// <returns>A list of records found.</returns>
        public IRecordIterator FindByDateOfBirth(DateTime dateTime)
        {
            MemoryIterator iterator = new MemoryIterator(this.dateOfBirthDictionary[dateTime.ToString(CultureInfo.CurrentCulture)]);
            return iterator;
        }

        /// <summary>
        /// Generates snapshot of the service.
        /// </summary>
        /// <returns>A snapshot of this service.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(this.list);
        }

        /// <summary>
        /// Adds records loaded from file.
        /// </summary>
        /// <param name="snapshot">Properties of the record.</param>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            if (snapshot == null)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }

            foreach (FileCabinetRecord record in snapshot.Records)
            {
                if (record.Id < this.list.Count)
                {
                    this.EditRecord(record.Id, new Arguments(record.FirstName, record.LastName, record.DateOfBirth, record.Height, record.Weight, record.DrivingLicenseCategory));
                }
                else
                {
                    this.CreateRecord(new Arguments(record.FirstName, record.LastName, record.DateOfBirth, record.Height, record.Weight, record.DrivingLicenseCategory));
                    this.list[this.list.Count - 1].Id = record.Id;
                }
            }
        }

        /// <summary>
        /// Removes a record.
        /// </summary>
        /// <param name="id">Id of a record to remove.</param>
        /// <returns>A bool result of removing.</returns>
        public bool Remove(int id)
        {
            bool hasFound = false;
            for (int i = 0; i < this.list.Count; i++)
            {
                FileCabinetRecord record = this.list[i];
                if (record.Id == id)
                {
                    this.list.Remove(record);
                    this.firstNameDictionary[record.FirstName].Remove(record);
                    this.lastNameDictionary[record.LastName].Remove(record);
                    this.dateOfBirthDictionary[record.DateOfBirth.ToString(CultureInfo.CurrentCulture)].Remove(record);
                    hasFound = true;
                    i--;
                }
            }

            return hasFound;
        }

        /// <summary>
        /// Gets the Id of the last record.
        /// </summary>
        /// <returns>Int id.</returns>
        public int GetID()
        {
            return this.id;
        }

        /// <summary>
        /// Purges deleted records.
        /// </summary>
        /// <returns>Amount of purged values.</returns>
        public int Purge()
        {
            return 0;
        }

        /// <summary>
        /// Gets the number of deleted records.
        /// </summary>
        /// <returns>Int id.</returns>
        public int GetDeletedStat()
        {
            return 0;
        }
    }
}
