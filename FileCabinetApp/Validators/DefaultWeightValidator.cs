using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Default weight validator.
    /// </summary>
    public class DefaultWeightValidator : IRecordValidator
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
            if ((arguments.Weight < 0) || (arguments.Weight > 180))
            {
                throw new ArgumentException("Invalid height", nameof(arguments));
            }
        }
    }
}
