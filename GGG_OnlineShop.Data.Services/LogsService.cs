namespace GGG_OnlineShop.Data.Services
{
    using Base;
    using Common;
    using Contracts;
    using InternalApiDB.Models;
    using InternalApiDB.Models.Enums;
    using System;
    using System.Data.Entity.Validation;
    using System.Runtime.CompilerServices;
    using System.Text;

    public class LogsService : BaseDataService<Log>, ILogsService
    {
        public LogsService(IInternalDbRepository<Log> dataSet) : base(dataSet)
        {
        }

        public void LogError(string error, string placeToWriteErrors = "")
        {
        }

        public void LogError(Exception exc, string comment, string className, [CallerMemberName]string method = "")
        {
            StringBuilder sb = new StringBuilder();
            string allExceptionMessage = GetAllExceptionMessages(exc, sb);
           

            Log newLog = new Log
            {
                Type = LogType.Error,
                Place = $"{className}.{method}",
                Info = allExceptionMessage,
                Comment = comment,
            };

            Add(newLog);
        }

        private string GetAllExceptionMessages(Exception exc, StringBuilder sb)
        {
            // bottom
            if (exc == null)
            {
                return sb.ToString();
            }

            sb.Append($"{exc.Message} --> ");

            return GetAllExceptionMessages(exc.InnerException, sb);
        }

        public void LogInfo(string info, string placeToWriteInfo = "")
        {
        }
    }
}
