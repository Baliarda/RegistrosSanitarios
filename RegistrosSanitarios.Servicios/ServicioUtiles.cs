using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using RegistrosSanitarios.EntityModel.Models;
using System.Runtime.CompilerServices;
using Serilog;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RegistrosSanitarios.EntityModel;
using RegistrosSanitarios.EntityModel.Enums;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Threading;

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;



namespace RegistrosSanitarios.Servicios
{
    public static class LoggerExtensions
    {
        public static Serilog.ILogger Here(this Serilog.ILogger logger,
            [CallerMemberName] string memberName = "")
        {
            return logger
                .ForContext("MemberName", memberName);

        }
    }

    /// <summary>
    /// Helper class to run async methods within a sync process.
    /// </summary>
    public static class AsyncUtil
    {
        private static readonly TaskFactory _taskFactory = new
            TaskFactory(CancellationToken.None,
                        TaskCreationOptions.None,
                        TaskContinuationOptions.None,
                        TaskScheduler.Default);

        /// <summary>
        /// Executes an async Task method which has a void return value synchronously
        /// USAGE: AsyncUtil.RunSync(() => AsyncMethod());
        /// </summary>
        /// <param name="task">Task method to execute</param>
        public static void RunSync(Func<Task> task)
            => _taskFactory
                .StartNew(task)
                .Unwrap()
                .GetAwaiter()
                .GetResult();

        /// <summary>
        /// Executes an async Task<T> method which has a T return type synchronously
        /// USAGE: T result = AsyncUtil.RunSync(() => AsyncMethod<T>());
        /// </summary>
        /// <typeparam name="TResult">Return Type</typeparam>
        /// <param name="task">Task<T> method to execute</param>
        /// <returns></returns>
        public static TResult RunSync<TResult>(Func<Task<TResult>> task)
            => _taskFactory
                .StartNew(task)
                .Unwrap()
                .GetAwaiter()
                .GetResult();
    }

    public class ServicioUtiles
    {
        private readonly ILogger _logger;
        
        public int AnioActual { get { return DateTime.Now.Year; } }
        public ServicioUtiles()
        {
            _logger = Log.ForContext<ServicioUtiles>();
            
            
        }
       
        public string ObtenerMes(int mes)
        {
            var ret = new List<string>();
            ret.Add("Enero");
            ret.Add("Febrero");
            ret.Add("Marzo");
            ret.Add("Abril");
            ret.Add("Mayo");
            ret.Add("Junio");
            ret.Add("Julio");
            ret.Add("Agosto");
            ret.Add("Septiembre");
            ret.Add("Octubre");
            ret.Add("Noviembre");
            ret.Add("Diciembre");
            return ret[mes - 1];
        }

        public List<object> ObtenerMeses(bool permitirNulo)
        {
            var ret = new List<object>();
            if (permitirNulo)
                ret.Add(new { Id = "", Value = "--Seleccionar--" });
            ret.Add(new { Id = 1, Value = "Enero" });
            ret.Add(new { Id = 2, Value = "Febrero" });
            ret.Add(new { Id = 3, Value = "Marzo" });
            ret.Add(new { Id = 4, Value = "Abril" });
            ret.Add(new { Id = 5, Value = "Mayo" });
            ret.Add(new { Id = 6, Value = "Junio" });
            ret.Add(new { Id = 7, Value = "Julio" });
            ret.Add(new { Id = 8, Value = "Agosto" });
            ret.Add(new { Id = 9, Value = "Septiembre" });
            ret.Add(new { Id = 10, Value = "Octubre" });
            ret.Add(new { Id = 11, Value = "Noviembre" });
            ret.Add(new { Id = 12, Value = "Diciembre" });
            return ret;
        }

        public DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();

            // column names 
            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            // Use reflection to get property names, to create table, Only first time, others 
            //will follow 
            {
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue(rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        public bool ExportExcel(DataTable dt, string titulo, string fileName, string path)
        {
            IWorkbook workbook;

            try
            {
                workbook = new HSSFWorkbook();

                ISheet sheet1 = workbook.CreateSheet(titulo);
                NPOI.SS.UserModel.ICellStyle cellStyle = workbook.CreateCellStyle();


                IDataFormat formatDateTime = workbook.CreateDataFormat();
                IDataFormat formatInt = workbook.CreateDataFormat();
                IDataFormat formatDec = workbook.CreateDataFormat();

                //make a header row
                IRow row1 = sheet1.CreateRow(0);

                for (int j = 0; j < dt.Columns.Count; j++)
                {

                    ICell cell = row1.CreateCell(j);
                    String columnName = dt.Columns[j].ToString();
                    cell.SetCellValue(columnName);
                }

                //loops through data
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IRow row = sheet1.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        ICell cell = row.CreateCell(j);
                        String columnName = dt.Columns[j].ToString();

                        if (dt.Columns[j].DataType == System.Type.GetType("System.DateTime"))
                        {
                            cell.CellStyle.DataFormat = formatDateTime.GetFormat("dd/mm/yyyy");
                            if (!(dt.Rows[i][columnName] is DBNull))
                                cell.SetCellValue(String.Format("{0:dd/MM/yyyy}", dt.Rows[i][columnName]));
                        }
                        else if (dt.Columns[j].DataType == System.Type.GetType("System.Int64") || dt.Columns[j].DataType == System.Type.GetType("System.Int32"))
                        {
                            cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0");
                            cell.CellStyle = cellStyle;

                            if (!(dt.Rows[i][columnName] is DBNull))
                                cell.SetCellValue(Convert.ToInt64(dt.Rows[i][columnName].ToString()));
                        }
                        else if (dt.Columns[j].DataType == System.Type.GetType("System.Decimal") || dt.Columns[j].DataType == System.Type.GetType("System.Double"))
                        {
                            cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0.0000");
                            cell.CellStyle = cellStyle;

                            if (!(dt.Rows[i][columnName] is DBNull))
                                cell.SetCellValue(Convert.ToDouble(dt.Rows[i][columnName].ToString()));
                        }
                        else
                        {
                            if (dt.Rows[i][columnName] != null)
                                cell.SetCellValue(dt.Rows[i][columnName].ToString());
                        }


                    }
                }


                string fullPath = Path.Combine(path, fileName);
                FileStream file = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                workbook.Write(file);

                file.Close();

                //borrar temporales anteriores
                string[] filesBorrar = Directory.GetFiles(path);

                foreach (string fileb in filesBorrar)
                {
                    FileInfo fi = new FileInfo(fileb);
                    if (fi.LastAccessTime < DateTime.Now.AddDays(-1))
                        fi.Delete();
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.Here().Error(ex, ex.Message);
                throw ex;
            }
        }

    }

    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) :
                                  JsonConvert.DeserializeObject<T>(value);
        }
    }
}
