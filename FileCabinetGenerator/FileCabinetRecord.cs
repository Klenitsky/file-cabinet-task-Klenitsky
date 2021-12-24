using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetGenerator
{
    class FileCabinetRecord
    {
        /// <summary>
        /// Gets or sets id of a person.
        /// </summary>
        /// <value>
        /// Id of the record.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets first name of a person.
        /// </summary>
        /// <value>
        /// First name of the record.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets first name of a person.
        /// </summary>
        /// <value>
        /// First name of the person.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets last name of a person.
        /// </summary>
        /// <value>
        /// Lst name of the person.
        /// </value>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets date of birth of a person.
        /// </summary>
        /// <value>
        /// Date of birth of the record.
        /// </value>
        public short Height { get; set; }

        /// <summary>
        /// Gets or sets Height of a person.
        /// </summary>
        /// <value>
        /// Weight of the person.
        /// </value>
        public decimal Weight { get; set; }

        /// <summary>
        /// Gets or sets driving license Category of a person.
        /// </summary>
        /// <value>
        /// Driving license category of the person.
        /// </value>
        public char DrivingLicenseCategory { get; set; }

        /// <summary>
        /// Gets a string representation of the record.
        /// </summary>
        /// <returns>String with information about record.</returns>
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
}
