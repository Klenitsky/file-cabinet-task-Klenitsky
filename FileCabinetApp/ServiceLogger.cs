using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

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
        /// Edits an existing record.
        /// </summary>
        /// <param name="id">The ID of a record.</param>
        /// <param name="arguments">Properties of the record.</param>
        /// <exception cref="ArgumentNullException">One of the parameters is null.</exception>
        /// <exception cref="ArgumentException">One of the parameters is not valid.</exception>
        public void EditRecord(int id, Arguments arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            this.writer.Write(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + " Calling Edit() with ");
            this.writer.Write("Id= '" + id + "', ");
            this.writer.Write("FirstName= '" + arguments.FirstName + "', ");
            this.writer.Write("Lastname= '" + arguments.LastName + "', ");
            this.writer.Write("DateOfBirth= '" + arguments.DateOfBirth + "', ");
            this.writer.Write("Height= '" + arguments.Height + "', ");
            this.writer.Write("Weight= '" + arguments.Weight + "', ");
            this.writer.WriteLine("DrivingLicenseCategory= '" + arguments.DrivingLicenseCategory + "'");
            this.service.EditRecord(id, arguments);
        }

        /// <summary>
        /// Finds all records with given date of Birth.
        /// </summary>
        /// <param name="dateTime">The date of birth of the person.</param>
        /// <returns>A list of records found.</returns>
        public IReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateTime)
        {
            this.writer.Write(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + " Calling FindByDateOfBirth() with ");
            this.writer.WriteLine("DateOfBirth= '" + dateTime + "'");
            IReadOnlyCollection<FileCabinetRecord> result = this.service.FindByDateOfBirth(dateTime);
            this.writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + " FindByDateOfBirth() returned '" + result.ToString() + "'");
            return result;
        }

        /// <summary>
        /// Finds all records with given firstname.
        /// </summary>
        /// <param name="firstName">The first name of the person.</param>
        /// <returns>A list of records found.</returns>
        /// <exception cref="ArgumentNullException">String firstName is null.</exception>
        public IReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            this.writer.Write(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + " Calling FindByFirstName() with ");
            this.writer.WriteLine("FirstName= '" + firstName + "'");
            IReadOnlyCollection<FileCabinetRecord> result = this.service.FindByFirstName(firstName);
            this.writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + " FindByFirstName() returned '" + result.ToString() + "'");
            return result;
        }

        /// <summary>
        /// Finds all records with given lastname.
        /// </summary>
        /// <param name="lastName">The last name of the person.</param>
        /// <returns>A list of records found.</returns>
        /// <exception cref="ArgumentNullException">String firstName is null.</exception>
        public IReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            this.writer.Write(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + " Calling FindByLastName() with ");
            this.writer.WriteLine("FirstName= '" + lastName + "'");
            IReadOnlyCollection<FileCabinetRecord> result = this.service.FindByLastName(lastName);
            this.writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + " FindByLastName() returned '" + result.ToString() + "'");
            return result;
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
        /// Removes a record.
        /// </summary>
        /// <param name="id">Id of a record to remove.</param>
        /// <returns>A bool result of removing.</returns>
        public bool Remove(int id)
        {
            this.writer.Write(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + " Calling Remove() with ");
            this.writer.WriteLine("Id= '" + id + "'");
            bool result = this.service.Remove(id);
            this.writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture) + " Remove() returned '" + result + "'");
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
