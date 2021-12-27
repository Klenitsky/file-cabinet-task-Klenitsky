using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    /// <summary>
    /// Validator with custom settings.
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

            CustomFirstNameValidator firstNameValidator = new CustomFirstNameValidator();
            firstNameValidator.ValidateParameters(arguments);
            CustomLastNameValidator lastNameValidator = new CustomLastNameValidator();
            lastNameValidator.ValidateParameters(arguments);
            CustomDateOfBirthValidator dateOfBirthValidator = new CustomDateOfBirthValidator();
            dateOfBirthValidator.ValidateParameters(arguments);
            CustomHeightValidator heightValidator = new CustomHeightValidator();
            heightValidator.ValidateParameters(arguments);
            CustomWeightValidator weightValidator = new CustomWeightValidator();
            weightValidator.ValidateParameters(arguments);
            CustomDrivingLicenseCategoryValidator licenseValidator = new CustomDrivingLicenseCategoryValidator();
            licenseValidator.ValidateParameters(arguments);
        }
    }
}
