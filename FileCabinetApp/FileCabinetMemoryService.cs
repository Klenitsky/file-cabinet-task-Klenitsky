using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using FileCabinetApp.CommandHandlers;
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
        private readonly Dictionary<List<SearchingAttributes>, List<FileCabinetRecord>> memoization = new Dictionary<List<SearchingAttributes>, List<FileCabinetRecord>>();
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

            this.memoization.Clear();
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
        /// Finds all records with given firstname.
        /// </summary>
        /// <param name="firstName">The first name of the person.</param>
        /// <returns>A list of records found.</returns>
        /// <exception cref="ArgumentNullException">String firstName is null.</exception>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
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
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
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
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime dateTime)
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
            this.memoization.Clear();
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

        /// <summary>
        /// Inserts a new Record.
        /// </summary>
        /// <param name="id">Id of a record.</param>
        /// <param name="arguments">Properties of the record.</param>
        /// <returns>New record's Id.</returns>
        public int InsertRecord(int id, Arguments arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            this.validator.ValidateParameters(arguments);

            var record = new FileCabinetRecord
            {
                Id = id,
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
        /// Updates a record.
        /// </summary>
        /// <param name="attriubutesToUpdate">Properties of values to update records.</param>
        /// <param name="attriubutesToFind">Properties of values to find records.</param>
        /// <returns>Updated values.</returns>
        public IEnumerable<FileCabinetRecord> Update(IEnumerable<SearchingAttributes> attriubutesToUpdate, IEnumerable<SearchingAttributes> attriubutesToFind)
        {
            if (attriubutesToFind == null)
            {
                throw new ArgumentNullException(nameof(attriubutesToFind));
            }

            if (attriubutesToUpdate == null)
            {
                throw new ArgumentNullException(nameof(attriubutesToUpdate));
            }

            this.memoization.Clear();
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            foreach (var record in this.list)
            {
                bool isValid = true;
                foreach (var attribute in attriubutesToFind)
                {
                    switch (attribute.Attribute)
                    {
                        case SearchingAttributes.AttributesSearch.Id:
                            if (record.Id != int.Parse(attribute.Value, CultureInfo.CurrentCulture))
                            {
                                isValid = false;
                            }

                            break;
                        case SearchingAttributes.AttributesSearch.FirstName:
                            if (record.FirstName != attribute.Value)
                            {
                                isValid = false;
                            }

                            break;
                        case SearchingAttributes.AttributesSearch.LastName:
                            if (record.LastName != attribute.Value)
                            {
                                isValid = false;
                            }

                            break;
                        case SearchingAttributes.AttributesSearch.DateOfBirth:
                            if (DateTime.Compare(record.DateOfBirth, DateTime.Parse(attribute.Value, CultureInfo.CurrentCulture)) != 0)
                            {
                                isValid = false;
                            }

                            break;
                        case SearchingAttributes.AttributesSearch.Height:
                            if (record.Height != short.Parse(attribute.Value, CultureInfo.CurrentCulture))
                            {
                                isValid = false;
                            }

                            break;
                        case SearchingAttributes.AttributesSearch.Weight:
                            if (record.Weight != short.Parse(attribute.Value, CultureInfo.CurrentCulture))
                            {
                                isValid = false;
                            }

                            break;
                        case SearchingAttributes.AttributesSearch.DrivingLicenseCategory:
                            if (record.DrivingLicenseCategory != char.Parse(attribute.Value))
                            {
                                isValid = false;
                            }

                            break;
                    }
                }

                if (isValid)
                {
                    foreach (var attribute in attriubutesToUpdate)
                    {
                        switch (attribute.Attribute)
                        {
                            case SearchingAttributes.AttributesSearch.Id:
                                record.Id = int.Parse(attribute.Value, CultureInfo.CurrentCulture);
                                break;
                            case SearchingAttributes.AttributesSearch.FirstName:
                                record.FirstName = attribute.Value;

                                break;
                            case SearchingAttributes.AttributesSearch.LastName:
                                record.LastName = attribute.Value;
                                break;
                            case SearchingAttributes.AttributesSearch.DateOfBirth:
                                record.DateOfBirth = DateTime.Parse(attribute.Value, CultureInfo.CurrentCulture);
                                break;
                            case SearchingAttributes.AttributesSearch.Height:
                                record.Height = short.Parse(attribute.Value, CultureInfo.CurrentCulture);
                                break;
                            case SearchingAttributes.AttributesSearch.Weight:
                                record.Weight = short.Parse(attribute.Value, CultureInfo.CurrentCulture);
                                break;
                            case SearchingAttributes.AttributesSearch.DrivingLicenseCategory:
                                record.DrivingLicenseCategory = char.Parse(attribute.Value);
                                break;
                        }
                    }

                    result.Add(record);
                }
            }

            return result;
        }

        /// <summary>
        /// Removes a record.
        /// </summary>
        /// <param name="arguments">Properties of values to delete.</param>
        /// <returns>Deleted values.</returns>
        public IEnumerable<FileCabinetRecord> Delete(SearchingAttributes arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            this.memoization.Clear();
            switch (arguments.Attribute)
            {
                case SearchingAttributes.AttributesSearch.Id:
                    return this.DeleteId(arguments);
                case SearchingAttributes.AttributesSearch.FirstName:
                    return this.DeleteFirstName(arguments);
                case SearchingAttributes.AttributesSearch.LastName:
                    return this.DeleteLastName(arguments);
                case SearchingAttributes.AttributesSearch.DateOfBirth:
                    return this.DeleteDateOfBirth(arguments);
                case SearchingAttributes.AttributesSearch.Height:
                    return this.DeleteHeight(arguments);
                case SearchingAttributes.AttributesSearch.Weight:
                    return this.DeleteWeight(arguments);
                case SearchingAttributes.AttributesSearch.DrivingLicenseCategory:
                    return this.DeleteDrivingLicenseCategory(arguments);
                default:
                    throw new ArgumentException(string.Empty, nameof(arguments));
            }
        }

        /// <summary>
        /// Selects records.
        /// </summary>
        /// <param name="attriubutesToFind">Properties of values to find records.</param>
        /// <param name="complexAttribute">Or or and.</param>
        /// <returns>Selected values.</returns>
        public IEnumerable<FileCabinetRecord> SelectRecords(IEnumerable<SearchingAttributes> attriubutesToFind, string complexAttribute)
        {
            if (attriubutesToFind == null)
            {
                throw new ArgumentNullException(nameof(attriubutesToFind));
            }

            foreach (var key in this.memoization.Keys)
            {
                if (key.Count == ((List<SearchingAttributes>)attriubutesToFind).Count)
                {
                    bool isIdentical = true;
                    for (int i = 0; i < key.Count; i++)
                    {
                        if (key[i].Attribute != ((List<SearchingAttributes>)attriubutesToFind)[i].Attribute || key[i].Value != ((List<SearchingAttributes>)attriubutesToFind)[i].Value)
                        {
                            isIdentical = false;
                        }
                    }

                    if (isIdentical)
                    {
                        return this.memoization[key];
                    }
                }
            }

            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            foreach (var record in this.list)
            {
                bool isValid = true;
                if (complexAttribute == "and" || string.IsNullOrEmpty(complexAttribute))
                {
                    isValid = true;
                    foreach (var attribute in attriubutesToFind)
                    {
                        switch (attribute.Attribute)
                        {
                            case SearchingAttributes.AttributesSearch.Id:
                                if (record.Id != int.Parse(attribute.Value, CultureInfo.CurrentCulture))
                                {
                                    isValid = false;
                                }

                                break;
                            case SearchingAttributes.AttributesSearch.FirstName:
                                if (record.FirstName != attribute.Value)
                                {
                                    isValid = false;
                                }

                                break;
                            case SearchingAttributes.AttributesSearch.LastName:
                                if (record.LastName != attribute.Value)
                                {
                                    isValid = false;
                                }

                                break;
                            case SearchingAttributes.AttributesSearch.DateOfBirth:
                                if (DateTime.Compare(record.DateOfBirth, DateTime.Parse(attribute.Value, CultureInfo.CurrentCulture)) != 0)
                                {
                                    isValid = false;
                                }

                                break;
                            case SearchingAttributes.AttributesSearch.Height:
                                if (record.Height != short.Parse(attribute.Value, CultureInfo.CurrentCulture))
                                {
                                    isValid = false;
                                }

                                break;
                            case SearchingAttributes.AttributesSearch.Weight:
                                if (record.Weight != short.Parse(attribute.Value, CultureInfo.CurrentCulture))
                                {
                                    isValid = false;
                                }

                                break;
                            case SearchingAttributes.AttributesSearch.DrivingLicenseCategory:
                                if (record.DrivingLicenseCategory != char.Parse(attribute.Value))
                                {
                                    isValid = false;
                                }

                                break;
                        }
                    }
                }

                if (complexAttribute == "or")
                {
                    isValid = false;
                    foreach (var attribute in attriubutesToFind)
                    {
                        switch (attribute.Attribute)
                        {
                            case SearchingAttributes.AttributesSearch.Id:
                                if (record.Id == int.Parse(attribute.Value, CultureInfo.CurrentCulture))
                                {
                                    isValid = true;
                                }

                                break;
                            case SearchingAttributes.AttributesSearch.FirstName:
                                if (record.FirstName == attribute.Value)
                                {
                                    isValid = true;
                                }

                                break;
                            case SearchingAttributes.AttributesSearch.LastName:
                                if (record.LastName == attribute.Value)
                                {
                                    isValid = true;
                                }

                                break;
                            case SearchingAttributes.AttributesSearch.DateOfBirth:
                                if (DateTime.Compare(record.DateOfBirth, DateTime.Parse(attribute.Value, CultureInfo.CurrentCulture)) == 0)
                                {
                                    isValid = true;
                                }

                                break;
                            case SearchingAttributes.AttributesSearch.Height:
                                if (record.Height == short.Parse(attribute.Value, CultureInfo.CurrentCulture))
                                {
                                    isValid = true;
                                }

                                break;
                            case SearchingAttributes.AttributesSearch.Weight:
                                if (record.Weight == short.Parse(attribute.Value, CultureInfo.CurrentCulture))
                                {
                                    isValid = true;
                                }

                                break;
                            case SearchingAttributes.AttributesSearch.DrivingLicenseCategory:
                                if (record.DrivingLicenseCategory == char.Parse(attribute.Value))
                                {
                                    isValid = true;
                                }

                                break;
                        }
                    }
                }

                if (isValid)
                {
                    result.Add(record);
                }
            }

            this.memoization.Add((List<SearchingAttributes>)attriubutesToFind, result);
            return result;
        }

        private IEnumerable<FileCabinetRecord> DeleteId(SearchingAttributes arguments)
        {
            int deleteId;
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            bool success = int.TryParse(arguments.Value, out deleteId);
            if (!success)
            {
                throw new ArgumentException(string.Empty, nameof(arguments));
            }

            for (int i = 0; i < this.list.Count; i++)
            {
                FileCabinetRecord record = this.list[i];
                if (record.Id == deleteId)
                {
                    result.Add(record);
                    this.list.Remove(record);
                    this.firstNameDictionary[record.FirstName].Remove(record);
                    this.lastNameDictionary[record.LastName].Remove(record);
                    this.dateOfBirthDictionary[record.DateOfBirth.ToString(CultureInfo.CurrentCulture)].Remove(record);
                    i--;
                }
            }

            return result;
        }

        private IEnumerable<FileCabinetRecord> DeleteFirstName(SearchingAttributes arguments)
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            for (int i = 0; i < this.list.Count; i++)
            {
                FileCabinetRecord record = this.list[i];
                if (record.FirstName == arguments.Value)
                {
                    result.Add(record);
                    this.list.Remove(record);
                    this.firstNameDictionary[record.FirstName].Remove(record);
                    this.lastNameDictionary[record.LastName].Remove(record);
                    this.dateOfBirthDictionary[record.DateOfBirth.ToString(CultureInfo.CurrentCulture)].Remove(record);
                    i--;
                }
            }

            return result;
        }

        private IEnumerable<FileCabinetRecord> DeleteLastName(SearchingAttributes arguments)
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            for (int i = 0; i < this.list.Count; i++)
            {
                FileCabinetRecord record = this.list[i];
                if (record.LastName == arguments.Value)
                {
                    result.Add(record);
                    this.list.Remove(record);
                    this.firstNameDictionary[record.FirstName].Remove(record);
                    this.lastNameDictionary[record.LastName].Remove(record);
                    this.dateOfBirthDictionary[record.DateOfBirth.ToString(CultureInfo.CurrentCulture)].Remove(record);
                    i--;
                }
            }

            return result;
        }

        private IEnumerable<FileCabinetRecord> DeleteDateOfBirth(SearchingAttributes arguments)
        {
            DateTime dateToDelete;
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            bool success = DateTime.TryParse(arguments.Value, out dateToDelete);
            if (!success)
            {
                throw new ArgumentException(string.Empty, nameof(arguments));
            }

            for (int i = 0; i < this.list.Count; i++)
            {
                FileCabinetRecord record = this.list[i];
                if (DateTime.Compare(record.DateOfBirth, dateToDelete) == 0)
                {
                    result.Add(record);
                    this.list.Remove(record);
                    this.firstNameDictionary[record.FirstName].Remove(record);
                    this.lastNameDictionary[record.LastName].Remove(record);
                    this.dateOfBirthDictionary[record.DateOfBirth.ToString(CultureInfo.CurrentCulture)].Remove(record);
                    i--;
                }
            }

            return result;
        }

        private IEnumerable<FileCabinetRecord> DeleteHeight(SearchingAttributes arguments)
        {
            short heightToDelete;
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            bool success = short.TryParse(arguments.Value, out heightToDelete);
            if (!success)
            {
                throw new ArgumentException(string.Empty, nameof(arguments));
            }

            for (int i = 0; i < this.list.Count; i++)
            {
                FileCabinetRecord record = this.list[i];
                if (record.Height == heightToDelete)
                {
                    result.Add(record);
                    this.list.Remove(record);
                    this.firstNameDictionary[record.FirstName].Remove(record);
                    this.lastNameDictionary[record.LastName].Remove(record);
                    this.dateOfBirthDictionary[record.DateOfBirth.ToString(CultureInfo.CurrentCulture)].Remove(record);
                    i--;
                }
            }

            return result;
        }

        private IEnumerable<FileCabinetRecord> DeleteDrivingLicenseCategory(SearchingAttributes arguments)
        {
            char categoryToDelete;
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            bool success = char.TryParse(arguments.Value, out categoryToDelete);
            if (!success)
            {
                throw new ArgumentException(string.Empty, nameof(arguments));
            }

            for (int i = 0; i < this.list.Count; i++)
            {
                FileCabinetRecord record = this.list[i];
                if (record.DrivingLicenseCategory == categoryToDelete)
                {
                    result.Add(record);
                    this.list.Remove(record);
                    this.firstNameDictionary[record.FirstName].Remove(record);
                    this.lastNameDictionary[record.LastName].Remove(record);
                    this.dateOfBirthDictionary[record.DateOfBirth.ToString(CultureInfo.CurrentCulture)].Remove(record);
                    i--;
                }
            }

            return result;
        }

        private IEnumerable<FileCabinetRecord> DeleteWeight(SearchingAttributes arguments)
        {
            decimal weightToDelete;
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            bool success = decimal.TryParse(arguments.Value, out weightToDelete);
            if (!success)
            {
                throw new ArgumentException(string.Empty, nameof(arguments));
            }

            for (int i = 0; i < this.list.Count; i++)
            {
                FileCabinetRecord record = this.list[i];
                if (record.Weight == weightToDelete)
                {
                    result.Add(record);
                    this.list.Remove(record);
                    this.firstNameDictionary[record.FirstName].Remove(record);
                    this.lastNameDictionary[record.LastName].Remove(record);
                    this.dateOfBirthDictionary[record.DateOfBirth.ToString(CultureInfo.CurrentCulture)].Remove(record);
                    i--;
                }
            }

            return result;
        }

        /// <summary>
        /// Edits an existing record.
        /// </summary>
        /// <param name="id">The ID of a record.</param>
        /// <param name="arguments">Properties of the record.</param>
        /// <exception cref="ArgumentNullException">One of the parameters is null.</exception>
        /// <exception cref="ArgumentException">One of the parameters is not valid.</exception>
        private void EditRecord(int id, Arguments arguments)
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
    }
}
