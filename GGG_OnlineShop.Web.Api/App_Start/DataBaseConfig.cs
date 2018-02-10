namespace GGG_OnlineShop.Web.Api.App_Start
{
    using InternalApiDB.Data;
    using System.Data.Entity;
    using InternalApiDB.Data.Migrations;

    public class DataBaseConfig
    {
        public static void Initialize()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<InternalApiDbContext, Configuration>());
            InternalApiDbContext.Create().Database.Initialize(false);
        }
    }
}