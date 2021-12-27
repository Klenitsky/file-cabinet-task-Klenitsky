using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Interface of printing.
    /// </summary>
    public interface IRecordPrinter
    {
        /// <summary>
        /// Prints the records.
        /// </summary>
        /// <param name="records">Records provided.</param>
        public void Print(IEnumerable<FileCabinetRecord> records);
    }
}
