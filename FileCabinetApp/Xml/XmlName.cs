using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Calss of xml name.
    /// </summary>
    [Serializable]
    public class XmlName
    {
        /// <summary>
        /// Gets or sets firstname.
        /// </summary>
        /// <value>
        /// Firstname.
        /// </value>
        [XmlAttribute("first")]
        public string First { get; set; }

        /// <summary>
        /// Gets or sets last.
        /// </summary>
        /// <value>
        /// LastName.
        /// </value>
        [XmlAttribute("last")]
        public string Last { get; set; }
    }
}
