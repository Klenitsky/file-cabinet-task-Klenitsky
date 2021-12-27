﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Default driving license validator.
    /// </summary>
    public class DefaultDrivingLicenseCategoryValidator : IRecordValidator
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

            this.ValidateDrivingLicenseCategory(arguments);
        }

        private void ValidateDrivingLicenseCategory(Arguments arguments)
        {
            if ((arguments.DrivingLicenseCategory != 'A') && (arguments.DrivingLicenseCategory != 'B') && (arguments.DrivingLicenseCategory != 'C') && (arguments.DrivingLicenseCategory != 'D'))
            {
                throw new ArgumentException("Invalid license", nameof(arguments));
            }
        }
    }
}
