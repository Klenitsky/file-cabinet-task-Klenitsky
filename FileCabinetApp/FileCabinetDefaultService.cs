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
        /// Initializes a new instance of the <see cref="FileCabinetDefaultService"/> class.
        /// Creates a validator with default settings.
        /// </summary>
        /// <returns>A validator for the parameters.</returns>
        public FileCabinetDefaultService()
            : base(new DefaultValidator())
        {
        }
    }
}
