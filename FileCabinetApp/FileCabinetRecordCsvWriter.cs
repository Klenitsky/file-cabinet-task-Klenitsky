using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Writes the information in csv file.
    /// </summary>
    public class FileCabinetRecordCsvWriter
    {
        private readonly TextWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvWriter"/> class.
        /// </summary>
        /// <param name="writer">A writer to work with.</param>
        public FileCabinetRecordCsvWriter(TextWriter writer)
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

            this.writer.Write(record.Id + ",");
            this.writer.Write(record.FirstName + ",");
            this.writer.Write(record.LastName + ",");
            this.writer.Write(record.DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + ",");
            this.writer.Write(record.Height + ",");
            this.writer.Write(record.Weight + ",");
            this.writer.WriteLine(record.DrivingLicenseCategory);
        }
    }
}
