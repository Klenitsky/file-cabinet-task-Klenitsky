using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FileCabinetApp.ValidationRules.SpecifiedRules
{
    /// <summary>
    /// Date of birth rule.
    /// </summary>
    public class DateOfBirth
    {
        /// <summary>
        /// Gets or sets date of birth min value.
        /// </summary>
        /// <value>
        /// Date of birth min value.
        /// </value>
        [JsonProperty("from")]
        public string Min { get; set; }

        /// <summary>
        /// Gets or sets date of birth max value.
        /// </summary>
        /// <value>
        /// Date of birth max value.
        /// </value>
        [JsonProperty("to")]
        public string Max { get; set; }
    }
}
