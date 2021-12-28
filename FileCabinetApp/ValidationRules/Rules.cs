using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.ValidationRules.SpecifiedRules;
using Newtonsoft.Json;

namespace FileCabinetApp.ValidationRules
{
    /// <summary>
    /// Object with rules of validation.
    /// </summary>
    public class Rules
    {
        /// <summary>
        /// Gets or sets first name validation rule.
        /// </summary>
        /// <value>
        /// Last name validation rule.
        /// </value>
        [JsonProperty("firstName")]
        public Name FirstName { get; set; }

        /// <summary>
        /// Gets or sets first name validation rule.
        /// </summary>
        /// <value>
        /// Last name validation rule.
        /// </value>
        [JsonProperty("lastName")]
        public Name LastName { get; set; }

        /// <summary>
        /// Gets or sets date of birth validation rule.
        /// </summary>
        /// <value>
        /// Date of birth validation rule.
        /// </value>
        [JsonProperty("dateOfBirth")]
        public DateOfBirth DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets weight validation rule.
        /// </summary>
        /// <value>
        /// Height validation rule.
        /// </value>
        [JsonProperty("height")]
        public Height Height { get; set; }

        /// <summary>
        /// Gets or sets weight validation rule.
        /// </summary>
        /// <value>
        /// Weight validation rule.
        /// </value>
        [JsonProperty("Weight")]
        public Weight Weight { get; set; }

        /// <summary>
        /// Gets or sets driving license  validation rule.
        /// </summary>
        /// <value>
        /// Driving license validation rule.
        /// </value>
        [JsonProperty("drivingLicenseCategory")]
        public DrivingLicenseCategory DrivingLicenseCategory { get; set; }
    }
}
