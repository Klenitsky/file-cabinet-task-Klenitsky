using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    /// <summary>
    /// Validator with default settings.
    /// </summary>
    public class DefaultValidator : CompositeValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValidator"/> class.
        /// </summary>
        public DefaultValidator()
            : base(
        new List<IRecordValidator>(new IRecordValidator[]
        {
           new FirstNameValidator(2, 60),
           new LastNameValidator(2, 60),
           new DateOfBirthValidator(new DateTime(1950, 1, 1), DateTime.Today),
           new HeightValidator(0, 250),
           new WeightValidator(0, 180),
           new DrivingLicenseCategoryValidator(new char[] { 'A', 'B', 'C', 'D' }),
        }))
        {
        }
    }
}
