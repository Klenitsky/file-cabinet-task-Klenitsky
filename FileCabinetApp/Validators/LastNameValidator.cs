using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Validator of last name.
    /// </summary>
    public class LastNameValidator : IRecordValidator
    {
        private readonly int minLength;
        private readonly int maxLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="LastNameValidator"/> class.
        /// </summary>
        /// <param name="min">min length.</param>
        /// <param name="max">max length.</param>
        public LastNameValidator(int min, int max)
        {
            this.minLength = min;
            this.maxLength = max;
        }

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

            if (string.IsNullOrEmpty(arguments.LastName))
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            if ((arguments.LastName.Length < this.minLength) || (arguments.LastName.Length > this.maxLength) || string.IsNullOrWhiteSpace(arguments.LastName))
            {
                throw new ArgumentException("Invalid Last Name", nameof(arguments));
            }
        }
    }
}
