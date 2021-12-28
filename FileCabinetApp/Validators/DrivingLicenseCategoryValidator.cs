using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Validator of driving license category.
    /// </summary>
    public class DrivingLicenseCategoryValidator : IRecordValidator
    {
        private Collection<char> categories = new Collection<char>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DrivingLicenseCategoryValidator"/> class.
        /// </summary>
        /// <param name="categories">List of license categories.</param>
        public DrivingLicenseCategoryValidator(Collection<string> categories)
        {
            if (categories == null)
            {
                throw new ArgumentNullException(nameof(categories));
            }

            foreach (string category in categories)
            {
                this.categories.Add(category[0]);
            }
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
