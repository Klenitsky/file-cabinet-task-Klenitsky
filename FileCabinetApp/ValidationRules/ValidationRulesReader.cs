using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace FileCabinetApp.ValidationRules
{
    /// <summary>
    /// Validation rules reader class.
    /// </summary>
    public static class ValidationRulesReader
    {
        /// <summary>
        /// Reads the rules.
        /// </summary>
        /// <param name="path">Path to read from.</param>
        /// <returns>Validation rules.</returns>
        public static ValidationTypes ReadRules(string path)
        {
            string jsonText = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<ValidationTypes>(jsonText);
        }
    }
}
