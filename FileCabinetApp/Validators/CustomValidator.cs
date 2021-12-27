using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    /// <summary>
    /// Validator with custom settings.
    /// </summary>
    public class CustomValidator : CompositeValidator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomValidator"/> class.
        /// </summary>
        public CustomValidator()
            : base(
        new List<IRecordValidator>(new IRecordValidator[]
        {
           new FirstNameValidator(1, 8),
           new LastNameValidator(1, 7),
           new DateOfBirthValidator(new DateTime(1800, 1, 1), DateTime.Today),
           new HeightValidator(10, 20),
           new WeightValidator(10, 20),
           new DrivingLicenseCategoryValidator(new char[] { 'A', 'B', 'C', 'Q' }),
        }))
        {
        }
    }
}
