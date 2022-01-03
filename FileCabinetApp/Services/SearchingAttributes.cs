using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
/// <summary>
/// Attributes to search.
/// </summary>
    public class SearchingAttributes
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchingAttributes"/> class.
        /// </summary>
        /// <param name="attribute">attributes.</param>
        /// <param name="value">value of attribute.</param>
        public SearchingAttributes(string attribute, string value)
        {
            this.Attribute = Enum.Parse<AttributesSearch>(attribute, true);
            this.Value = value;
        }

        /// <summary>
        /// Enum of attributes.
        /// </summary>
        public enum AttributesSearch
        {
            /// <summary>
            /// Id attribute.
            /// </summary>
            Id,

            /// <summary>
            /// FirstName attribute.
            /// </summary>
            FirstName,

            /// <summary>
            /// LastName attribute.
            /// </summary>
            LastName,

            /// <summary>
            /// DateOfBirth attribute.
            /// </summary>
            DateOfBirth,

            /// <summary>
            /// Height attribute.
            /// </summary>
            Height,

            /// <summary>
            /// Weight attribute.
            /// </summary>
            Weight,

            /// <summary>
            /// Driving License attribute.
            /// </summary>
            DrivingLicenseCategory,
        }

        /// <summary>
        /// Gets attribute.
        /// </summary>
        /// <value>attribute name.</value>
        public AttributesSearch Attribute { get; }

        /// <summary>
        /// Gets avalue of an attribute.
        /// </summary>
        /// <value>attribute name.</value>
        public string Value { get; }
    }
}
