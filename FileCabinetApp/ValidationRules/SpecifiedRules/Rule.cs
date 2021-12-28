using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FileCabinetApp.ValidationRules.SpecifiedRules
{
    /// <summary>
    /// Default rule type.
    /// </summary>
    /// <typeparam name = "T">
    /// type of rule.
    /// </typeparam>
    public abstract class Rule<T>
    {
        /// <summary>
        /// Gets or sets rule min value.
        /// </summary>
        /// <value>
        /// Rule min value.
        /// </value>
        [JsonProperty("min")]
        public T MinValue { get; set; }

        /// <summary>
        /// Gets or sets rule max value.
        /// </summary>
        /// <value>
        /// Rule max value.
        /// </value>
        [JsonProperty("max")]
        public T MaxValue { get; set; }
    }
}
