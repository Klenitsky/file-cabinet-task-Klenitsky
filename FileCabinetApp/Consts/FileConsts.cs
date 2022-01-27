using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Consts
{
    /// <summary>
    /// Consts of byte files.
    /// </summary>
    public static class FileConsts
    {
        /// <summary>
        /// Size of a full record.
        /// </summary>
        public const int RecordSize = 279;

        /// <summary>
        /// Size of a name.
        /// </summary>
        public const int NameSize = 120;

        /// <summary>
        /// Beginning of status data.
        /// </summary>
        public const int StatusBegin = 0;

        /// <summary>
        /// Beginning of id data.
        /// </summary>
        public const int IdBegin = 2;

        /// <summary>
        /// Beginning of first name data.
        /// </summary>
        public const int FirstNameBegin = 6;

        /// <summary>
        /// Beginning of last name data.
        /// </summary>
        public const int LastNameBegin = 126;

        /// <summary>
        /// Beginning of Year data.
        /// </summary>
        public const int YearBegin = 246;

        /// <summary>
        /// Beginning of month data.
        /// </summary>
        public const int MonthBegin = 250;

        /// <summary>
        /// Beginning of day data.
        /// </summary>
        public const int DayBegin = 254;

        /// <summary>
        /// Beginning of height data.
        /// </summary>
        public const int HeightBegin = 258;

        /// <summary>
        /// Beginning of weight data.
        /// </summary>
        public const int WeightBegin = 260;

        /// <summary>
        /// Beginning of driving license category data.
        /// </summary>
        public const int DrivingLicenseCategoryBegin = 268;
    }
}
