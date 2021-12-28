using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using FileCabinetApp.ValidationRules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Builder of the validators.
    /// </summary>
    public class ValidatorBuilder
    {
        private readonly ValidationRules.ValidationTypes validationRules;
        private List<IRecordValidator> validators = new List<IRecordValidator>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatorBuilder"/> class.
        /// </summary>
        public ValidatorBuilder()
        {
            this.validationRules = ValidationRulesReader.ReadRules("validation-rules.json");
        }

        /// <summary>
        /// Builds default validator.
        /// </summary>
        /// <returns>new validator.</returns>
        public IRecordValidator CreateDefault()
        {
            DateTime minDate;
            bool success = DateTime.TryParse(this.validationRules.Default.DateOfBirth.Min, out minDate);
            if (!success)
            {
                Console.WriteLine("Validation rules invalid.");
                return null;
            }

            DateTime maxDate;
            success = DateTime.TryParse(this.validationRules.Default.DateOfBirth.Max, out maxDate);
            if (!success)
            {
                Console.WriteLine("Validation rules invalid.");
                return null;
            }

            this.validators = new List<IRecordValidator>
            {
                new FirstNameValidator(this.validationRules.Default.FirstName.MinValue, this.validationRules.Default.FirstName.MaxValue),
                new LastNameValidator(this.validationRules.Default.LastName.MinValue, this.validationRules.Default.LastName.MaxValue),
                new DateOfBirthValidator(minDate, maxDate),
                new HeightValidator(this.validationRules.Default.Height.MinValue, this.validationRules.Default.Height.MaxValue),
                new WeightValidator(this.validationRules.Default.Weight.MinValue, this.validationRules.Default.Weight.MaxValue),
                new DrivingLicenseCategoryValidator(this.validationRules.Default.DrivingLicenseCategory.Values),
            };

            return new CompositeValidator(this.validators);
        }

        /// <summary>
        /// Builds custom validator.
        /// </summary>
        /// <returns>new validator.</returns>
        public IRecordValidator CreateCustom()
        {
            DateTime minDate;
            bool success = DateTime.TryParse(this.validationRules.Custom.DateOfBirth.Min, out minDate);
            if (!success)
            {
                Console.WriteLine("Validation rules invalid.");
                return null;
            }

            DateTime maxDate;
            success = DateTime.TryParse(this.validationRules.Custom.DateOfBirth.Max, out maxDate);
            if (!success)
            {
                Console.WriteLine("Validation rules invalid.");
                return null;
            }

            this.validators = new List<IRecordValidator>
            {
                new FirstNameValidator(this.validationRules.Custom.FirstName.MinValue, this.validationRules.Custom.FirstName.MaxValue),
                new LastNameValidator(this.validationRules.Custom.LastName.MinValue, this.validationRules.Custom.LastName.MaxValue),
                new DateOfBirthValidator(minDate, maxDate),
                new HeightValidator(this.validationRules.Custom.Height.MinValue, this.validationRules.Custom.Height.MaxValue),
                new WeightValidator(this.validationRules.Custom.Weight.MinValue, this.validationRules.Custom.Weight.MaxValue),
                new DrivingLicenseCategoryValidator(this.validationRules.Custom.DrivingLicenseCategory.Values),
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
        public ValidatorBuilder ValidateDrivingLicenseCategory(Collection<string> categories)
        {
            this.validators.Add(new DrivingLicenseCategoryValidator(categories));
            return this;
        }
    }
}
