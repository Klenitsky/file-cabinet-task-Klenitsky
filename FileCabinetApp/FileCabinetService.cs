using System;
using System.Collections.Generic;
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
    }
}
