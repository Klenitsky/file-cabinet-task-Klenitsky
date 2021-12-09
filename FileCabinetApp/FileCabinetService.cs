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
