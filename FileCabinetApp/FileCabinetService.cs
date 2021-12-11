using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
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

            this.list.Add(record);

            return record.Id;
        }

        public FileCabinetRecord[] GetRecords()
        {
            FileCabinetRecord[] result = new FileCabinetRecord[this.list.Count];
            this.list.CopyTo(result);
            return result;
        }

        public int GetStat()
        {
                return this.list.Count;
        }

        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short height, decimal weight, char drivingLicenseCategory)
        {
            bool isValid = false;
            foreach (FileCabinetRecord tmp in this.list)
            {
                if (tmp.Id == id)
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

                    this.firstNameDictionary[tmp.FirstName].Remove(tmp);
                    this.lastNameDictionary[tmp.LastName].Remove(tmp);
                    tmp.FirstName = firstName;
                    tmp.LastName = lastName;
                    tmp.DateOfBirth = dateOfBirth;
                    tmp.Height = height;
                    tmp.Weight = weight;
                    tmp.DrivingLicenseCategory = drivingLicenseCategory;

                    if (!this.firstNameDictionary.ContainsKey(firstName))
                    {
                        this.firstNameDictionary.Add(firstName, new List<FileCabinetRecord>());
                    }

                    this.firstNameDictionary[firstName].Add(tmp);

                    if (!this.lastNameDictionary.ContainsKey(lastName))
                    {
                        this.lastNameDictionary.Add(lastName, new List<FileCabinetRecord>());
                    }

                    this.lastNameDictionary[lastName].Add(tmp);
                }
            }

            if (!isValid)
            {
                throw new ArgumentException("There is no such Id.", nameof(id));
            }
        }

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentNullException(nameof(firstName));
            }

            return this.firstNameDictionary[firstName].ToArray();
        }

        public FileCabinetRecord[] FindByLastName(string lastname)
        {
            if (string.IsNullOrEmpty(lastname))
            {
                throw new ArgumentNullException(nameof(lastname));
            }

            return this.lastNameDictionary[lastname].ToArray();
        }

        public FileCabinetRecord[] FindByDateOfBirth(DateTime date)
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            foreach (FileCabinetRecord tmp in this.list)
            {
                if (DateTime.Compare(tmp.DateOfBirth, date) == 0)
                {
                    result.Add(tmp);
                }
            }

            return result.ToArray();
        }
    }
}
