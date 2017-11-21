namespace GGG_OnlineShop.InternalApiDB.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using Models.Base;

    // TODO
    //    - if no - implement the same IQueryable< VehicleGlass> GetByRandomCode(string code)
    //    - discuss GetByRandomCode functionality
    // - async post to database with glasses?
    // hash pass when send from frontend
    // think about indeces, when have the full info - probably not needed
    // - think about key types
    // - introduce constants
    // - think about when model and bodytype are "" -> directly get car ID by make only
    //        same case when hanve only one model or bodytype
    // check for repeatables in DB - OK(double check in the end)
    public class InternalApiDbContext: IdentityDbContext<User>, IInternalApiDbContext
    {
        public InternalApiDbContext()
            : base("GGG_OnlineShopInternalDb", throwIfV1Schema: false)
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
