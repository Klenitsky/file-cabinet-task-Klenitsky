using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using FileCabinetApp.Consts;
using FileCabinetApp.Iterators;
using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    /// <summary>
    /// Service that works with FileStream.
    /// </summary>
    public class FileCabinetFilesystemService : IFileCabinetService, IDisposable
    {
        private readonly Dictionary<string, List<long>> firstNameDictionary = new Dictionary<string, List<long>>();
        private readonly Dictionary<string, List<long>> lastNameDictionary = new Dictionary<string, List<long>>();
        private readonly Dictionary<string, List<long>> dateOfBirthDictionary = new Dictionary<string, List<long>>();

        private readonly IRecordValidator validator;
        private FileStream fileStream;
        private int id = 1;
        private int deleted;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="fileStream">Filestream provided.</param>
        /// <param name="validator">Validator provided.</param>
        public FileCabinetFilesystemService(FileStream fileStream, IRecordValidator validator)
        {
            this.fileStream = fileStream;
            this.validator = validator;
            this.id = (int)(this.fileStream.Length / FileConsts.RecordSize) + 1;
            this.RemakeDictionaries();
        }

        /// <summary>
        /// Creates a new Record.
        /// </summary>
        /// <param name="arguments">Properties of the record.</param>
        /// <returns>New record's Id.</returns>
        public int CreateRecord(Arguments arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            this.validator.ValidateParameters(arguments);
            short st = 0;
            byte[] status = BitConverter.GetBytes(st);
            byte[] recordId = BitConverter.GetBytes(this.id);
            byte[] firstName = Encoding.UTF8.GetBytes(arguments.FirstName);

            byte[] firstNameResult = new byte[FileConsts.NameSize];
            for (int i = 0; i < firstName.Length; i++)
            {
                firstNameResult[i] = firstName[i];
            }

            byte[] lastName = Encoding.UTF8.GetBytes(arguments.LastName);
            byte[] lastNameResult = new byte[FileConsts.NameSize];
            for (int i = 0; i < lastName.Length; i++)
            {
                lastNameResult[i] = lastName[i];
            }

            byte[] year = BitConverter.GetBytes(arguments.DateOfBirth.Year);
            byte[] month = BitConverter.GetBytes(arguments.DateOfBirth.Month);
            byte[] day = BitConverter.GetBytes(arguments.DateOfBirth.Day);

            byte[] height = BitConverter.GetBytes(arguments.Height);
            byte[] weight = BitConverter.GetBytes(decimal.ToDouble(arguments.Weight));
            byte[] drivingLicenseCategory = BitConverter.GetBytes(arguments.DrivingLicenseCategory);
            this.id++;

            if (!this.firstNameDictionary.ContainsKey(arguments.FirstName))
            {
                this.firstNameDictionary.Add(arguments.FirstName, new List<long>());
            }

            this.firstNameDictionary[arguments.FirstName].Add(this.fileStream.Length);

            if (!this.lastNameDictionary.ContainsKey(arguments.LastName))
            {
                this.lastNameDictionary.Add(arguments.LastName, new List<long>());
            }

            this.lastNameDictionary[arguments.LastName].Add(this.fileStream.Length);

            if (!this.dateOfBirthDictionary.ContainsKey(arguments.DateOfBirth.ToString(CultureInfo.CurrentCulture)))
            {
                this.dateOfBirthDictionary.Add(arguments.DateOfBirth.ToString(CultureInfo.CurrentCulture), new List<long>());
            }

            this.dateOfBirthDictionary[arguments.DateOfBirth.ToString(CultureInfo.CurrentCulture)].Add(this.fileStream.Length);

            this.fileStream.Write(status, 0, status.Length);
            this.fileStream.Write(recordId, 0, recordId.Length);
            this.fileStream.Write(firstNameResult, 0, firstNameResult.Length);
            this.fileStream.Write(lastNameResult, 0, lastNameResult.Length);
            this.fileStream.Write(year, 0, year.Length);
            this.fileStream.Write(month, 0, month.Length);
            this.fileStream.Write(day, 0, day.Length);
            this.fileStream.Write(height, 0, height.Length);
            this.fileStream.Write(weight, 0, weight.Length);
            this.fileStream.Write(drivingLicenseCategory, 0, drivingLicenseCategory.Length);
            this.fileStream.Flush();
            return this.id;
        }

        /// <summary>
        /// Gets a copy of the list.
        /// </summary>
        /// <returns>Array of records.</returns>
        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            if (this.fileStream.Length % FileConsts.RecordSize != 0)
            {
                return result;
            }

            int index = 0;
            this.fileStream.Seek(index, SeekOrigin.Begin);
            while (index < this.fileStream.Length)
            {
                FileCabinetRecord record;
                byte[] buffer = new byte[FileConsts.RecordSize];
                this.fileStream.Read(buffer, 0, buffer.Length);
                byte[] statusBuf = buffer[FileConsts.StatusBegin..FileConsts.IdBegin];
                short status = BitConverter.ToInt16(statusBuf);
                status &= 4;
                if (status == 0)
                {
                byte[] recordIdBuf = buffer[FileConsts.IdBegin..FileConsts.FirstNameBegin];
                byte[] firstNameBuf = buffer[FileConsts.FirstNameBegin..FileConsts.LastNameBegin];
                byte[] lastNameBuf = buffer[FileConsts.LastNameBegin..FileConsts.YearBegin];
                byte[] yearBuf = buffer[FileConsts.YearBegin..FileConsts.MonthBegin];
                byte[] monthBuf = buffer[FileConsts.MonthBegin..FileConsts.DayBegin];
                byte[] dayBuf = buffer[FileConsts.DayBegin..FileConsts.HeightBegin];
                byte[] heightBuf = buffer[FileConsts.HeightBegin..FileConsts.WeightBegin];
                byte[] weightBuf = buffer[FileConsts.WeightBegin..FileConsts.DrivingLicenseCategoryBegin];
                byte[] drivingLicenseCategoryBuf = buffer[FileConsts.DrivingLicenseCategoryBegin..FileConsts.RecordSize];

                int recordId = BitConverter.ToInt32(recordIdBuf);
                string firstName = Encoding.UTF8.GetString(firstNameBuf);
                string lastName = Encoding.UTF8.GetString(lastNameBuf);
                DateTime dateOfBirth = new DateTime(BitConverter.ToInt32(yearBuf), BitConverter.ToInt32(monthBuf), BitConverter.ToInt32(dayBuf));
                short height = BitConverter.ToInt16(heightBuf);
                decimal weight = new decimal(BitConverter.ToDouble(weightBuf));
                char drivingLicenseCategory = Encoding.UTF8.GetString(drivingLicenseCategoryBuf)[0];

                record = new FileCabinetRecord
                {
                    Id = recordId,
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = dateOfBirth,
                    Height = height,
                    Weight = weight,
                    DrivingLicenseCategory = drivingLicenseCategory,
                };
                result.Add(record);
                }

                index += FileConsts.RecordSize;
            }

            return result;
        }

        /// <summary>
        /// Gets a number of records.
        /// </summary>
        /// <returns>Number of records.</returns>
        public int GetStat()
        {
            return (int)(this.fileStream.Length / FileConsts.RecordSize) - this.deleted;
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

            int index = 0;
            this.fileStream.Seek(index, SeekOrigin.Begin);
            while (index < this.fileStream.Length)
            {
                byte[] buffer = new byte[FileConsts.RecordSize];
                this.fileStream.Read(buffer, 0, buffer.Length);
                byte[] recordIdBuf = buffer[FileConsts.IdBegin..FileConsts.FirstNameBegin];

                int recordId = BitConverter.ToInt32(recordIdBuf);
                if (recordId == id)
                {
                    byte[] statusBuf = buffer[FileConsts.StatusBegin..FileConsts.IdBegin];
                    short status = BitConverter.ToInt16(statusBuf);
                    status &= 4;
                    if (status == 0)
                    {
                        this.validator.ValidateParameters(arguments);
                        this.fileStream.Seek(index, SeekOrigin.Begin);

                        byte[] firstNameBuf = buffer[FileConsts.FirstNameBegin..FileConsts.LastNameBegin];
                        byte[] lastNameBuf = buffer[FileConsts.LastNameBegin..FileConsts.YearBegin];
                        byte[] yearBuf = buffer[FileConsts.YearBegin..FileConsts.MonthBegin];
                        byte[] monthBuf = buffer[FileConsts.MonthBegin..FileConsts.DayBegin];
                        byte[] dayBuf = buffer[FileConsts.DayBegin..FileConsts.HeightBegin];
                        string firstNameDelete = Encoding.UTF8.GetString(firstNameBuf);
                        string lastNameDelete = Encoding.UTF8.GetString(lastNameBuf);
                        string dateOfBirthDelete = new DateTime(BitConverter.ToInt32(yearBuf), BitConverter.ToInt32(monthBuf), BitConverter.ToInt32(dayBuf)).ToString(CultureInfo.InvariantCulture);
                        this.firstNameDictionary[firstNameDelete].Remove(index);
                        this.lastNameDictionary[lastNameDelete].Remove(index);
                        this.dateOfBirthDictionary[dateOfBirthDelete].Remove(index);
                        short st = 0;
                        byte[] statusBf = BitConverter.GetBytes(st);
                        byte[] firstName = Encoding.UTF8.GetBytes(arguments.FirstName);

                        byte[] firstNameResult = new byte[FileConsts.NameSize];
                        for (int i = 0; i < firstName.Length; i++)
                        {
                            firstNameResult[i] = firstName[i];
                        }

                        byte[] lastName = Encoding.UTF8.GetBytes(arguments.LastName);
                        byte[] lastNameResult = new byte[FileConsts.NameSize];
                        for (int i = 0; i < lastName.Length; i++)
                        {
                            lastNameResult[i] = lastName[i];
                        }

                        byte[] year = BitConverter.GetBytes(arguments.DateOfBirth.Year);
                        byte[] month = BitConverter.GetBytes(arguments.DateOfBirth.Month);
                        byte[] day = BitConverter.GetBytes(arguments.DateOfBirth.Day);

                        byte[] height = BitConverter.GetBytes(arguments.Height);
                        byte[] weight = BitConverter.GetBytes(decimal.ToDouble(arguments.Weight));
                        byte[] drivingLicenseCategory = BitConverter.GetBytes(arguments.DrivingLicenseCategory);
                        if (!this.firstNameDictionary.ContainsKey(arguments.FirstName))
                        {
                            this.firstNameDictionary.Add(arguments.FirstName, new List<long>());
                        }

                        this.firstNameDictionary[arguments.FirstName].Add(index);

                        if (!this.lastNameDictionary.ContainsKey(arguments.LastName))
                        {
                            this.lastNameDictionary.Add(arguments.LastName, new List<long>());
                        }

                        this.lastNameDictionary[arguments.LastName].Add(index);

                        if (!this.dateOfBirthDictionary.ContainsKey(arguments.DateOfBirth.ToString(CultureInfo.CurrentCulture)))
                        {
                            this.dateOfBirthDictionary.Add(arguments.DateOfBirth.ToString(CultureInfo.CurrentCulture), new List<long>());
                        }

                        this.dateOfBirthDictionary[arguments.DateOfBirth.ToString(CultureInfo.CurrentCulture)].Add(index);

                        this.fileStream.Write(statusBf, 0, statusBf.Length);
                        this.fileStream.Write(recordIdBuf, 0, recordIdBuf.Length);
                        this.fileStream.Write(firstNameResult, 0, firstNameResult.Length);
                        this.fileStream.Write(lastNameResult, 0, lastNameResult.Length);
                        this.fileStream.Write(year, 0, year.Length);
                        this.fileStream.Write(month, 0, month.Length);
                        this.fileStream.Write(day, 0, day.Length);
                        this.fileStream.Write(height, 0, height.Length);
                        this.fileStream.Write(weight, 0, weight.Length);
                        this.fileStream.Write(drivingLicenseCategory, 0, drivingLicenseCategory.Length);
                        this.fileStream.Flush();
                        break;
                    }
                }

                index += FileConsts.RecordSize;
            }
        }

        /// <summary>
        /// Finds all records with given firstname.
        /// </summary>
        /// <param name="firstName">The first name of the person.</param>
        /// <returns>A list of records found.</returns>
        /// <exception cref="ArgumentNullException">String firstName is null.</exception>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (firstName == null)
            {
                throw new ArgumentNullException(nameof(firstName));
            }

            byte[] firstNameToChangeBuf = new byte[FileConsts.NameSize];
            byte[] firstNameBufSmall = Encoding.UTF8.GetBytes(firstName);

            for (int i = 0; i < firstNameBufSmall.Length; i++)
            {
                firstNameToChangeBuf[i] = firstNameBufSmall[i];
            }

            firstName = Encoding.UTF8.GetString(firstNameToChangeBuf);
            return new FilesystemIterator(this.firstNameDictionary[firstName], this.fileStream);
        }

        /// <summary>
        /// Finds all records with given lastname.
        /// </summary>
        /// <param name="lastName">The last name of the person.</param>
        /// <returns>A list of records found.</returns>
        /// <exception cref="ArgumentNullException">String firstName is null.</exception>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (lastName == null)
            {
                throw new ArgumentNullException(nameof(lastName));
            }

            byte[] lastNameToChangeBuf = new byte[FileConsts.NameSize];
            byte[] lastNameBufSmall = Encoding.UTF8.GetBytes(lastName);

            for (int i = 0; i < lastNameBufSmall.Length; i++)
            {
                lastNameToChangeBuf[i] = lastNameBufSmall[i];
            }

            lastName = Encoding.UTF8.GetString(lastNameToChangeBuf);

            return new FilesystemIterator(this.lastNameDictionary[lastName], this.fileStream);
        }

        /// <summary>
        /// Finds all records with given date of Birth.
        /// </summary>
        /// <param name="dateTime">The date of birth of the person.</param>
        /// <returns>A list of records found.</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime dateTime)
        {
          return new FilesystemIterator(this.lastNameDictionary[dateTime.ToString(CultureInfo.CurrentCulture)], this.fileStream);
        }

        /// <summary>
        /// Generates snapshot of the service.
        /// </summary>
        /// <returns>A snapshot of this service.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            if (this.fileStream.Length % FileConsts.RecordSize != 0)
            {
                return new FileCabinetServiceSnapshot(result);
            }

            int index = 0;
            this.fileStream.Seek(index, SeekOrigin.Begin);
            while (index < this.fileStream.Length)
            {
                FileCabinetRecord record;
                byte[] buffer = new byte[FileConsts.RecordSize];
                this.fileStream.Read(buffer, 0, buffer.Length);
                byte[] statusBuf = buffer[FileConsts.StatusBegin..FileConsts.IdBegin];
                short status = BitConverter.ToInt16(statusBuf);
                status &= 4;
                if (status == 0)
                {
                    byte[] recordIdBuf = buffer[FileConsts.IdBegin..FileConsts.FirstNameBegin];
                    byte[] firstNameBuf = buffer[FileConsts.FirstNameBegin..FileConsts.LastNameBegin];
                    byte[] lastNameBuf = buffer[FileConsts.LastNameBegin..FileConsts.YearBegin];
                    byte[] yearBuf = buffer[FileConsts.YearBegin..FileConsts.MonthBegin];
                    byte[] monthBuf = buffer[FileConsts.MonthBegin..FileConsts.DayBegin];
                    byte[] dayBuf = buffer[FileConsts.DayBegin..FileConsts.HeightBegin];
                    byte[] heightBuf = buffer[FileConsts.HeightBegin..FileConsts.WeightBegin];
                    byte[] weightBuf = buffer[FileConsts.WeightBegin..FileConsts.DrivingLicenseCategoryBegin];
                    byte[] drivingLicenseCategoryBuf = buffer[FileConsts.DrivingLicenseCategoryBegin..FileConsts.RecordSize];

                    int recordId = BitConverter.ToInt32(recordIdBuf);
                    string firstName = Encoding.UTF8.GetString(firstNameBuf);
                    string lastName = Encoding.UTF8.GetString(lastNameBuf);
                    for (int i = 0; i < firstName.Length; i++)
                    {
                        if (firstName[i] == '\0')
                        {
                            firstName = firstName[0..i];
                        }
                    }

                    for (int i = 0; i < lastName.Length; i++)
                    {
                        if (lastName[i] == '\0')
                        {
                            lastName = lastName[0..i];
                        }
                    }

                    DateTime dateOfBirth = new DateTime(BitConverter.ToInt32(yearBuf), BitConverter.ToInt32(monthBuf), BitConverter.ToInt32(dayBuf));
                    short height = BitConverter.ToInt16(heightBuf);
                    decimal weight = new decimal(BitConverter.ToDouble(weightBuf));
                    char drivingLicenseCategory = Encoding.UTF8.GetString(drivingLicenseCategoryBuf)[0];

                    record = new FileCabinetRecord
                    {
                        Id = recordId,
                        FirstName = firstName,
                        LastName = lastName,
                        DateOfBirth = dateOfBirth,
                        Height = height,
                        Weight = weight,
                        DrivingLicenseCategory = drivingLicenseCategory,
                    };
                    result.Add(record);
                }

                index += 270;
            }

            return new FileCabinetServiceSnapshot(result);
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

            foreach (FileCabinetRecord record in snapshot.Records)
            {
                if (record.Id < this.id)
                {
                    this.EditRecord(record.Id, new Arguments(record.FirstName, record.LastName, record.DateOfBirth, record.Height, record.Weight, record.DrivingLicenseCategory));
                }
                else
                {
                    this.id = record.Id;
                    this.CreateRecord(new Arguments(record.FirstName, record.LastName, record.DateOfBirth, record.Height, record.Weight, record.DrivingLicenseCategory));
                }
            }
        }

        /// <summary>
        /// Removes a record.
        /// </summary>
        /// <param name="id">Id of a record to remove.</param>
        /// <returns>A bool result of removing.</returns>
        public bool Remove(int id)
        {
            bool hasFound = false;
            int index = 0;
            this.fileStream.Seek(index, SeekOrigin.Begin);
            while (index < this.fileStream.Length)
            {
                byte[] buffer = new byte[FileConsts.RecordSize];
                this.fileStream.Read(buffer, 0, buffer.Length);
                byte[] recordIdBuf = buffer[FileConsts.IdBegin..FileConsts.FirstNameBegin];

                int recordId = BitConverter.ToInt32(recordIdBuf);
                if (recordId == id)
                {
                    this.deleted++;
                    hasFound = true;
                    this.fileStream.Seek(index, SeekOrigin.Begin);
                    byte[] statusBuf = buffer[FileConsts.StatusBegin..FileConsts.IdBegin];
                    short status = BitConverter.ToInt16(statusBuf);
                    status |= 4;
                    statusBuf = BitConverter.GetBytes(status);
                    this.fileStream.Write(statusBuf, 0, statusBuf.Length);
                    this.fileStream.Flush();
                    break;
                }

                index += FileConsts.RecordSize;
            }

            return hasFound;
        }

        /// <summary>
        /// Gets the Id of the last record.
        /// </summary>
        /// <returns>Int id.</returns>
        public int GetID()
        {
            return this.id;
        }

        /// <summary>
        /// Purges deleted records.
        /// </summary>
        /// <returns>Num of purged records.</returns>
        public int Purge()
        {
            int result = (int)(this.fileStream.Length / FileConsts.RecordSize);
            List<FileCabinetRecord> lst = (List<FileCabinetRecord>)this.GetRecords();
            this.fileStream.Seek(0, SeekOrigin.Begin);
            string name = this.fileStream.Name;
            this.fileStream.Close();
            File.Delete(name);
            this.fileStream = new FileStream(name, FileMode.CreateNew);
            foreach (FileCabinetRecord record in lst)
            {
                short st = 0;
                byte[] status = BitConverter.GetBytes(st);
                byte[] recordId = BitConverter.GetBytes(record.Id);
                byte[] firstName = Encoding.UTF8.GetBytes(record.FirstName);

                byte[] firstNameResult = new byte[FileConsts.NameSize];
                for (int i = 0; i < firstName.Length; i++)
                {
                    firstNameResult[i] = firstName[i];
                }

                byte[] lastName = Encoding.UTF8.GetBytes(record.LastName);
                byte[] lastNameResult = new byte[FileConsts.NameSize];
                for (int i = 0; i < lastName.Length; i++)
                {
                    lastNameResult[i] = lastName[i];
                }

                byte[] year = BitConverter.GetBytes(record.DateOfBirth.Year);
                byte[] month = BitConverter.GetBytes(record.DateOfBirth.Month);
                byte[] day = BitConverter.GetBytes(record.DateOfBirth.Day);

                byte[] height = BitConverter.GetBytes(record.Height);
                byte[] weight = BitConverter.GetBytes(decimal.ToDouble(record.Weight));
                byte[] drivingLicenseCategory = BitConverter.GetBytes(record.DrivingLicenseCategory);
                this.id++;

                this.fileStream.Write(status, 0, status.Length);
                this.fileStream.Write(recordId, 0, recordId.Length);
                this.fileStream.Write(firstNameResult, 0, firstNameResult.Length);
                this.fileStream.Write(lastNameResult, 0, lastNameResult.Length);
                this.fileStream.Write(year, 0, year.Length);
                this.fileStream.Write(month, 0, month.Length);
                this.fileStream.Write(day, 0, day.Length);
                this.fileStream.Write(height, 0, height.Length);
                this.fileStream.Write(weight, 0, weight.Length);
                this.fileStream.Write(drivingLicenseCategory, 0, drivingLicenseCategory.Length);
                this.fileStream.Flush();
                result--;
            }

            this.deleted = 0;
            return result;
        }

        /// <summary>
        /// Inserts a new Record.
        /// </summary>
        /// <param name="id">Id of a record.</param>
        /// <param name="arguments">Properties of the record.</param>
        /// <returns>New record's Id.</returns>
        public int InsertRecord(int id, Arguments arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            this.validator.ValidateParameters(arguments);
            short st = 0;
            byte[] status = BitConverter.GetBytes(st);
            byte[] recordId = BitConverter.GetBytes(id);
            byte[] firstName = Encoding.UTF8.GetBytes(arguments.FirstName);

            byte[] firstNameResult = new byte[FileConsts.NameSize];
            for (int i = 0; i < firstName.Length; i++)
            {
                firstNameResult[i] = firstName[i];
            }

            byte[] lastName = Encoding.UTF8.GetBytes(arguments.LastName);
            byte[] lastNameResult = new byte[FileConsts.NameSize];
            for (int i = 0; i < lastName.Length; i++)
            {
                lastNameResult[i] = lastName[i];
            }

            byte[] year = BitConverter.GetBytes(arguments.DateOfBirth.Year);
            byte[] month = BitConverter.GetBytes(arguments.DateOfBirth.Month);
            byte[] day = BitConverter.GetBytes(arguments.DateOfBirth.Day);

            byte[] height = BitConverter.GetBytes(arguments.Height);
            byte[] weight = BitConverter.GetBytes(decimal.ToDouble(arguments.Weight));
            byte[] drivingLicenseCategory = BitConverter.GetBytes(arguments.DrivingLicenseCategory);
            this.id++;

            if (!this.firstNameDictionary.ContainsKey(arguments.FirstName))
            {
                this.firstNameDictionary.Add(arguments.FirstName, new List<long>());
            }

            this.firstNameDictionary[arguments.FirstName].Add(this.fileStream.Length);

            if (!this.lastNameDictionary.ContainsKey(arguments.LastName))
            {
                this.lastNameDictionary.Add(arguments.LastName, new List<long>());
            }

            this.lastNameDictionary[arguments.LastName].Add(this.fileStream.Length);

            if (!this.dateOfBirthDictionary.ContainsKey(arguments.DateOfBirth.ToString(CultureInfo.CurrentCulture)))
            {
                this.dateOfBirthDictionary.Add(arguments.DateOfBirth.ToString(CultureInfo.CurrentCulture), new List<long>());
            }

            this.dateOfBirthDictionary[arguments.DateOfBirth.ToString(CultureInfo.CurrentCulture)].Add(this.fileStream.Length);

            this.fileStream.Write(status, 0, status.Length);
            this.fileStream.Write(recordId, 0, recordId.Length);
            this.fileStream.Write(firstNameResult, 0, firstNameResult.Length);
            this.fileStream.Write(lastNameResult, 0, lastNameResult.Length);
            this.fileStream.Write(year, 0, year.Length);
            this.fileStream.Write(month, 0, month.Length);
            this.fileStream.Write(day, 0, day.Length);
            this.fileStream.Write(height, 0, height.Length);
            this.fileStream.Write(weight, 0, weight.Length);
            this.fileStream.Write(drivingLicenseCategory, 0, drivingLicenseCategory.Length);
            this.fileStream.Flush();
            return id;
        }

        /// <summary>
        /// Disposes service.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Gets the number of deleted records.
        /// </summary>
        /// <returns>Int id.</returns>
        public int GetDeletedStat()
        {
            return this.deleted;
        }

        /// <summary>
        /// Disposes service.
        /// </summary>
        /// <param name="disposing">Indicator.</param>
        protected virtual void Dispose(bool disposing)
        {
            this.fileStream.Dispose();
        }

        private void RemakeDictionaries()
        {
            int index = 0;
            this.fileStream.Seek(index, SeekOrigin.Begin);
            while (index < this.fileStream.Length)
            {
                byte[] buffer = new byte[FileConsts.RecordSize];
                this.fileStream.Read(buffer, 0, buffer.Length);
                byte[] statusBuf = buffer[FileConsts.StatusBegin..FileConsts.IdBegin];
                short status = BitConverter.ToInt16(statusBuf);
                status &= 4;
                if (status == 0)
                {
                    byte[] firstNameBuf = buffer[FileConsts.FirstNameBegin..FileConsts.LastNameBegin];
                    byte[] lastNameBuf = buffer[FileConsts.LastNameBegin..FileConsts.YearBegin];
                    byte[] yearBuf = buffer[FileConsts.YearBegin..FileConsts.MonthBegin];
                    byte[] monthBuf = buffer[FileConsts.MonthBegin..FileConsts.DayBegin];
                    byte[] dayBuf = buffer[FileConsts.DayBegin..FileConsts.HeightBegin];
                    string firstName = Encoding.UTF8.GetString(firstNameBuf);
                    string lastName = Encoding.UTF8.GetString(lastNameBuf);
                    DateTime dateOfBirth = new DateTime(BitConverter.ToInt32(yearBuf), BitConverter.ToInt32(monthBuf), BitConverter.ToInt32(dayBuf));
                    if (!this.firstNameDictionary.ContainsKey(firstName))
                    {
                        this.firstNameDictionary.Add(firstName, new List<long>());
                    }

                    this.firstNameDictionary[firstName].Add(index);

                    if (!this.lastNameDictionary.ContainsKey(lastName))
                    {
                        this.lastNameDictionary.Add(lastName, new List<long>());
                    }

                    this.lastNameDictionary[lastName].Add(index);

                    if (!this.dateOfBirthDictionary.ContainsKey(dateOfBirth.ToString(CultureInfo.CurrentCulture)))
                    {
                        this.dateOfBirthDictionary.Add(dateOfBirth.ToString(CultureInfo.CurrentCulture), new List<long>());
                    }

                    this.dateOfBirthDictionary[dateOfBirth.ToString(CultureInfo.CurrentCulture)].Add(index);
                }

                index += FileConsts.RecordSize;
            }
        }
    }
}
