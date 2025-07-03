using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using ExcelDataReader;

namespace Framework.Infrastructure.Helpers
{
    public static class ExcelHelper
    {
        /// <summary>
        /// reads excel binary and returns dataset with excel rows.
        /// </summary>
        /// <param name="memoryStream">Memory Stream of the excel file.</param>
        /// <param name="fileExtension">File Extension(Either .xls, .xlsx, .csv).</param>
        /// <returns>Data set of the records in excel sheet.</returns>
        public static DataSet ReadExcelBinary(MemoryStream memoryStream, string fileExtension)
        {
            using (var reader = (fileExtension.ToLower() == ".xlsx" || fileExtension.ToLower() == ".xls")
                ? ExcelReaderFactory.CreateReader(memoryStream) : (fileExtension.ToLower() == ".csv") ? ExcelReaderFactory.CreateCsvReader(memoryStream) : null)
            {
                if (reader == null)
                {
                    return null;
                }

                do
                {
                    while (reader.Read())
#pragma warning disable S108 // Nested blocks of code should not be left empty
                    {
                    }
#pragma warning restore S108 // Nested blocks of code should not be left empty
                }
                while (reader.NextResult());
                var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });
                return result;
            }
        }

        /// <summary>
        /// Converts a DataTable to a list with generic objects.
        /// </summary>
        /// <typeparam name="T">Generic object.</typeparam>
        /// <param name="table">DataTable.</param>
        /// <returns>List with generic objects.</returns>
        public static List<T> DataTableToList<T>(this DataTable table, Type headerType)
            where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (var row in table.AsEnumerable())
                {
                    T obj = new T();

                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            if (propertyInfo != null)
                                propertyInfo.SetValue(obj, Convert.ChangeType(row[headerType.GetProperty(prop.Name).GetValue(null).ToString()], propertyInfo.PropertyType), null);
                        }
#pragma warning disable S2486 // Generic exceptions should not be ignored
                        catch
#pragma warning disable S108 // Nested blocks of code should not be left empty
                        {
                        }
#pragma warning restore S108 // Nested blocks of code should not be left empty
#pragma warning restore S2486 // Generic exceptions should not be ignored
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
#pragma warning disable S1168 // Empty arrays and collections should be returned instead of null
                return null;
#pragma warning restore S1168 // Empty arrays and collections should be returned instead of null
            }
        }
    }
}
