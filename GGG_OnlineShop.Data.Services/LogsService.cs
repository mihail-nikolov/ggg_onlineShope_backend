namespace GGG_OnlineShop.Data.Services
{
    using Base;
    using Common;
    using Contracts;
    using InternalApiDB.Models;
    using InternalApiDB.Models.Enums;
    using System;
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
            // Log time error - use enumerator with json properties EnumeratorConvert
            //string errorMessage = exc.Message;
            //string innerErrorMessage = string.Empty;
            //string innerInnerErrorMessage = string.Empty;
            //string errorMessage = string.Empty;

            //while (true)
            //{

            //}
            //if (exc.InnerException != null)
            //{
            //    innerErrorMessage = exc.InnerException.Message;

            //    if (exc.InnerException.InnerException != null)
            //    {
            //        innerInnerErrorMessage = exc.InnerException.InnerException.Message;
            //    }
            //}

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
