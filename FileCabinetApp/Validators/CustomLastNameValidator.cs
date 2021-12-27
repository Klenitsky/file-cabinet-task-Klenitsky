using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Custom last name validator.
    /// </summary>
    public class CustomLastNameValidator : IRecordValidator
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

            this.ValidateLastName(arguments);
        }

        private void ValidateLastName(Arguments arguments)
        {
            if (string.IsNullOrEmpty(arguments.LastName))
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            if ((arguments.LastName.Length < 1) || (arguments.LastName.Length > 7) || string.IsNullOrWhiteSpace(arguments.LastName))
            {
                throw new ArgumentException("Invalid Last Name", nameof(arguments));
            }
        }
    }
}
