using System.Data;
using Dapper;
using FmsEliteBilling.Model;
using FmsEliteBilling.Model.Response;
using Microsoft.Data.Sqlite;


namespace FmsEliteBilling.Data.Repository
{
    public class LogRespository
    {
        private readonly string _connectionString;

        public object Log { get; private set; }

        // private readonly string _conStr;

        public LogRespository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection CreateConnection()
        {
            return new SqliteConnection(_connectionString);
        }

        public GetResponse<LogListVM> GetLogList(string fromDt, string toDt)
        {
            GetResponse<LogListVM> result = new GetResponse<LogListVM>();
            result.response = new ResponseModel();
            try
            {
                string query = "";
                string whereCondition = "";
                whereCondition = (!string.IsNullOrEmpty(fromDt) && !string.IsNullOrEmpty(toDt)) ? "where Date(l.Timestamp) between @fromDt and @toDt Order by l.Timestamp" : (!string.IsNullOrEmpty(fromDt) && string.IsNullOrEmpty(toDt)) ? "where Date(l.Timestamp) >= @fromDt Order by l.Timestamp" : "";
                using (var connection = CreateConnection())
                {
                    query = "Select * from logs l " + whereCondition + " ";
                    var json = connection.Query<LogListVM>(query, new { fromDt = fromDt, toDt = toDt }).AsList();
                    result.value = json;
                    result.response.ReturnNumber = 0;
                    result.response.ErrorMessage = "Log retrieved sucessfully";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public GetResponse<LogListVM> GetLogsFromPreviousMonthAndYear(string Year, string Month)
        {
            GetResponse<LogListVM> result = new GetResponse<LogListVM>();
            result.response = new ResponseModel();
            try
            {
                string query = "";
                string currentDirectory = Directory.GetCurrentDirectory();
                string logFileName = "logs_" + Year + Month;
                string logsDirectory = Path.Combine(Directory.GetParent(Directory.GetParent(currentDirectory).ToString()).ToString(), "EATracker-Logs\\");
                using (var connection = new SqliteConnection("Data Source=" + Path.Combine(logsDirectory + logFileName + ".db")))
                {
                    query = "Select * from logs l ";
                    var json = connection.Query<LogListVM>(query).AsList();
                    result.value = json;
                    result.response.ReturnNumber = 0;
                    result.response.ErrorMessage = "Log retrieved sucessfully";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public ResponseModel DeleteLog(DateTime from, DateTime to)
        {
            ResponseModel result = new ResponseModel();
            try
            {
                if (to.Date >= from.Date)
                {
                    string fromDt = from.ToString("yyyy-MM-dd");
                    string toDt = to.ToString("yyyy-MM-dd");
                    string query = "";
                    string whereCondition = "";
                    whereCondition = (!string.IsNullOrEmpty(fromDt) && !string.IsNullOrEmpty(toDt)) ? " where Date(Timestamp) between @fromDt and @toDt" : "";
                    using (var connection = CreateConnection())
                    {
                        query = "Delete from logs " + whereCondition + " ";
                        connection.Query<bool>(query, new { fromDt = fromDt, toDt = toDt });
                        result.ReturnNumber = 0;
                        result.ErrorMessage = "Successfully logs deleted from " + fromDt + " to " + toDt + " ";
                    }
                }
                else
                {
                    result.ReturnNumber = -100;
                    result.ErrorMessage = "To Date is lesser than from Date";
                }
            }
            catch (Exception)
            {
                result.ReturnNumber = -100;
                result.ErrorMessage = "Unable to Delete Logs";
            }
            return result;
        }

        public ResponseModel DeleteLogFile(string Year, string Month)
        {
            ResponseModel result = new ResponseModel();
            // string logsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs\\");
            string currentDirectory = Directory.GetCurrentDirectory();
            string logsDirectory = Path.Combine(Directory.GetParent(Directory.GetParent(currentDirectory).ToString()).ToString(), "EATracker-Logs\\");
            string logFileName = "logs_" + Year + Month;
            try
            {
                if (File.Exists(logsDirectory + logFileName + ".db"))
                {
                    File.Delete(logsDirectory + logFileName + ".db");
                    result.ReturnNumber = 0;
                    result.ErrorMessage = "File Deleted Sucessfully";
                }
                else
                {
                    result.ReturnNumber = -100;
                    result.ErrorMessage = "File does not Exist";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }
    }
}