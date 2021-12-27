﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Default firstname validator.
    /// </summary>
    public class DefaultFirstNameValidator : IRecordValidator
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

            this.ValidateFirstName(arguments);
        }

        private void ValidateFirstName(Arguments arguments)
        {
            if (string.IsNullOrEmpty(arguments.FirstName))
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            if ((arguments.FirstName.Length < 2) || (arguments.FirstName.Length > 60) || string.IsNullOrWhiteSpace(arguments.FirstName))
            {
                throw new ArgumentException("Invalid First Name", nameof(arguments));
            }
        }
    }
}
