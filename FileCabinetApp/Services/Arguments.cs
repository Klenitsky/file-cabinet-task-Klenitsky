using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Stores parameters for creating and editing records.
    /// </summary>
    public class Arguments
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Arguments"/> class.
        /// </summary>
        /// <param name="firstName">The first name of the person.</param>
        /// <param name="lastName">The last name of the person.</param>
        /// <param name="dateTime">The date of birth of the person.</param>
        /// <param name="height">The height of the person.</param>
        /// <param name="weight">The weight of the person.</param>
        /// <param name="drivingLicenseCategory">The category of driving license of the person.</param>
        /// <returns>A list of records found.</returns>
        /// <exception cref="ArgumentNullException">String firstName is null.</exception>
        public Arguments(string firstName, string lastName, DateTime dateTime, short height, decimal weight, char drivingLicenseCategory)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateOfBirth = dateTime;
            this.Height = height;
            this.Weight = weight;
            this.DrivingLicenseCategory = drivingLicenseCategory;
        }

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
    }
}
