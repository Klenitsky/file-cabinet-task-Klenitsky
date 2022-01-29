using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FileCabinetApp.Consts;

namespace FileCabinetApp.Iterators
{
    /// <summary>
    /// Iterator for FileCabinetMemoryService.
    /// </summary>
    public class FilesystemIterator : IRecordIterator
    {
        private readonly FileStream fileStream;
        private readonly List<long> positions;
        private int currentPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilesystemIterator"/> class.
        /// </summary>
        /// <param name="positions">"List of positions in the file.</param>
        /// <param name="reader">Reader provided.</param>
        public FilesystemIterator(List<long> positions, FileStream reader)
        {
            this.fileStream = reader;
            this.positions = positions;
            this.currentPosition = 0;
        }

        /// <summary>
        /// Gets the Enumerator.
        /// </summary>
        /// <returns>Enumerator.</returns>
        public IEnumerator<FileCabinetRecord> GetEnumerator()
        {
            while (this.HasMore())
            {
                yield return this.GetNext();
            }
        }

        /// <summary>
        /// Returns next value of the collection.
        /// </summary>
        /// <returns>Record from collection.</returns>
        public FileCabinetRecord GetNext()
        {

            this.fileStream.Seek(this.positions[this.currentPosition], SeekOrigin.Begin);
            BinaryReader reader = new BinaryReader(this.fileStream);
            FileCabinetRecord record;
            short status = reader.ReadInt16();
            int recordId = reader.ReadInt32();
            string firstName = reader.ReadString();
            string lastName = reader.ReadString();
            DateTime dateOfBirth = new DateTime(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
            short height = reader.ReadInt16(); ;
            decimal weight = reader.ReadDecimal();
            char drivingLicenseCategory = reader.ReadChar();

            record = new FileCabinetRecord
                {
                    Id = recordId,
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = dateOfBirth,
                    Height = height,
                    Weight = weight,
                    DrivingLicenseCategory = drivingLicenseCategory,
                };
            this.currentPosition++;
            return record;
        }

        /// <summary>
        /// Indicates if there are more records in the cpllection.
        /// </summary>
        /// <returns>Bool indicator.</returns>
        public bool HasMore()
        {
            return this.currentPosition < this.positions.Count;
        }

        /// <summary>
        /// Gets the Enumerator.
        /// </summary>
        /// <returns>Enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
