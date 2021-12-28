using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FileCabinetApp.ValidationRules
{
    /// <summary>
    /// Object with rules of validation.
    /// </summary>
    public class ValidationTypes
    {
        /// <summary>
        /// Gets or sets default  validation rules.
        /// </summary>
        /// <value>
        ///  Validation rules.
        /// </value>
            [JsonProperty("default")]
            public Rules Default { get; set; }

        /// <summary>
        /// Gets or sets custom validation rules.
        /// </summary>
        /// <value>
        /// Validation rules.
        /// </value>
            [JsonProperty("custom")]
            public Rules Custom { get; set; }
    }
}
