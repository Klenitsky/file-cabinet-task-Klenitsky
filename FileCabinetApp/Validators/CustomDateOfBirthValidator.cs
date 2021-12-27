using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Custom date of birth validator.
    /// </summary>
    public class CustomDateOfBirthValidator : IRecordValidator
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

            this.ValidateDateOfBirth(arguments);
        }

        private void ValidateDateOfBirth(Arguments arguments)
        {
            if (DateTime.Compare(arguments.DateOfBirth, DateTime.Today) > 0 || (DateTime.Compare(arguments.DateOfBirth, new DateTime(1800, 1, 1)) < 0))
            {
                throw new ArgumentException("Invalid date", nameof(arguments));
            }
        }
    }
}
