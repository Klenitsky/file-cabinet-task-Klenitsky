using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Newtonsoft.Json;

namespace FileCabinetApp.ValidationRules.SpecifiedRules
{
    /// <summary>
    /// Driving license category rule.
    /// </summary>
    public class DrivingLicenseCategory
    {
        /// <summary>
        /// Gets or sets driving license category possible values.
        /// </summary>
        /// <value>
        /// Driving license category possible values.
        /// </value>
        [JsonProperty("values")]
        public ReadOnlyCollection<string> Values { get; set; } = default(ReadOnlyCollection<string>);
    }
}
