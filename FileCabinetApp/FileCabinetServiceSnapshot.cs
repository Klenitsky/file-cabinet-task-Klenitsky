using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace FileCabinetApp
{
    /// <summary>
    /// Snapshot of a FileCabinetService.
    /// </summary>
    public class FileCabinetServiceSnapshot
    {
        private FileCabinetRecord[] list;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="lst">List of records from service.</param>
        public FileCabinetServiceSnapshot(List<FileCabinetRecord> lst)
        {
            if (lst == null)
            {
                throw new ArgumentNullException(nameof(lst));
            }

            this.list = lst.ToArray();
        }

        /// <summary>
        /// Gets or sets records to load.
        /// </summary>
        /// <value>
        /// Collecton of records.
        /// </value>
        public IReadOnlyCollection<FileCabinetRecord> Records { get; set; }

        /// <summary>
        /// Saves the information in csv format.
        /// </summary>
        /// <param name="stream">Stream for saving.</param>
        public void SaveToCSV(StreamWriter stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            FileCabinetRecordCsvWriter writer = new FileCabinetRecordCsvWriter(stream);
            stream.WriteLine("Id,First Name, Last Name,Date Of Birth,Height,Weight,Driving License Category");
            foreach (FileCabinetRecord record in this.list)
            {
                writer.Write(record);
            }
        }

        /// <summary>
        /// Saves the information in xml format.
        /// </summary>
        /// <param name="stream">Stream for saving.</param>
        public void SaveToXml(StreamWriter stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            XmlWriterSettings settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                Indent = true,
                IndentChars = "\t",
                NewLineHandling = NewLineHandling.Replace,
            };
            XmlWriter xmlWriter = XmlWriter.Create(stream, settings);
            FileCabinetRecordXmlWriter writer = new FileCabinetRecordXmlWriter(xmlWriter);
            xmlWriter.WriteStartElement("records");
            xmlWriter.Flush();
            foreach (FileCabinetRecord record in this.list)
            {
                writer.Write(record);
            }

            xmlWriter.WriteEndElement();
            xmlWriter.Flush();
        }

        /// <summary>
        /// Saves the information in xml format.
        /// </summary>
        /// <param name="stream">Stream for reading.</param>
        public void LoadFromCsv(StreamReader stream)
        {
            FileCabinetRecordCsvReader reader = new FileCabinetRecordCsvReader(stream);
            IList<FileCabinetRecord> listOfRecords = reader.ReadAll();
            this.list = new FileCabinetRecord[listOfRecords.Count];
            listOfRecords.CopyTo(this.list, 0);
            this.Records = (IReadOnlyCollection<FileCabinetRecord>)listOfRecords;
        }
    }
}
