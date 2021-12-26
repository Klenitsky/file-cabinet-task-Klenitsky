using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Xml Record  class.
    /// </summary>
    [Serializable]
    public class XmlRecord
    {
        /// <summary>
        /// Gets or sets id .
        /// </summary>
        /// <value>
        /// Id.
        /// </value>
        [XmlAttribute("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Name .
        /// </summary>
        /// <value>
        /// Name.
        /// </value>
        [XmlElement("name")]
        public XmlName Name { get; set; } = new XmlName();

        /// <summary>
        /// Gets or sets Date of Birth .
        /// </summary>
        /// <value>
        /// Date of birth.
        /// </value>
        [XmlElement("dateOfBirth")]
        public string DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets height.
        /// </summary>
        /// <value>
        /// Height.
        /// </value>
        [XmlElement("height")]
        public short Height { get; set; }

        /// <summary>
        /// Gets or sets weight.
        /// </summary>
        /// <value>
        /// Weight.
        /// </value>
        [XmlElement("weight")]
        public decimal Weight { get; set; }

        /// <summary>
        /// Gets or sets driving license category.
        /// </summary>
        /// <value>
        /// Driving license category.
        /// </value>
        [XmlElement("drivingLicenseCategory")]
        public char DrivingLicenseCategory { get; set; }
    }
}
