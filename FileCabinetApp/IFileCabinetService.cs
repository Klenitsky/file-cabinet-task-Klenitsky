using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.Iterators;

namespace FileCabinetApp
{
    /// <summary>
    /// Interface for services.
    /// </summary>
    public interface IFileCabinetService
    {
        /// <summary>
        /// Creates a new Record.
        /// </summary>
        /// <param name="arguments">Properties of the record.</param>
        /// <returns>New record's Id.</returns>
        public int CreateRecord(Arguments arguments);

        /// <summary>
        /// Inserts a new Record.
        /// </summary>
        /// <param name="id">Id of a record.</param>
        /// <param name="arguments">Properties of the record.</param>
        /// <returns>New record's Id.</returns>
        public int InsertRecord(int id, Arguments arguments);

        /// <summary>
        /// Gets a copy of the list.
        /// </summary>
        /// <returns>Array of records.</returns>
        public IReadOnlyCollection<FileCabinetRecord> GetRecords();

        /// <summary>
        /// Gets a number of records.
        /// </summary>
        /// <returns>Number of records.</returns>
        public int GetStat();

        /// <summary>
        /// Finds all records with given firstname.
        /// </summary>
        /// <param name="firstName">The first name of the person.</param>
        /// <returns>Iterator on records.</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>
        /// Finds all records with given lastname.
        /// </summary>
        /// <param name="lastName">The last name of the person.</param>
        /// <returns>Iterator on records.</returns>
        /// <exception cref="ArgumentNullException">String firstName is null.</exception>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName);

        /// <summary>
        /// Finds all records with given date of Birth.
        /// </summary>
        /// <param name="dateTime">The date of birth of the person.</param>
        /// <returns>Iterator on records.</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime dateTime);

        /// <summary>
        /// Generates snapshot of the service.
        /// </summary>
        /// <returns>A snapshot of this service.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot();

        /// <summary>
        /// Adds records loaded from file.
        /// </summary>
        /// <param name="snapshot">Properties of the record.</param>
        public void Restore(FileCabinetServiceSnapshot snapshot);

        /// <summary>
        /// Removes a record.
        /// </summary>
        /// <param name="arguments">Properties of values to delete.</param>
        /// <returns>Deleted values.</returns>
        public IEnumerable<FileCabinetRecord> Delete(SearchingAttributes arguments);

        /// <summary>
        /// Updates a record.
        /// </summary>
        /// <param name="attriubutesToUpdate">Properties of values to update records.</param>
        /// <param name="attriubutesToFind">Properties of values to find records.</param>
        /// <returns>Updated values.</returns>
        public IEnumerable<FileCabinetRecord> Update(IEnumerable<SearchingAttributes> attriubutesToUpdate, IEnumerable<SearchingAttributes> attriubutesToFind);

        /// <summary>
        /// Gets the Id of the last record.
        /// </summary>
        /// <returns>Int id.</returns>
        public int GetID();

        /// <summary>
        /// Purges deleted records.
        /// </summary>
        /// <returns>Amount of purged records.</returns>
        public int Purge();

        /// <summary>
        /// Gets the number of deleted records.
        /// </summary>
        /// <returns>Int id.</returns>
        public int GetDeletedStat();

        /// <summary>
        /// Updates a record.
        /// </summary>
        /// <param name="attriubutesToFind">Properties of values to find records.</param>
        /// <param name="complexAttribute">Or or and.</param>
        /// <returns>Updated values.</returns>
        public IEnumerable<FileCabinetRecord> Select(IEnumerable<SearchingAttributes> attriubutesToFind, string complexAttribute);
    }
}
