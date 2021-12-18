using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Default Service.
    /// </summary>
    internal class FileCabinetDefaultService : FileCabinetService
    {
        /// <summary>
        /// Creates a validator with default settings.
        /// </summary>
        /// <returns>A validator for the parameters.</returns>
        protected override IRecordValidator CreateValidator()
        {
            return new DefaultValidator();
        }
    }
}
