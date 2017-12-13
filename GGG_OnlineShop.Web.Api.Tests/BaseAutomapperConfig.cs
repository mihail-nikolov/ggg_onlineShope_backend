namespace GGG_OnlineShop.Web.Api.Tests
{
    using Infrastructure;
    using System;
    using System.Linq;

    public class BaseAutomapperConfig
    {
        public void Execute()
        {
            var autoMapperConfig = new AutoMapperConfig();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var neededAssemblies = assemblies.Where(a => a.FullName.Contains("GGG_OnlineShop")).ToList();
            autoMapperConfig.Execute(neededAssemblies);
        }
    }
}
