using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// XmlRecords array class.
    /// </summary>
    [Serializable]
    [XmlRoot(Namespace = "", IsNullable = false, ElementName = "records")]
    public class XmlRecords
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlRecords"/> class.
        /// </summary>
        public XmlRecords()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlRecords"/> class.
        /// </summary>
        /// <param name="records">List of records provided.</param>
        public XmlRecords(List<XmlRecord> records)
        {
            this.Records = records;
        }

        /// <summary>
        /// Gets list of xml records.
        /// </summary>
        /// <value>
        /// List of xml records.
        /// </value>
        [XmlElement("record")]
        public List<XmlRecord> Records { get; } = new List<XmlRecord>();
    }
}
