using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp
{
    public class FileCabinetRecord
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public short Height { get; set; }

        public decimal Weight { get; set; }

        public char DrivingLicenseCategory { get; set; }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append("#" + this.Id + ", ");
            result.Append(this.FirstName + ", ");
            result.Append(this.LastName + ", ");
            result.Append(this.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture) + ", ");
            result.Append("height: " + this.Height + ", ");
            result.Append("weight: " + this.Weight + ", ");
            result.Append("driving license category: " + this.DrivingLicenseCategory);
            return result.ToString();
        }
    }
}