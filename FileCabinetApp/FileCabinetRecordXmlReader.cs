using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Reads the record from csv file.
    /// </summary>
    public class FileCabinetRecordXmlReader
    {
        private readonly StreamReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlReader"/> class.
        /// </summary>
        /// <param name="reader"> Reader provided.</param>
        public FileCabinetRecordXmlReader(StreamReader reader)
        {
            this.reader = reader;
        }

        /// <summary>
        /// Reads records from the file.
        /// </summary>
        /// <returns> List of records.</returns>
        public IList<FileCabinetRecord> ReadAll()
        {
            var serializer = new XmlSerializer(typeof(XmlRecords));

            using var xmlReader = XmlReader.Create(this.reader);

            XmlRecords records = (XmlRecords)serializer.Deserialize(xmlReader);

            var result = new List<FileCabinetRecord>();

            foreach (XmlRecord item in records.Records)
            {
                result.Add(MakeRecord(item));
            }

            return result;
        }

        private static FileCabinetRecord MakeRecord(XmlRecord record)
        {
            return new FileCabinetRecord
            {
                Id = record.Id,
                FirstName = record.Name.First,
                LastName = record.Name.Last,
                DateOfBirth = DateTime.ParseExact(record.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Height = record.Height,
                Weight = record.Weight,
                DrivingLicenseCategory = record.DrivingLicenseCategory,
            };
        }
    }
}
