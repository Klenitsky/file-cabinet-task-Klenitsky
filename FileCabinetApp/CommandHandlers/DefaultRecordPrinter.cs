using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Interface of printing.
    /// </summary>
    public class DefaultRecordPrinter : IRecordPrinter
    {
        /// <summary>
        /// Prints the records.
        /// </summary>
        /// <param name="records">Records provided.</param>
        public void Print(IEnumerable<FileCabinetRecord> records)
        {
            if (records == null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            foreach (FileCabinetRecord record in records)
            {
                Console.WriteLine(record.ToString());
            }
        }
    }
}
