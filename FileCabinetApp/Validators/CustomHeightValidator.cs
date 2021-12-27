using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Custom height validator.
    /// </summary>
    public class CustomHeightValidator : IRecordValidator
    {
        /// <summary>
        /// Checks the parameters.
        /// </summary>
        /// <param name="arguments">Properties of the record.</param>
        public void ValidateParameters(Arguments arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            this.ValidateHeight(arguments);
        }

        private void ValidateHeight(Arguments arguments)
        {
                if ((arguments.Height < 10) || (arguments.Height > 20))
                {
                    throw new ArgumentException("Invalid height", nameof(arguments));
                }
        }
    }
}
