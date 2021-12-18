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
        /// Initializes a new instance of the <see cref="FileCabinetCustomService"/> class.
        /// Creates a validator with custom settings.
        /// </summary>
        /// <returns>A validator for the parameters.</returns>
        public FileCabinetCustomService()
            : base(new DefaultValidator())
        {
        }
    }
}
