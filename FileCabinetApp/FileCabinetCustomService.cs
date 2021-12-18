using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Custom Service.
    /// </summary>
    internal class FileCabinetCustomService : FileCabinetService
    {
        /// <summary>
        /// Creates a validator with custom settings.
        /// </summary>
        /// <returns>A validator for the parameters.</returns>
        protected override IRecordValidator CreateValidator()
        {
            return new CustomValidator();
        }
    }
}
