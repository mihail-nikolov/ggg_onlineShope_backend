namespace GGG_OnlineShop.InternalApiDB.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using Models.Base;

    // TODO
    // - introduce constants

    public class InternalApiDbContext: IdentityDbContext<User>, IInternalApiDbContext
    {
        public InternalApiDbContext()
            : base("GGG_OnlineShopInternalDb")
                  // TODO restore ,  throwIfV1Schema: false)
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
            // TODO removing the user should not remove the orderedItem
            // removing model/make/bodytype should not remove the vehicle
            // removing glass/characteristics/images/vehicle should not remove the mentioned
            // reset password functionality
            //modelBuilder.Entity<User>().HasOptional(u => u.OrderedItems).WithMany(i => i).WillCascadeOnDelete(false);
            //modelBuilder.Entity<OrderedItem>().HasOptional(v => v.UserId);
            modelBuilder.Entity<Vehicle>().HasRequired(v => v.Make).WithMany(v => v.Vehicles).WillCascadeOnDelete(false);
            modelBuilder.Entity<Vehicle>().HasOptional(v => v.BodyType).WithMany(v => v.Vehicles).WillCascadeOnDelete(false);
            modelBuilder.Entity<Vehicle>().HasOptional(v => v.Model).WithMany(v => v.Vehicles).WillCascadeOnDelete(false);
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
