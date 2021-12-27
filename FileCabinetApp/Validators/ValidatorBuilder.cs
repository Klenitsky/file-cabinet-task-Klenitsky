using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Builder of the validators.
    /// </summary>
    public class ValidatorBuilder
    {
        private List<IRecordValidator> validators = new List<IRecordValidator>();

        /// <summary>
        /// Builds default validator.
        /// </summary>
        /// <returns>new validator.</returns>
        public IRecordValidator CreateDefault()
        {
            this.validators = new List<IRecordValidator>
            {
                new FirstNameValidator(2, 60),
                new LastNameValidator(2, 60),
                new DateOfBirthValidator(new DateTime(1950, 1, 1), DateTime.Today),
                new HeightValidator(0, 250),
                new WeightValidator(0, 180),
                new DrivingLicenseCategoryValidator(new char[] { 'A', 'B', 'C', 'D' }),
            };

            return new CompositeValidator(this.validators);
        }

        /// <summary>
        /// Builds custom validator.
        /// </summary>
        /// <returns>new validator.</returns>
        public IRecordValidator CreateCustom()
        {
            this.validators = new List<IRecordValidator>
            {
                new FirstNameValidator(1, 8),
                new LastNameValidator(1, 7),
                new DateOfBirthValidator(new DateTime(1800, 1, 1), DateTime.Today),
                new HeightValidator(10, 20),
                new WeightValidator(10, 20),
                new DrivingLicenseCategoryValidator(new char[] { 'A', 'B', 'C', 'Q' }),
            };

            return new CompositeValidator(this.validators);
        }

        /// <summary>
        /// Builds validator.
        /// </summary>
        /// <returns>new validator.</returns>
        public IRecordValidator Create()
        {
            return new CompositeValidator(this.validators);
        }

        /// <summary>
        /// Adds last name validator.
        /// </summary>
        /// <param name="min">minimal length.</param>
        /// <param name="max">max length.</param>
        /// <returns>this validator builder.</returns>
        public ValidatorBuilder ValidateFirstName(int min, int max)
        {
            this.validators.Add(new FirstNameValidator(min, max));
            return this;
        }

        /// <summary>
        /// Adds last name validator.
        /// </summary>
        /// <param name="min">minimal length.</param>
        /// <param name="max">max length.</param>
        /// <returns>this validator builder.</returns>
        public ValidatorBuilder ValidateLastName(int min, int max)
        {
            this.validators.Add(new LastNameValidator(min, max));
            return this;
        }

        /// <summary>
        /// Adds date of birth validator.
        /// </summary>
        /// <param name="min">minimal date.</param>
        /// <param name="max">max date.</param>
        /// <returns>this validator builder.</returns>
        public ValidatorBuilder ValidateDateOfBirth(DateTime min, DateTime max)
        {
            this.validators.Add(new DateOfBirthValidator(min, max));
            return this;
        }

        /// <summary>
        /// Adds height validator.
        /// </summary>
        /// <param name="min">minimal value.</param>
        /// <param name="max">max value.</param>
        /// <returns>this validator builder.</returns>
        public ValidatorBuilder ValidateHeight(short min, short max)
        {
            this.validators.Add(new HeightValidator(min, max));
            return this;
        }

        /// <summary>
        /// Adds weight validator.
        /// </summary>
        /// <param name="min">minimal value.</param>
        /// <param name="max">max value.</param>
        /// <returns>this validator builder.</returns>
        public ValidatorBuilder ValidateWeight(decimal min, decimal max)
        {
            this.validators.Add(new WeightValidator(min, max));
            return this;
        }

        /// <summary>
        /// Adds driving license category validator.
        /// </summary>
        /// <param name="categories">availiable categories.</param>
        /// <returns>this validator builder.</returns>
        public ValidatorBuilder ValidateDrivingLicenseCategory(char[] categories)
        {
            this.validators.Add(new DrivingLicenseCategoryValidator(categories));
            return this;
        }
    }
}
