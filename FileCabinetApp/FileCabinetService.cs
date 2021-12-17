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

        /// <summary>
        /// Creates a new Record.
        /// </summary>
        /// <param name="firstName">The first name of the person.</param>
        /// <param name="lastName">The last name of the person.</param>
        /// <param name="dateOfBirth">The date of birth of the person.</param>
        /// <param name="height">The height of the person.</param>
        /// <param name="weight">The weight of the person.</param>
        /// <param name="drivingLicenseCategory">A name of category of the driving license.</param>
        /// <returns>New record's Id.</returns>
        /// <exception cref="ArgumentNullException">One of the parameters is null.</exception>
        /// <exception cref="ArgumentException">One of the parameters is not valid.</exception>
        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, short height, decimal weight, char drivingLicenseCategory)
        {
            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentNullException(nameof(firstName));
            }

            if ((firstName.Length < 2) || (firstName.Length > 60) || string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentException("Invalid First Name", nameof(firstName));
            }

            if (string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentNullException(nameof(lastName));
            }

            if ((lastName.Length < 2) || (lastName.Length > 60) || string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException("Invalid Last Name", nameof(lastName));
            }

            if (DateTime.Compare(dateOfBirth, DateTime.Today) > 0 || (DateTime.Compare(dateOfBirth, new DateTime(1950, 1, 1)) < 0))
            {
                throw new ArgumentException("Invalid date", nameof(dateOfBirth));
            }

            if ((height < 0) || (height > 250))
            {
                throw new ArgumentException("Invalid height", nameof(height));
            }

            if ((weight < 0) || (weight > 180))
            {
                throw new ArgumentException("Invalid height", nameof(height));
            }

            if ((drivingLicenseCategory != 'A') && (drivingLicenseCategory != 'B') && (drivingLicenseCategory != 'C') && (drivingLicenseCategory != 'D'))
            {
                throw new ArgumentException("Invalid license", nameof(drivingLicenseCategory));
            }

            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Height = height,
                Weight = weight,
                DrivingLicenseCategory = drivingLicenseCategory,
            };

            if (!this.firstNameDictionary.ContainsKey(firstName))
            {
                this.firstNameDictionary.Add(firstName, new List<FileCabinetRecord>());
            }

            this.firstNameDictionary[firstName].Add(record);

            if (!this.lastNameDictionary.ContainsKey(lastName))
            {
                this.lastNameDictionary.Add(lastName, new List<FileCabinetRecord>());
            }

            this.lastNameDictionary[lastName].Add(record);

            if (!this.dateOfBirthDictionary.ContainsKey(dateOfBirth.ToString(CultureInfo.CurrentCulture)))
            {
                this.dateOfBirthDictionary.Add(dateOfBirth.ToString(CultureInfo.CurrentCulture), new List<FileCabinetRecord>());
            }

            this.dateOfBirthDictionary[dateOfBirth.ToString(CultureInfo.CurrentCulture)].Add(record);

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
        /// <param name="firstName">The first name of the person.</param>
        /// <param name="lastName">The last name of the person.</param>
        /// <param name="dateOfBirth">The date of birth of the person.</param>
        /// <param name="height">The height of the person.</param>
        /// <param name="weight">The weight of the person.</param>
        /// <param name="drivingLicenseCategory">A name of category of the driving license.</param>
        /// <exception cref="ArgumentNullException">One of the parameters is null.</exception>
        /// <exception cref="ArgumentException">One of the parameters is not valid.</exception>
        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short height, decimal weight, char drivingLicenseCategory)
        {
            bool isValid = false;
            foreach (FileCabinetRecord record in this.list)
            {
                if (record.Id == id)
                {
                    isValid = true;
                    if (string.IsNullOrEmpty(firstName))
                    {
                        throw new ArgumentNullException(nameof(firstName));
                    }

                    if ((firstName.Length < 2) || (firstName.Length > 60) || string.IsNullOrWhiteSpace(firstName))
                    {
                        throw new ArgumentException("Invalid First Name", nameof(firstName));
                    }

                    if (string.IsNullOrEmpty(lastName))
                    {
                        throw new ArgumentNullException(nameof(lastName));
                    }

                    if ((lastName.Length < 2) || (lastName.Length > 60) || string.IsNullOrWhiteSpace(lastName))
                    {
                        throw new ArgumentException("Invalid Last Name", nameof(lastName));
                    }

                    if (DateTime.Compare(dateOfBirth, DateTime.Today) > 0 || (DateTime.Compare(dateOfBirth, new DateTime(1950, 1, 1)) < 0))
                    {
                        throw new ArgumentException("Invalid date", nameof(dateOfBirth));
                    }

                    if ((height < 0) || (height > 250))
                    {
                        throw new ArgumentException("Invalid height", nameof(height));
                    }

                    if ((weight < 0) || (weight > 180))
                    {
                        throw new ArgumentException("Invalid height", nameof(height));
                    }

                    if ((drivingLicenseCategory != 'A') && (drivingLicenseCategory != 'B') && (drivingLicenseCategory != 'C') && (drivingLicenseCategory != 'D'))
                    {
                        throw new ArgumentException("Invalid license", nameof(drivingLicenseCategory));
                    }

                    this.firstNameDictionary[record.FirstName].Remove(record);
                    this.lastNameDictionary[record.LastName].Remove(record);
                    this.dateOfBirthDictionary[record.DateOfBirth.ToString(CultureInfo.CurrentCulture)].Remove(record);
                    record.FirstName = firstName;
                    record.LastName = lastName;
                    record.DateOfBirth = dateOfBirth;
                    record.Height = height;
                    record.Weight = weight;
                    record.DrivingLicenseCategory = drivingLicenseCategory;

                    if (!this.firstNameDictionary.ContainsKey(firstName))
                    {
                        this.firstNameDictionary.Add(firstName, new List<FileCabinetRecord>());
                    }

                    this.firstNameDictionary[firstName].Add(record);

                    if (!this.lastNameDictionary.ContainsKey(lastName))
                    {
                        this.lastNameDictionary.Add(lastName, new List<FileCabinetRecord>());
                    }

                    this.lastNameDictionary[lastName].Add(record);

                    if (!this.dateOfBirthDictionary.ContainsKey(dateOfBirth.ToString(CultureInfo.CurrentCulture)))
                    {
                        this.dateOfBirthDictionary.Add(dateOfBirth.ToString(CultureInfo.CurrentCulture), new List<FileCabinetRecord>());
                    }

                    this.dateOfBirthDictionary[dateOfBirth.ToString(CultureInfo.CurrentCulture)].Add(record);
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
