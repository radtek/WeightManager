using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.IO;
using FastReport.Data.ConnectionEditors;

namespace FastReport.Data
{
    /// <summary>
    /// Represents a connection to csv file-based database.
    /// </summary>
    /// <example>This example shows how to add a new connection to the report.
    /// <code>
    /// Report report1;
    /// CsvDataConnection conn = new CsvDataConnection();
    /// conn.CsvFile = @"c:\data.csv";
    /// report1.Dictionary.Connections.Add(conn);
    /// conn.CreateAllTables();
    /// </code>
    /// </example>
    public class CsvDataConnection : DataConnectionBase
    {
        #region Constants

        /// <summary>
        /// The default field name.
        /// </summary>
        public const string DEFAULT_FIELD_NAME = "Field";
        private const int NUMBER_OF_STRINGS_FOR_TYPE_CHECKING = 100;

        #endregion Constants

        #region Properties

        /// <summary>
        /// Gets or sets the path to .csv file.
        /// </summary>
        [Category("Data")]
        public string CsvFile
        {
            get
            {
                CsvConnectionStringBuilder builder = new CsvConnectionStringBuilder(ConnectionString);
                return builder.CsvFile;
            }
            set
            {
                CsvConnectionStringBuilder builder = new CsvConnectionStringBuilder(ConnectionString);
                builder.CsvFile = value;
                ConnectionString = builder.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the codepage of the .csv file.
        /// </summary>
        [Category("Data")]
        public int Codepage
        {
            get
            {
                CsvConnectionStringBuilder builder = new CsvConnectionStringBuilder(ConnectionString);
                return Convert.ToInt32(builder.Codepage);
            }
            set
            {
                CsvConnectionStringBuilder builder = new CsvConnectionStringBuilder(ConnectionString);
                builder.Codepage = value.ToString();
                ConnectionString = builder.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the separator of the .csv file.
        /// </summary>
        [Category("Data")]
        public string Separator
        {
            get
            {
                CsvConnectionStringBuilder builder = new CsvConnectionStringBuilder(ConnectionString);
                return builder.Separator;
            }
            set
            {
                CsvConnectionStringBuilder builder = new CsvConnectionStringBuilder(ConnectionString);
                builder.Separator = value;
                ConnectionString = builder.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the value indicating that field names should be loaded from the first string of the file.
        /// </summary>
        [Category("Data")]
        public bool FieldNamesInFirstString
        {
            get
            {
                CsvConnectionStringBuilder builder = new CsvConnectionStringBuilder(ConnectionString);
                if (builder.FieldNamesInFirstString.ToLower() == "false")
                {
                    return false;
                }
                return true;
            }
            set
            {
                CsvConnectionStringBuilder builder = new CsvConnectionStringBuilder(ConnectionString);
                builder.FieldNamesInFirstString = value.ToString().ToLower();
                ConnectionString = builder.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the value indicating that quotation marks should be removed.
        /// </summary>
        [Category("Data")]
        public bool RemoveQuotationMarks
        {
            get
            {
                CsvConnectionStringBuilder builder = new CsvConnectionStringBuilder(ConnectionString);
                if (builder.RemoveQuotationMarks.ToLower() == "true")
                {
                    return true;
                }
                return false;
            }
            set
            {
                CsvConnectionStringBuilder builder = new CsvConnectionStringBuilder(ConnectionString);
                builder.RemoveQuotationMarks = value.ToString().ToLower();
                ConnectionString = builder.ToString();
            }
        }


        /// <summary>
        /// Gets or sets the value indicating that field types fhould be converted.
        /// </summary>
        [Category("Data")]
        public bool ConvertFieldTypes
        {
            get
            {
                CsvConnectionStringBuilder builder = new CsvConnectionStringBuilder(ConnectionString);
                if (builder.ConvertFieldTypes.ToLower() == "true")
                {
                    return true;
                }
                return false;
            }
            set
            {
                CsvConnectionStringBuilder builder = new CsvConnectionStringBuilder(ConnectionString);
                builder.ConvertFieldTypes = value.ToString().ToLower();
                ConnectionString = builder.ToString();
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvDataConnection"/> class.
        /// </summary>
        public CsvDataConnection()
        {
            IsSqlBased = false;
        }

        #endregion Constructors

        #region Private Methods

        private bool CheckType(List<string> values, Type type)
        {
            // column type is int
            if (type == typeof(Int32))
            {
                int intTemp = 0;
                foreach (string value in values)
                {
                    if (!Int32.TryParse(value, out intTemp))
                    {
                        return false;
                    }
                }
                return true;
            }
            // column type is double
            else if (type == typeof(Double))
            {
                double doubleTemp = 0.0;
                foreach (string value in values)
                {
                    if (!Double.TryParse(value, out doubleTemp))
                    {
                        return false;
                    }
                }
                return true;
            }
            // column type is decimal
            else if (type == typeof(Decimal))
            {
                decimal decimalTemp;
                foreach (string value in values)
                {
                    if (!Decimal.TryParse(value, out decimalTemp))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        private void DetermineTypes(List<string> lines, DataTable table)
        {
            // init test columns
            List<List<string>> columns = new List<List<string>>();
            for (int i = 0; i < table.Columns.Count; i++)
            {
                columns.Add(new List<string>());
            }

            // init all rows of each test column
            foreach (string line in lines)
            {
                string[] values = line.Split(Separator.ToCharArray());
                // remove qoutes if needed
                if (RemoveQuotationMarks)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = values[i].Trim("\"".ToCharArray());
                    }
                }

                int valuesCount = values.Length < table.Columns.Count ? values.Length : table.Columns.Count;
                for (int i = 0; i < valuesCount; i++)
                {
                    columns[i].Add(values[i]);
                }
            }

            // determine the types first time
            int intTemp = 0;
            double doubleTemp = 0.0;
            decimal decimalTemp;
            for (int i = 0; i < table.Columns.Count; i++)
            {
                if (Int32.TryParse(columns[i][0], out intTemp))
                {
                    table.Columns[i].DataType = typeof(Int32);
                }
                else if (Double.TryParse(columns[i][0], out doubleTemp))
                {
                    table.Columns[i].DataType = typeof(Double);
                }
                else if (Decimal.TryParse(columns[i][0], out decimalTemp))
                {
                    table.Columns[i].DataType = typeof(Decimal);
                }
            }

            // try to convert all test values of each column to type determined first time
            for (int i = 0; i < table.Columns.Count; i++)
            {
                if (!CheckType(columns[i], table.Columns[i].DataType))
                {
                    table.Columns[i].DataType = typeof(string);
                }
            }
        }

        #endregion Private Methods

        #region Protected Methods

        /// <inheritdoc/>
        protected override DataSet CreateDataSet()
        {
            DataSet dataset = base.CreateDataSet();

            if (!String.IsNullOrEmpty(CsvFile) && !String.IsNullOrEmpty(Separator))
            {
                StreamReader reader = new StreamReader(CsvFile, Encoding.GetEncoding(Codepage));
                List<string> lines = new List<string>();
                string allText = reader.ReadToEnd();
                lines.AddRange(allText.Split(Environment.NewLine.ToCharArray()));

                // get table name from file name
                string tableName = Path.GetFileNameWithoutExtension(CsvFile);
                if (String.IsNullOrEmpty(tableName))
                {
                    tableName = "Table";
                }

                DataTable table = new DataTable(tableName);

                // get values for field names
                string[] values = lines[0].Split(Separator.ToCharArray());
                // remove qoutes if needed
                if (RemoveQuotationMarks)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = values[i].Trim("\"".ToCharArray());
                    }
                }

                // create table columns
                for (int i = 0; i < values.Length; i++)
                {
                    DataColumn column = new DataColumn();

                    // get field names from first string if needed
                    if (FieldNamesInFirstString)
                    {
                        column.ColumnName = values[i];
                        column.Caption = values[i];
                    }
                    else
                    {
                        column.ColumnName = DEFAULT_FIELD_NAME + i.ToString();
                        column.Caption = DEFAULT_FIELD_NAME + i.ToString();
                    }

                    column.DataType = typeof(string);
                    table.Columns.Add(column);
                }

                // cast types of fields if needed
                if (ConvertFieldTypes)
                {
                    int index = FieldNamesInFirstString ? 1 : 0;
                    int number = lines.Count - index;
                    if (lines.Count > NUMBER_OF_STRINGS_FOR_TYPE_CHECKING)
                    {
                        number = NUMBER_OF_STRINGS_FOR_TYPE_CHECKING;
                    }
                    DetermineTypes(lines.GetRange(index, number), table);
                }

                // add table rows
                for (int i = FieldNamesInFirstString ? 1 : 0; i < lines.Count; i++)
                {
                    // get values from the string
                    values = lines[i].Split(Separator.ToCharArray());
                    // remove qoutes if needed
                    if (RemoveQuotationMarks)
                    {
                        for (int j = 0; j < values.Length; j++)
                        {
                            values[j] = values[j].Trim("\"".ToCharArray());
                        }
                    }

                    // add a new row
                    DataRow row = table.NewRow();
                    int valuesCount = values.Length < table.Columns.Count ? values.Length : table.Columns.Count;
                    for (int j = 0; j < valuesCount; j++)
                    {
                        row[j] = values[j];
                    }
                    table.Rows.Add(row);
                }

                dataset.Tables.Add(table);
            }

            return dataset;
        }

        /// <inheritdoc/>
        protected override void SetConnectionString(string value)
        {
            DisposeDataSet();
            base.SetConnectionString(value);
        }

        #endregion Protected Methods

        #region Public Methods

        /// <inheritdoc/>
        public override void FillTableSchema(DataTable table, string selectCommand, CommandParameterCollection parameters)
        {
            // do nothing
        }

        /// <inheritdoc/>
        public override void FillTableData(DataTable table, string selectCommand, CommandParameterCollection parameters)
        {
            // do nothing
        }

        /// <inheritdoc/>
        public override void CreateTable(TableDataSource source)
        {
            if (DataSet.Tables.Contains(source.TableName))
            {
                source.Table = DataSet.Tables[source.TableName];
                base.CreateTable(source);
            }
            else
            {
                source.Table = null;
            }
        }

        /// <inheritdoc/>
        public override void DeleteTable(TableDataSource source)
        {
            // do nothing
        }

        /// <inheritdoc/>
        public override string QuoteIdentifier(string value, DbConnection connection)
        {
            return value;
        }

        /// <inheritdoc/>
        public override ConnectionEditorBase GetEditor()
        {
            return new CsvConnectionEditor();
        }

        /// <inheritdoc/>
        public override string GetConnectionId()
        {
            return "Csv: " + CsvFile;
        }

        /// <inheritdoc/>
        public override string[] GetTableNames()
        {
            string[] result = new string[DataSet.Tables.Count];
            for (int i = 0; i < DataSet.Tables.Count; i++)
            {
                result[i] = DataSet.Tables[i].TableName;
            }
            return result;
        }

        /// <inheritdoc/>
        public override void TestConnection()
        {
            using (DataSet dataset = CreateDataSet())
            {
            }
        }

        #endregion Public Methods
    }
}