using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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
        /// Edits an existing record.
        /// </summary>
        /// <param name="id">The ID of a record.</param>
        /// <param name="arguments">Properties of the record.</param>
        /// <exception cref="ArgumentNullException">One of the parameters is null.</exception>
        /// <exception cref="ArgumentException">One of the parameters is not valid.</exception>
        public void EditRecord(int id, Arguments arguments)
        {
            Stopwatch elapsedTime = Stopwatch.StartNew();
            this.service.EditRecord(id, arguments);
            elapsedTime.Stop();
            Console.WriteLine("Edit method execution duration is " + elapsedTime.ElapsedTicks + " ticks.");
        }

        /// <summary>
        /// Finds all records with given date of Birth.
        /// </summary>
        /// <param name="dateTime">The date of birth of the person.</param>
        /// <returns>A list of records found.</returns>
        public IRecordIterator FindByDateOfBirth(DateTime dateTime)
        {
            Stopwatch elapsedTime = Stopwatch.StartNew();
            IRecordIterator result = this.service.FindByDateOfBirth(dateTime);
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
        public IRecordIterator FindByFirstName(string firstName)
        {
            Stopwatch elapsedTime = Stopwatch.StartNew();
            IRecordIterator result = this.service.FindByFirstName(firstName);
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
        public IRecordIterator FindByLastName(string lastName)
        {
            Stopwatch elapsedTime = Stopwatch.StartNew();
            IRecordIterator result = this.service.FindByLastName(lastName);
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
        /// Removes a record.
        /// </summary>
        /// <param name="id">Id of a record to remove.</param>
        /// <returns>A bool result of removing.</returns>
        public bool Remove(int id)
        {
            Stopwatch elapsedTime = Stopwatch.StartNew();
            bool result = this.service.Remove(id);
            elapsedTime.Stop();
            Console.WriteLine("Remove method execution duration is " + elapsedTime.ElapsedTicks + " ticks.");
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
    }
}
