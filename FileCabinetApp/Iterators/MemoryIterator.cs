using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Iterators
{
    /// <summary>
    /// Iterator for FileCabinetMemoryService.
    /// </summary>
    public class MemoryIterator : IRecordIterator
    {
        private readonly List<FileCabinetRecord> records;
        private int currentIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryIterator"/> class.
        /// </summary>
        /// <param name="records">List of records provided.</param>
        public MemoryIterator(List<FileCabinetRecord> records)
        {
            this.records = records;
            this.currentIndex = 0;
        }

        /// <summary>
        /// Returns next value of the collection.
        /// </summary>
        /// <returns>Record from collection.</returns>
        public FileCabinetRecord GetNext()
        {
            FileCabinetRecord result = this.records[this.currentIndex];
            this.currentIndex++;
            return result;
        }

        /// <summary>
        /// Indicates if there are more records in the cpllection.
        /// </summary>
        /// <returns>Bool indicator.</returns>
        public bool HasMore()
        {
            return this.currentIndex < this.records.Count;
        }
    }
}
