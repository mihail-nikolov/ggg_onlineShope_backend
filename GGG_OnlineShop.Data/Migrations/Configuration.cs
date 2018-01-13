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
            // TODO  - adapt when deploy
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

                var admin = new User
                {
                    UserName = AdministratorUserName,
                    Email = AdministratorUserName,
                    EmailConfirmed = true,
                    Bulstat = "123456",
                    Name = "GGG",
                    DeliveryAddress = "Sofia Liuln",
                    DeliveryCountry = "Bulgaria",
                    DeliveryTown = "Sofia",
                    PhoneNumber = "1312312312"
                };
                userManager.Create(admin, AdministratorPassword);
                userManager.AddToRole(admin.Id, GlobalConstants.AdministratorRoleName);
            }
        }

        protected override void Seed(InternalApiDbContext context)
        {
            this.SeedUsersAndAdmin(context);
        }
    }
}
