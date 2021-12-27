using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CompositeValidator"/> class.
    /// </summary>
    public class CompositeValidator : IRecordValidator
    {
        private readonly List<IRecordValidator> validators;
        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeValidator"/> class.
        /// </summary>
        /// <param name="validators"> Validators provided.</param>
        protected CompositeValidator(List<IRecordValidator> validators)
        {
            this.validators = validators;
        }

        /// <summary>
        /// Validates the parameters.
        /// </summary>
        /// <param name="arguments"> Arguments provided.</param>
        public void ValidateParameters(Arguments arguments)
        {
           foreach (var validator in this.validators)
            {
                validator.ValidateParameters(arguments);
            }
        }
    }
}
