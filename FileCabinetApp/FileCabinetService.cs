using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

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

                    tmp.FirstName = firstName;
                    tmp.LastName = lastName;
                    tmp.DateOfBirth = dateOfBirth;
                    tmp.Height = height;
                    tmp.Weight = weight;
                    tmp.DrivingLicenseCategory = drivingLicenseCategory;
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

            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            foreach (FileCabinetRecord tmp in this.list)
            {
                if (tmp.FirstName.ToUpperInvariant() == firstName.ToUpperInvariant())
                {
                    result.Add(tmp);
                }
            }

            return result.ToArray();
        }

        public FileCabinetRecord[] FindByLastName(string lastname)
        {
            if (string.IsNullOrEmpty(lastname))
            {
                throw new ArgumentNullException(nameof(lastname));
            }

            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            foreach (FileCabinetRecord tmp in this.list)
            {
                if (tmp.LastName.ToUpperInvariant() == lastname.ToUpperInvariant())
                {
                    result.Add(tmp);
                }
            }

            return result.ToArray();
        }
    }
}
