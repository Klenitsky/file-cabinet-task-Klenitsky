using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Custom weight validator.
    /// </summary>
    public class CustomWeightValidator : IRecordValidator
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

            this.ValidateWeight(arguments);
        }

        private void ValidateWeight(Arguments arguments)
        {
            if ((arguments.Weight < 10) || (arguments.Weight > 20))
            {
                throw new ArgumentException("Invalid height", nameof(arguments));
            }
        }
    }
}
