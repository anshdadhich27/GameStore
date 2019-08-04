using GameStore.Models.DataProvider;
using System;
using System.Threading.Tasks;
using Dapper;
using System.Data;

namespace GameStore.Models.Entity
{
    public class ExceptionLog
    {
        public string Source { get; set; }
        public string Stacktrace { get; set; }
        public string TargetSite { get; set; }
        public string ErrorMessage { get; set; }
        public string Information { get; set; }
        public string ExceptionFound { get; set; }
        public DateTime ErrorTime { get; set; } = DateTime.Now;

        public ExceptionLog() { }
        public ExceptionLog(Exception ex, string info)
        {
            Information = info;            
            Source = ex.Source;
            ErrorMessage = ex.Message;
            Stacktrace = ex.StackTrace;
            TargetSite = ex.TargetSite?.ToString();            
            ExceptionFound = ex.GetType()?.FullName;
        }
    }

    public class ExceptionGateway
    {
        private const string AddCExceptionSP = "Log_Add_Exception_details";

        public static async Task<bool> AddExceptionAsync(ExceptionLog obj)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return await con.ExecuteScalarAsync<bool>(AddCExceptionSP, new { obj.ErrorTime, obj.ExceptionFound, obj.Source, obj.Stacktrace, obj.TargetSite, obj.ErrorMessage, obj.Information }, commandType: CommandType.StoredProcedure);
            }
        }

        public static bool AddException(ExceptionLog obj)
        {
            using (var con = DbHelper.GetSqlConnection())
            {
                return con.ExecuteScalar<bool>(AddCExceptionSP, new { obj.ErrorTime, obj.ExceptionFound, obj.Source, obj.Stacktrace, obj.TargetSite, obj.ErrorMessage, obj.Information }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
