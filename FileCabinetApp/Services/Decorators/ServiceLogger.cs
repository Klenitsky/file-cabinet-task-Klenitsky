using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.Iterators;

namespace FileCabinetApp
{
    /// <summary>
    /// Decorator that writes logs.
    /// </summary>
    public class ServiceLogger : IFileCabinetService, IDisposable
    {
        private readonly IFileCabinetService service;
        private readonly TextWriter writer;
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLogger"/> class.
        /// </summary>
        /// <param name="service">Service to be decorated.</param>
        /// <param name="filename">File to save logs.</param>
        public ServiceLogger(IFileCabinetService service, string filename)
        {
            this.service = service;
            this.writer = new StreamWriter(File.OpenWrite(filename)) { AutoFlush = true };
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
            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            this.writer.Write(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + " Calling Create() with ");
            this.writer.Write("FirstName= '" + arguments.FirstName + "', ");
            this.writer.Write("Lastname= '" + arguments.LastName + "', ");
            this.writer.Write("DateOfBirth= '" + arguments.DateOfBirth + "', ");
            this.writer.Write("Height= '" + arguments.Height + "', ");
            this.writer.Write("Weight= '" + arguments.Weight + "', ");
            this.writer.WriteLine("DrivingLicenseCategory= '" + arguments.DrivingLicenseCategory + "'");
            int id = this.service.CreateRecord(arguments);
            this.writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + " Create() returned '" + id + "'");
            return id;
        }

        /// <summary>
        /// Gets the number of deleted records.
        /// </summary>
        /// <returns>Int id.</returns>
        public int GetDeletedStat()
        {
            this.writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + " Calling GetDeletedStat()");
            int result = this.service.GetDeletedStat();
            this.writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + " GetDeletedStat() returned '" + result + "'");
            return result;
        }

        /// <summary>
        /// Gets the Id of the last record.
        /// </summary>
        /// <returns>Int id.</returns>
        public int GetID()
        {
            this.writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + " Calling GetID()");
            int result = this.service.GetID();
            this.writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + " GetID() returned '" + result + "'");
            return result;
        }

        /// <summary>
        /// Gets a copy of the list.
        /// </summary>
        /// <returns>Array of records.</returns>
        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            this.writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + " Calling GetRecords()");
            IReadOnlyCollection<FileCabinetRecord> result = this.service.GetRecords();
            this.writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + " GetRecords() returned '" + result.ToString() + "'");
            return result;
        }

        /// <summary>
        /// Gets a number of records.
        /// </summary>
        /// <returns>Number of records.</returns>
        public int GetStat()
        {
            this.writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + " Calling GetStat()");
            int result = this.service.GetStat();
            this.writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + " GetStat() returned '" + result + "'");
            return result;
        }

        /// <summary>
        /// Generates snapshot of the service.
        /// </summary>
        /// <returns>A snapshot of this service.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            this.writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + " Calling MakeSnapshot()");
            FileCabinetServiceSnapshot result = this.service.MakeSnapshot();
            this.writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + " MakeSnapshot() returned new snapshot");
            return result;
        }

        /// <summary>
        /// Purges deleted records.
        /// </summary>
        /// <returns>Amount of purged values.</returns>
        public int Purge()
        {
            this.writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + " Calling Purge()");
            int result = this.service.Purge();
            this.writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + " Purge() returned '" + result + "'");
            return result;
        }

        /// <summary>
        /// Adds records loaded from file.
        /// </summary>
        /// <param name="snapshot">Properties of the record.</param>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            if (snapshot == null)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }

            this.writer.Write(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + " Calling Restore() with ");
            this.writer.WriteLine("Snapshot= '" + snapshot.ToString() + "'");
            this.service.Restore(snapshot);
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

            this.writer.Write(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + " Calling Insert() with ");
            this.writer.WriteLine("Id= '" + id + "'");
            this.writer.Write("FirstName= '" + arguments.FirstName + "', ");
            this.writer.Write("Lastname= '" + arguments.LastName + "', ");
            this.writer.Write("DateOfBirth= '" + arguments.DateOfBirth + "', ");
            this.writer.Write("Height= '" + arguments.Height + "', ");
            this.writer.Write("Weight= '" + arguments.Weight + "', ");
            this.writer.WriteLine("DrivingLicenseCategory= '" + arguments.DrivingLicenseCategory + "'");
            int result = this.service.InsertRecord(id, arguments);
            this.writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + " Insert() returned '" + result + "'");
            return result;
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

            this.writer.Write(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + " Calling Delete() with ");
            this.writer.WriteLine("Attributes= '" + arguments.Value + "', ");
            IEnumerable<FileCabinetRecord> result = this.service.Delete(arguments);
            this.writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + " Insert() returned '" + result + "'");
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

            this.writer.Write(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + " Calling Update() with ");
            foreach (SearchingAttributes attribute in attriubutesToUpdate)
            {
                this.writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + "Attributes to update: " + attribute.Attribute + " " + "'" + attribute.Value + "'");
            }

            foreach (SearchingAttributes attribute in attriubutesToFind)
            {
                this.writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + "Attributes to find: " + attribute.Attribute + " " + "'" + attribute.Value + "'");
            }

            IEnumerable<FileCabinetRecord> result = this.service.Update(attriubutesToUpdate, attriubutesToFind);
            this.writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + " Insert() returned:");
            foreach (var record in result)
            {
                this.writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + "'" + record.ToString() + "'");
            }

            return result;
        }

        /// <summary>
        /// Updates a record.
        /// </summary>
        /// <param name="attriubutesToFind">Properties of values to find records.</param>
        /// <param name="complexAttribute">Or or and.</param>
        /// <returns>Updated values.</returns>
        public IEnumerable<FileCabinetRecord> SelectRecords(IEnumerable<SearchingAttributes> attriubutesToFind, string complexAttribute)
        {
            if (attriubutesToFind == null)
            {
                throw new ArgumentNullException(nameof(attriubutesToFind));
            }

            this.writer.Write(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + " Calling Select() with ");
            foreach (SearchingAttributes attribute in attriubutesToFind)
            {
                this.writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + "Attributes to find: " + attribute.Attribute + " " + "'" + attribute.Value + "'");
            }

            this.writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + "keyword: " + "'" + complexAttribute + "'");
            var result = this.service.SelectRecords(attriubutesToFind, complexAttribute);
            this.writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + " Select() returned:");
            foreach (var record in result)
            {
                this.writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + "'" + record.ToString() + "'");
            }

            return result;
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="value">Bool flag of disposion.</param>
        protected virtual void Dispose(bool value)
        {
            if (!this.disposed)
            {
                if (value)
                {
                    this.writer.Dispose();
                }

                this.disposed = true;
            }
        }
    }
}
