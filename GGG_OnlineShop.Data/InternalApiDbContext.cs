namespace GGG_OnlineShop.InternalApiDB.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using Models.Base;

    public class InternalApiDbContext : IdentityDbContext<User>, IInternalApiDbContext
    {
        public InternalApiDbContext()
            : base("GGG_OnlineShopInternalDb_new", throwIfV1Schema: false)
        {
        }

        public static InternalApiDbContext Create()
        {
            return new InternalApiDbContext();
        }

        public IDbSet<OrderedItem> OrderedItems { get; set; }

        public IDbSet<VehicleGlass> VehicleGlasses { get; set; }

        public IDbSet<Vehicle> Vehicles { get; set; }

        public IDbSet<VehicleMake> VehicleMakes { get; set; }

        public IDbSet<VehicleModel> VehicleModels { get; set; }

        public IDbSet<VehicleBodyType> VehicleBodyTypes { get; set; }

        public IDbSet<VehicleGlassImage> VehicleGlassImages { get; set; }

        public IDbSet<VehicleGlassCharacteristic> VehicleGlassCharacteristics { get; set; }

        public IDbSet<VehicleGlassInterchangeablePart> VehicleGlassInterchangeableParts { get; set; }

        public IDbSet<VehicleGlassAccessory> VehicleGlassAccessories { get; set; }

        public IDbSet<VehicleGlassSuperceed> VehicleGlassSuperceeds { get; set; }

        public override int SaveChanges()
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("AspNetUsers");
            // info:
            // removing glass -> removes the connection with characteristics for the glassId (applicable for many to many), but not the characteristic itself
            //    - check what happens when this happens (glass should show [] characteristics)
            // removing model/make/bodytype removes the vehicle

            base.OnModelCreating(modelBuilder);
        }

        private void ApplyAuditInfoRules()
        {
            // Approach via @julielerman: http://bit.ly/123661P
            foreach (var entry in
                this.ChangeTracker.Entries()
                    .Where(
                        e =>
                        e.Entity is IAuditInfo && ((e.State == EntityState.Added) || (e.State == EntityState.Modified))))
            {
                var entity = (IAuditInfo)entry.Entity;
                if (entry.State == EntityState.Added && entity.CreatedOn == default(DateTime))
                {
                    entity.CreatedOn = DateTime.Now;
                }
            }
        }
    }
}
