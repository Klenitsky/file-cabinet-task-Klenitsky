using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Default last name validator.
    /// </summary>
    public class DefaultLastNameValidator : IRecordValidator
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

            if ((arguments.LastName.Length < 2) || (arguments.LastName.Length > 60) || string.IsNullOrWhiteSpace(arguments.LastName))
            {
                throw new ArgumentException("Invalid Last Name", nameof(arguments));
            }
        }
    }
}
