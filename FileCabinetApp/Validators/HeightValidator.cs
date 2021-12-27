using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Validator of height.
    /// </summary>
    public class HeightValidator : IRecordValidator
    {
        private readonly int minValue;
        private readonly int maxValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeightValidator"/> class.
        /// </summary>
        /// <param name="min">min length.</param>
        /// <param name="max">max length.</param>
        public HeightValidator(int min, int max)
        {
            this.minValue = min;
            this.maxValue = max;
        }

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

            if ((arguments.Height < this.minValue) || (arguments.Height > this.maxValue))
            {
                throw new ArgumentException("Invalid height", nameof(arguments));
            }
        }
    }
}
