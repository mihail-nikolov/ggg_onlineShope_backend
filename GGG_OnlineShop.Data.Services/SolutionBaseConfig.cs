namespace GGG_OnlineShop.Web.Api.App_Start
{
    using Data.Services.Contracts;
    using System.IO;
    using System.Web;

    public class SolutionBaseConfig: ISolutionBaseConfig
    {
        public string GetSolutionPath()
        {
            return Path.GetDirectoryName(Path.GetDirectoryName(HttpRuntime.AppDomainAppPath));
        }
    }
}