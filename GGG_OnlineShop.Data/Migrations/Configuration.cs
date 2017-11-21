namespace GGG_OnlineShop.InternalApiDB.Data.Migrations
{
    using Common;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<InternalApiDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        private void SeedUsersAndAdmin(InternalApiDbContext context)
        {
            const string AdministratorUserName = "admin@admin.com";
            const string AdministratorPassword = "admin123";

            if (!context.Roles.Any())
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var role = new IdentityRole { Name = GlobalConstants.AdministratorRoleName };
                roleManager.Create(role);

                var userStore = new UserStore<User>(context);
                var userManager = new UserManager<User>(userStore);
                var admin = new User { UserName = AdministratorUserName, Email = AdministratorUserName, Bulstat = "123456", CompanyName = "GGG", DeliveryAddress = "Sofia Liuln" };
                userManager.Create(admin, AdministratorPassword);
                userManager.AddToRole(admin.Id, GlobalConstants.AdministratorRoleName);
            }
        }

        protected override void Seed(InternalApiDbContext context)
        {
            this.SeedUsersAndAdmin(context);
            //context.OrderedItems.AddOrUpdate(new OrderedItem() { EuroCode = "2345A", Description = "Ford Fiesta 2004 3 Doors", CreatedOn = DateTime.Now });
            //context.VehicleBodyTypes.AddOrUpdate(new VehicleBodyType() { Name = "3 HBK" });
            //context.VehicleBodyTypes.AddOrUpdate(new VehicleBodyType() { Name = "3 HBK" });
            //context.VehicleMakes.AddOrUpdate(new VehicleMake() { Name = "Ford" });
            //context.VehicleModelEurocodes.AddOrUpdate(new VehicleGlass() { Name = "2030"});
            //context.VehicleModels.AddOrUpdate(new VehicleModel() { Name = "Fiesta"});
            //context.VehicleYears.AddOrUpdate(new VehicleYear() { Year = "02-05" });
            //context.Vehicles.AddOrUpdate(new Vehicle() { Type = VehicleType.Car, BrandId = 1, ModelId = 1, BodyTypeId = 1, YearId = 1, EurocodeId = 1 });

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
