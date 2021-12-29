using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Iterators
{
    /// <summary>
    /// Iterator interface.
    /// </summary>
    public interface IRecordIterator
    {
        /// <summary>
        /// Returns next value of the collection.
        /// </summary>
        /// <returns>Record from collection.</returns>
        public FileCabinetRecord GetNext();

        /// <summary>
        /// Indicates if there are more records in the cpllection.
        /// </summary>
        /// <returns>Bool indicator.</returns>
        public bool HasMore();

    }
}
