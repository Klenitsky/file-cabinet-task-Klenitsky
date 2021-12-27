using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Validator of weight.
    /// </summary>
    public class WeightValidator : IRecordValidator
    {
        private readonly decimal minValue;
        private readonly decimal maxValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeightValidator"/> class.
        /// </summary>
        /// <param name="min">min length.</param>
        /// <param name="max">max length.</param>
        public WeightValidator(decimal min, decimal max)
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

            if ((arguments.Weight < this.minValue) || (arguments.Weight > this.maxValue))
            {
                throw new ArgumentException("Invalid weight", nameof(arguments));
            }
        }
    }
}
