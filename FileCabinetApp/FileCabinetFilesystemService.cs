using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Service that works with FileStream.
    /// </summary>
    public class FileCabinetFilesystemService: IFileCabinetService
    {
        private readonly FileStream fileStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="fileStream">Filestream providet.</param>
        public FileCabinetFilesystemService(FileStream fileStream)
        {
            this.fileStream = fileStream;
        }

        /// <summary>
        /// Creates a new Record.
        /// </summary>
        /// <param name="arguments">Properties of the record.</param>
        /// <returns>New record's Id.</returns>
        public int CreateRecord(Arguments arguments)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a copy of the list.
        /// </summary>
        /// <returns>Array of records.</returns>
        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a number of records.
        /// </summary>
        /// <returns>Number of records.</returns>
        public int GetStat()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Edits an existing record.
        /// </summary>
        /// <param name="id">The ID of a record.</param>
        /// <param name="arguments">Properties of the record.</param>
        /// <exception cref="ArgumentNullException">One of the parameters is null.</exception>
        /// <exception cref="ArgumentException">One of the parameters is not valid.</exception>
        public void EditRecord(int id, Arguments arguments)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds all records with given firstname.
        /// </summary>
        /// <param name="firstName">The first name of the person.</param>
        /// <returns>A list of records found.</returns>
        /// <exception cref="ArgumentNullException">String firstName is null.</exception>
        public IReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds all records with given lastname.
        /// </summary>
        /// <param name="lastName">The last name of the person.</param>
        /// <returns>A list of records found.</returns>
        /// <exception cref="ArgumentNullException">String firstName is null.</exception>
        public IReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds all records with given date of Birth.
        /// </summary>
        /// <param name="dateTime">The date of birth of the person.</param>
        /// <returns>A list of records found.</returns>
        public IReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generates snapshot of the service.
        /// </summary>
        /// <returns>A snapshot of this service.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            throw new NotImplementedException();
        }
    }
}
