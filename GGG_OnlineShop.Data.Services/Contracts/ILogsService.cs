namespace GGG_OnlineShop.Data.Services.Contracts
{
    using GGG_OnlineShop.Common.Services.Contracts;
    using InternalApiDB.Models;
    using System;
    using System.Runtime.CompilerServices;

    public interface ILogsService : ILogger, IBaseDataService<Log>
    {
        void LogError(Exception exc, string comment, string className, [CallerMemberName]string method = "");

        void LogInfo(string info, string comment, string className, [CallerMemberName] string method = "");
    }
}
