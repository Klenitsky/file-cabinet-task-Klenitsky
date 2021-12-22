using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;

namespace FileCabinetApp
{
    /// <summary>
    /// Writes the information in csv file.
    /// </summary>
    public class FileCabinetRecordXmlWriter
    {
        private readonly XmlWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlWriter"/> class.
        /// </summary>
        /// <param name="writer">A writer to work with.</param>
        public FileCabinetRecordXmlWriter(XmlWriter writer)
        {
            this.writer = writer;
        }

        /// <summary>
        /// Write the information about record.
        /// </summary>
        /// <param name="record">A record to write.</param>
        public void Write(FileCabinetRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            this.writer.WriteStartElement("record");
            this.writer.Flush();
            this.writer.WriteStartAttribute("id");
            this.writer.Flush();
            this.writer.WriteValue(record.Id);
            this.writer.Flush();
            this.writer.WriteEndAttribute();
            this.writer.Flush();

            this.writer.WriteStartElement("name");
            this.writer.Flush();
            this.writer.WriteStartAttribute("first");
            this.writer.Flush();
            this.writer.WriteValue(record.FirstName);
            this.writer.Flush();
            this.writer.WriteEndAttribute();
            this.writer.Flush();

            this.writer.WriteStartAttribute("last");
            this.writer.Flush();
            this.writer.WriteValue(record.LastName);
            this.writer.Flush();
            this.writer.WriteEndAttribute();
            this.writer.Flush();
            this.writer.WriteEndElement();
            this.writer.Flush();

            this.writer.WriteStartElement("dateOfBirth");
            this.writer.Flush();
            this.writer.WriteValue(record.DateOfBirth.ToString("d", CultureInfo.InvariantCulture));
            this.writer.Flush();
            this.writer.WriteEndElement();
            this.writer.Flush();

            this.writer.WriteStartElement("Height");
            this.writer.Flush();
            this.writer.WriteValue(record.Height);
            this.writer.Flush();
            this.writer.WriteEndElement();
            this.writer.Flush();

            this.writer.WriteStartElement("Weight");
            this.writer.Flush();
            this.writer.WriteValue(record.Weight);
            this.writer.Flush();
            this.writer.WriteEndElement();
            this.writer.Flush();

            this.writer.WriteStartElement("DrivingLicenseCategory");
            this.writer.Flush();
            this.writer.WriteValue(record.DrivingLicenseCategory);
            this.writer.Flush();
            this.writer.WriteEndElement();
            this.writer.Flush();

            this.writer.WriteEndElement();
            this.writer.Flush();
        }
    }
}
