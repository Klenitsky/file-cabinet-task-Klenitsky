using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    /// <summary>
    /// Validator with default settings.
    /// </summary>
    public class DefaultValidator : IRecordValidator
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

            DefaultFirstNameValidator firstNameValidator = new DefaultFirstNameValidator();
            firstNameValidator.ValidateParameters(arguments);
            DefaultLastNameValidator lastNameValidator = new DefaultLastNameValidator();
            lastNameValidator.ValidateParameters(arguments);
            DefaultDateOfBirthValidator dateOfBirthValidator = new DefaultDateOfBirthValidator();
            dateOfBirthValidator.ValidateParameters(arguments);
            DefaultHeightValidator heightValidator = new DefaultHeightValidator();
            heightValidator.ValidateParameters(arguments);
            DefaultWeightValidator weightValidator = new DefaultWeightValidator();
            weightValidator.ValidateParameters(arguments);
            DefaultDrivingLicenseCategoryValidator licenseValidator = new DefaultDrivingLicenseCategoryValidator();
            licenseValidator.ValidateParameters(arguments);
        }
    }
}
