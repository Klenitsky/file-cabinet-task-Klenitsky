using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Validator with custom settings
    /// </summary>
    public class CustomValidator : IRecordValidator
    {
        /// <summary>
        /// Checks the parameters.
        /// </summary>
        /// <param name="arguments">Properties of the record.</param>
        public void ValidateParameters(Arguments arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            if (string.IsNullOrEmpty(arguments.FirstName))
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            if ((arguments.FirstName.Length < 1) || (arguments.FirstName.Length > 8) || string.IsNullOrWhiteSpace(arguments.FirstName))
            {
                throw new ArgumentException("Invalid First Name", nameof(arguments));
            }

            if (string.IsNullOrEmpty(arguments.LastName))
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            if ((arguments.LastName.Length < 1) || (arguments.LastName.Length > 8) || string.IsNullOrWhiteSpace(arguments.LastName))
            {
                throw new ArgumentException("Invalid Last Name", nameof(arguments));
            }

            if (DateTime.Compare(arguments.DateOfBirth, DateTime.Today) > 0 || (DateTime.Compare(arguments.DateOfBirth, new DateTime(1800, 1, 1)) < 0))
            {
                throw new ArgumentException("Invalid date", nameof(arguments));
            }

            if ((arguments.Height < 10) || (arguments.Height > 20))
            {
                throw new ArgumentException("Invalid height", nameof(arguments));
            }

            if ((arguments.Weight < 10) || (arguments.Weight > 20))
            {
                throw new ArgumentException("Invalid height", nameof(arguments));
            }

            if ((arguments.DrivingLicenseCategory != 'A') && (arguments.DrivingLicenseCategory != 'B') && (arguments.DrivingLicenseCategory != 'C') && (arguments.DrivingLicenseCategory != 'D'))
            {
                throw new ArgumentException("Invalid license", nameof(arguments));
            }
        }
    }
}
