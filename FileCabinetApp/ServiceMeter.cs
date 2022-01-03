using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.Iterators;

namespace FileCabinetApp
{
    /// <summary>
    /// Service decorator that counts time.
    /// </summary>
    public class ServiceMeter : IFileCabinetService
    {
        private readonly IFileCabinetService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceMeter"/> class.
        /// </summary>
        /// <param name="service">Service to be decorated.</param>
        public ServiceMeter(IFileCabinetService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Creates a new Record.
        /// </summary>
        /// <param name="arguments">Properties of the record.</param>
        /// <returns>New record's Id.</returns>
        /// <exception cref="ArgumentNullException"> Parameters are null.</exception>
        /// <exception cref="ArgumentException">One of the parameters is not valid.</exception>
        public int CreateRecord(Arguments arguments)
        {
            Stopwatch elapsedTime = Stopwatch.StartNew();
            int id = this.service.CreateRecord(arguments);
            elapsedTime.Stop();
            Console.WriteLine("Create method execution duration is " + elapsedTime.ElapsedTicks + " ticks.");
            return id;
        }

        /// <summary>
        /// Finds all records with given date of Birth.
        /// </summary>
        /// <param name="dateTime">The date of birth of the person.</param>
        /// <returns>A list of records found.</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime dateTime)
        {
            Stopwatch elapsedTime = Stopwatch.StartNew();
            IEnumerable<FileCabinetRecord> result = this.service.FindByDateOfBirth(dateTime);
            elapsedTime.Stop();
            Console.WriteLine("Find method execution duration is " + elapsedTime.ElapsedTicks + " ticks.");
            return result;
        }

        /// <summary>
        /// Finds all records with given firstname.
        /// </summary>
        /// <param name="firstName">The first name of the person.</param>
        /// <returns>A list of records found.</returns>
        /// <exception cref="ArgumentNullException">String firstName is null.</exception>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            Stopwatch elapsedTime = Stopwatch.StartNew();
            IEnumerable<FileCabinetRecord> result = this.service.FindByFirstName(firstName);
            elapsedTime.Stop();
            Console.WriteLine("Find method execution duration is " + elapsedTime.ElapsedTicks + " ticks.");
            return result;
        }

        /// <summary>
        /// Finds all records with given lastname.
        /// </summary>
        /// <param name="lastName">The last name of the person.</param>
        /// <returns>A list of records found.</returns>
        /// <exception cref="ArgumentNullException">String firstName is null.</exception>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            Stopwatch elapsedTime = Stopwatch.StartNew();
            IEnumerable<FileCabinetRecord> result = this.service.FindByLastName(lastName);
            elapsedTime.Stop();
            Console.WriteLine("Find method execution duration is " + elapsedTime.ElapsedTicks + " ticks.");
            return result;
        }

        /// <summary>
        /// Gets the number of deleted records.
        /// </summary>
        /// <returns>Int id.</returns>
        public int GetDeletedStat()
        {
            Stopwatch elapsedTime = Stopwatch.StartNew();
            int result = this.service.GetDeletedStat();
            elapsedTime.Stop();
            Console.WriteLine("Get deleted stat method execution duration is " + elapsedTime.ElapsedTicks + " ticks.");
            return result;
        }

        /// <summary>
        /// Gets the Id of the last record.
        /// </summary>
        /// <returns>Int id.</returns>
        public int GetID()
        {
            Stopwatch elapsedTime = Stopwatch.StartNew();
            int result = this.service.GetID();
            elapsedTime.Stop();
            Console.WriteLine("Get id  method execution duration is " + elapsedTime.ElapsedTicks + " ticks.");
            return result;
        }

        /// <summary>
        /// Gets a copy of the list.
        /// </summary>
        /// <returns>Array of records.</returns>
        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            Stopwatch elapsedTime = Stopwatch.StartNew();
            IReadOnlyCollection<FileCabinetRecord> result = this.service.GetRecords();
            elapsedTime.Stop();
            Console.WriteLine("Get records method execution duration is " + elapsedTime.ElapsedTicks + " ticks.");
            return result;
        }

        /// <summary>
        /// Gets a number of records.
        /// </summary>
        /// <returns>Number of records.</returns>
        public int GetStat()
        {
            Stopwatch elapsedTime = Stopwatch.StartNew();
            int result = this.service.GetStat();
            elapsedTime.Stop();
            Console.WriteLine("Get  stat method execution duration is " + elapsedTime.ElapsedTicks + " ticks.");
            return result;
        }

        /// <summary>
        /// Generates snapshot of the service.
        /// </summary>
        /// <returns>A snapshot of this service.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            Stopwatch elapsedTime = Stopwatch.StartNew();
            FileCabinetServiceSnapshot result = this.service.MakeSnapshot();
            elapsedTime.Stop();
            Console.WriteLine("Make snapshot method execution duration is " + elapsedTime.ElapsedTicks + " ticks.");
            return result;
        }

        /// <summary>
        /// Purges deleted records.
        /// </summary>
        /// <returns>Amount of purged values.</returns>
        public int Purge()
        {
            Stopwatch elapsedTime = Stopwatch.StartNew();
            int result = this.service.Purge();
            elapsedTime.Stop();
            Console.WriteLine("Purge method execution duration is " + elapsedTime.ElapsedTicks + " ticks.");
            return result;
        }

        /// <summary>
        /// Inserts a new Record.
        /// </summary>
        /// <param name="id">Id of the record.</param>
        /// <param name="arguments">Properties of the record.</param>
        /// <returns>New record's Id.</returns>
        public int InsertRecord(int id, Arguments arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            Stopwatch elapsedTime = Stopwatch.StartNew();
            int result = this.service.InsertRecord(id, arguments);
            elapsedTime.Stop();
            Console.WriteLine("Insert method execution duration is " + elapsedTime.ElapsedTicks + " ticks.");
            return result;
        }

        /// <summary>
        /// Adds records loaded from file.
        /// </summary>
        /// <param name="snapshot">Properties of the record.</param>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            Stopwatch elapsedTime = Stopwatch.StartNew();
            this.service.Restore(snapshot);
            elapsedTime.Stop();
            Console.WriteLine("Restore method execution duration is" + elapsedTime.ElapsedTicks + " ticks.");
        }

        /// <summary>
        /// Removes a record.
        /// </summary>
        /// <param name="arguments">Properties of values to delete.</param>
        /// <returns>Deleted values.</returns>
        public IEnumerable<FileCabinetRecord> Delete(SearchingAttributes arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            Stopwatch elapsedTime = Stopwatch.StartNew();
            IEnumerable<FileCabinetRecord> result = this.service.Delete(arguments);
            elapsedTime.Stop();
            Console.WriteLine("Delete method execution duration is" + elapsedTime.ElapsedTicks + " ticks.");
            return result;
        }

        /// <summary>
        /// Updates a record.
        /// </summary>
        /// <param name="attriubutesToUpdate">Properties of values to update records.</param>
        /// <param name="attriubutesToFind">Properties of values to find records.</param>
        /// <returns>Updated values.</returns>
        public IEnumerable<FileCabinetRecord> Update(IEnumerable<SearchingAttributes> attriubutesToUpdate, IEnumerable<SearchingAttributes> attriubutesToFind)
        {
            if (attriubutesToUpdate == null)
            {
                throw new ArgumentNullException(nameof(attriubutesToUpdate));
            }

            if (attriubutesToFind == null)
            {
                throw new ArgumentNullException(nameof(attriubutesToFind));
            }

            Stopwatch elapsedTime = Stopwatch.StartNew();
            IEnumerable<FileCabinetRecord> result = this.service.Update(attriubutesToUpdate, attriubutesToFind);
            elapsedTime.Stop();
            Console.WriteLine("Update method execution duration is" + elapsedTime.ElapsedTicks + " ticks.");

            return result;
        }

        /// <summary>
        /// Updates a record.
        /// </summary>
        /// <param name="attriubutesToFind">Properties of values to find records.</param>
        /// <param name="complexAttribute">Or or and.</param>
        /// <returns>Updated values.</returns>
        public IEnumerable<FileCabinetRecord> Select(IEnumerable<SearchingAttributes> attriubutesToFind, string complexAttribute)
        {
            if (attriubutesToFind == null)
            {
                throw new ArgumentNullException(nameof(attriubutesToFind));
            }

            Stopwatch elapsedTime = Stopwatch.StartNew();
            var result = this.service.Select(attriubutesToFind, complexAttribute);
            elapsedTime.Stop();
            Console.WriteLine("Select method execution duration is" + elapsedTime.ElapsedTicks + " ticks.");

            return result;
        }
    }
}
