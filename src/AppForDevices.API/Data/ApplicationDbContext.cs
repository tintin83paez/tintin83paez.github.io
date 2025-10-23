namespace AppForDevices.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<PurchaseItem>().HasKey(pi => new { pi.DeviceId, pi.PurchaseId });
            builder.Entity<RentDevice>().HasKey(pi =>  new {pi.DeviceId, pi.RentalId} );
            builder.Entity<ReviewItem>().HasKey(pi => new { pi.ReviewId, pi.DeviceId });
        }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Model> Models {  get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ReviewItem> ReviewItems { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<Repair> Repairs { get; set; }
        public DbSet<ReceiptItem> ReceiptItems { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet <Rental> Rentals { get; set; }
        public DbSet<Scale> Scales { get; set; }
    }
        
}