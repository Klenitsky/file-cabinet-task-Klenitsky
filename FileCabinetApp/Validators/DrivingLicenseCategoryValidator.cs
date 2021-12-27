using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Validator of driving license category.
    /// </summary>
    public class DrivingLicenseCategoryValidator : IRecordValidator
    {
        private readonly char[] categories;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrivingLicenseCategoryValidator"/> class.
        /// </summary>
        /// <param name="categories">List of license categories</param>
        public DrivingLicenseCategoryValidator(char[] categories)
        {
            this.categories = categories;
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
            bool isValid = false;
            foreach (char category in this.categories)
            {
                if (category == arguments.DrivingLicenseCategory)
                {
                    isValid = true;
                }
            }

            if (!isValid)
            {
                throw new ArgumentException("Invalid driving license category", nameof(arguments));
            }
        }
    }
}
