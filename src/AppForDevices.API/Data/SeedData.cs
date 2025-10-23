using Humanizer.Localisation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AppForDevices.Shared;
namespace AppForDevices.API.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            List<string> rolesNames = new List<string> { "Administrator", "Employee", "Customer" };
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            SeedRoles(roleManager, rolesNames);

            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            SeedUsers(userManager, rolesNames);

            var user = dbContext.Users.OfType<ApplicationUser>().FirstOrDefault(u => u.UserName == "carmen@uclm.es");


            //it initializes the database with genres and movies
            SeedModelsAndDevices(dbContext);

            //it initializes the database with a purchase
            SeedPurchase(dbContext, user);

            //it initializes the database with a rental
            SeedRental(dbContext, user);
            //it initializes the database with scales and repairs
            SeedScalesAndRepairs(dbContext);

            //it initializes the database with a receipt
            SeedReceipt(dbContext, user);

        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager, List<string> roles)
        {

            foreach (string roleName in roles)
            {
                //it checks such role does not exist in the database 
                if (!roleManager.RoleExistsAsync(roleName).Result)
                {
                    IdentityRole role = new IdentityRole();
                    role.Name = roleName;
                    role.NormalizedName = roleName;
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                }
            }

        }

        public static void SeedUsers(UserManager<IdentityUser> userManager, List<string> roles)
        {
            //first, it checks the user does not already exist in the DB
            if (userManager.FindByNameAsync("carmen@uclm.es").Result == null)
            {
                ApplicationUser user = new ApplicationUser("1", "carmen@uclm.es", "Carmen", "Noblejas Carreto");
                user.EmailConfirmed = true;

                var result = userManager.CreateAsync(user, "Password1234%");
                result.Wait();

                if (result.IsCompletedSuccessfully)
                {
                    //administrator role
                    userManager.AddToRoleAsync(user, roles[0]).Wait();
                }
            }

            if (userManager.FindByNameAsync("lucian@uclm.es").Result == null)
            {
                ApplicationUser user = new ApplicationUser("2", "lucian@uclm.es","Lucian", "Negoita");
                user.EmailConfirmed = true;

                var result = userManager.CreateAsync(user, "APassword1234%");
                result.Wait();

                if (result.IsCompletedSuccessfully)
                {
                    //employee role
                    userManager.AddToRoleAsync(user, roles[1]).Wait();
                }
            }

            if (userManager.FindByNameAsync("javier@uclm.es").Result == null)
            {
                //A customer class has been defined because it has different attributes (purchase, rental, etc.)
                ApplicationUser user = new ApplicationUser("3", "javier@uclm.es", "Javier", "Garcia Tercero");
                user.EmailConfirmed = true;

                var result = userManager.CreateAsync(user, "OtherPass12$");

                result.Wait();

                if (result.IsCompletedSuccessfully)
                {
                    //customer role
                    userManager.AddToRoleAsync(user, roles[2]).Wait();

                }
            }
            if (userManager.FindByNameAsync("agus@uclm.es").Result == null)
            {
                ApplicationUser user = new ApplicationUser("4", "agus@uclm.es", "Agustín", "Prieto Páez");
                user.EmailConfirmed = true;

                var result = userManager.CreateAsync(user, "Password1234%");
                result.Wait();

                if (result.IsCompletedSuccessfully)
                {
                    //administrator role
                    userManager.AddToRoleAsync(user, roles[0]).Wait();
                }
            }

        }

        public static void SeedModelsAndDevices(ApplicationDbContext dbcontext)
        {
            string[] modelnames = ["Computer", "Phone"];
            List<Model> models = [];
            Device device;
            foreach (string modelname in modelnames)
            {
                var model = dbcontext.Models.FirstOrDefault(g => g.NameModel == modelname);
                if (model == null)
                    models.Add(new Model(modelname));
                else
                    models.Add(model);
            }
            dbcontext.SaveChanges();
            if (dbcontext.Devices.FirstOrDefault(m => m.Name == "iphone16") == null)
            {
                device = new Device("iphone16", "max", "apple", "white", 700.0, 7, 2024, models[1], 200.0, 3, QualityType.New);
                dbcontext.Devices.Add(device);
            }

            if (dbcontext.Devices.FirstOrDefault(m => m.Name == "razerblade") == null)
            {
                device = new Device("razerblade", "small", "razer", "black", 2000, 2, 2024, models[0], 500, 2, Shared.QualityType.New);
                dbcontext.Devices.Add(device);
            }

            //it saves the modification of dbcontext to the database
            dbcontext.SaveChanges();

            //alternatively you may have used a raw SQL
            //dbcontext.Database.ExecuteSqlRaw("INSERT INTO [Movies] ([Id], [Title], [GenreId], [ReleaseDate], [PriceForPurchase], [QuantityForPurchase], [PriceForRenting], [QuantityForRenting]) VALUES (1, N'The lord of the rings', 1, N'2011-10-20 00:00:00', 10, 1000, 1, 100)");
            //dbcontext.Database.ExecuteSqlRaw("INSERT INTO [Movies] ([Id], [Title], [GenreId], [ReleaseDate], [PriceForPurchase], [QuantityForPurchase], [PriceForRenting], [QuantityForRenting]) VALUES (2, N'The flying castle', 2, N'2007-04-04 00:00:00', 20, 1000, 3, 10)");


            //Since EFCORE7, you can perform bulk updates with linq.
            dbcontext.Devices.ExecuteUpdate(s => s.SetProperty(m => m.quantityForPurchase, 1000));

            //other example using existing information: add 100 to the QuantityForPurchase of each Movie
            //dbcontext.Movies.ExecuteUpdate(s => s.SetProperty(m => m.QuantityForPurchase, m=>m.QuantityForPurchase+100));

            //You can alternatively use raw SQL to perform the operation where performance is sensitive:
            //dbcontext.Database.ExecuteSqlRaw("UPDATE [Movies] SET [QuantityForPurchase] = 100");

            dbcontext.SaveChanges();

            //Lucian part for seed data 

            List<Model> newModels = new List<Model>
            {
                new Model("Tablet")
            };

            foreach (var model in newModels)
            {
                if (dbcontext.Models.FirstOrDefault(m => m.NameModel == model.NameModel) == null)
                {
                    dbcontext.Models.Add(model);
                    models.Add(model); // Add to our local list so we can use them to add devices later
                }
            }

            dbcontext.SaveChanges(); // Save models to database

            List<Device> testDevices = new List<Device>()
            {
                new Device("Galaxy S21", "High-end smartphone", "Samsung", "Black", 2022, models.FirstOrDefault(m => m.NameModel == "Phone")),
                new Device("iPad Pro", "Latest Apple tablet", "Apple", "Silver", 2023, models.FirstOrDefault(m => m.NameModel == "Tablet"))
            };

            foreach (var deviceL in testDevices) // changed device to deviceL to not have conflict iterating
            {
                if (dbcontext.Devices.FirstOrDefault(d => d.Name == deviceL.Name) == null)
                {
                    dbcontext.Devices.Add(deviceL);
                }
            }

            dbcontext.SaveChanges();

        }

        public static void SeedPurchase(ApplicationDbContext dbcontext, ApplicationUser user)
        {

            if (dbcontext.Purchases.FirstOrDefault(p => p.Id == 1) == null)
            {
                var device = dbcontext.Devices.First();
                var purchase = new Purchase("Avda. España s/n, Albacete", "carmen@uclm.es",
                     "Carmen Noblejas", DateTime.Today, new List<PurchaseItem>(), AppForDevices.Shared.PaymentMethodTypes.CreditCard, user);
                purchase.PurchaseItems.Add(new PurchaseItem(device, 1, purchase));
                dbcontext.Purchases.Add(purchase);
            }
            dbcontext.SaveChanges();

        }

        public static void SeedRental(ApplicationDbContext dbcontext, ApplicationUser user)
        {

            if (dbcontext.Rentals.FirstOrDefault(p => p.Id == 1) == null)
            {
                var device = dbcontext.Devices.First();
                var rental = new Rental("carmen@uclm.es", "Carmen Noblejas",
                                   "Avda. España s/n, Albacete 02071",
                                   DateTime.Now, DateTime.Today.AddDays(2), DateTime.Today.AddDays(5),
                                   new List<RentDevice>(), AppForDevices.Shared.PaymentMethodTypes.CreditCard, user);
                rental.RentDevices.Add(new RentDevice(rental, device));
                dbcontext.Rentals.Add(rental);
            }
            dbcontext.SaveChanges();

        }

        // Reviews seed
        public static void SeedReviews(ApplicationDbContext dbContext, ApplicationUser user)
        {
            // Add some reviews as per your test cases
            var devices = dbContext.Devices.ToList();

            List<Review> reviews = new List<Review>()
            {
                new Review("Great Devices", DateTime.UtcNow, "USA", "customer123", new List<ReviewItem>
                {
                    new ReviewItem(1, null, devices[0].Id, devices[0], "Fantastic device!", 5),
                    new ReviewItem(2, null, devices[1].Id, devices[1], "Good tablet!", 4)
                }),
                new Review("Not that great", DateTime.UtcNow, "Germany", "customer456", new List<ReviewItem>
                {
                    new ReviewItem(3, null, devices[0].Id, devices[0], "Not up to the mark.", 3)
                })
            };

            foreach (var review in reviews)
            {
                if (dbContext.Reviews.FirstOrDefault(r => r.ReviewTitle == review.ReviewTitle) == null)
                {
                    dbContext.Reviews.Add(review);
                }
            }

            dbContext.SaveChanges();
        }
        public static void SeedReceipt(ApplicationDbContext dbcontext, ApplicationUser user)
        {

            if (dbcontext.Receipts.FirstOrDefault(p => p.Id == 1) == null)
            {
                var repair = dbcontext.Repairs.First();
                var receipt = new Receipt( "Agustín Prieto", "Avda. España s/n, Albacete", Shared.PaymentMethodTypes.PayPal,
                    new List<ReceiptItem>() { }, user, DateTime.Today);
                receipt.ReceiptItems.Add(new ReceiptItem(repair, receipt, "Arrivederci"));
                receipt.TotalPrice = receipt.ReceiptItems.Sum(ri => ri.Repair.Cost);
                dbcontext.Receipts.Add(receipt);
            }
            dbcontext.SaveChanges();

        }
        public static void SeedScalesAndRepairs(ApplicationDbContext dbcontext)
        {
            string[] scalenames = ["basic", "medium", "luxury"];
            List<Scale> scales = [];
            Repair repair;
            foreach (string scalename in scalenames)
            {
                var scale = dbcontext.Scales.FirstOrDefault(g => g.Name == scalename);
                if (scale == null)
                    scales.Add(new Scale(scalename));
                else
                    scales.Add(scale);
            }
            if (dbcontext.Repairs.FirstOrDefault(m => m.Name == "Repair Playstation 4") == null)
            {
                repair = new Repair("Repair Playstation 4", "We are going to clean the Playstation", 23.56f, scales[0]);
                dbcontext.Repairs.Add(repair);

            }

            if (dbcontext.Repairs.FirstOrDefault(m => m.Name == "Repair Xbox 364") == null)
            {
                repair = new Repair("Repair Xbox 364", "We are going to repair an Xbox 364 by changing the CPU and GPU", 45.45f, scales[1]);
                dbcontext.Repairs.Add(repair);
            }
            if (dbcontext.Repairs.FirstOrDefault(m => m.Name == "Repair camera Canon") == null)
            {
                repair = new Repair("Repair camera Canon", "We are going to repair the lens of the camera ", 67.45f, scales[2]);
                dbcontext.Repairs.Add(repair);
            }

            //it saves the modification of dbcontext to the database
            dbcontext.SaveChanges();

            //alternatively you may have used a raw SQL
            //dbcontext.Database.ExecuteSqlRaw("INSERT INTO [Movies] ([Id], [Title], [GenreId], [ReleaseDate], [PriceForPurchase], [QuantityForPurchase], [PriceForRenting], [QuantityForRenting]) VALUES (1, N'The lord of the rings', 1, N'2011-10-20 00:00:00', 10, 1000, 1, 100)");
            //dbcontext.Database.ExecuteSqlRaw("INSERT INTO [Movies] ([Id], [Title], [GenreId], [ReleaseDate], [PriceForPurchase], [QuantityForPurchase], [PriceForRenting], [QuantityForRenting]) VALUES (2, N'The flying castle', 2, N'2007-04-04 00:00:00', 20, 1000, 3, 10)");


            //Since EFCORE7, you can perform bulk updates with linq.
            //dbcontext.Repairs.ExecuteUpdate(s => s.SetProperty(m => m.QuantityForPurchase, 10)); // no tengo quantity

            //other example using existing information: add 100 to the QuantityForPurchase of each Movie
            //dbcontext.Movies.ExecuteUpdate(s => s.SetProperty(m => m.QuantityForPurchase, m=>m.QuantityForPurchase+100));

            //You can alternatively use raw SQL to perform the operation where performance is sensitive:
            //dbcontext.Database.ExecuteSqlRaw("UPDATE [Movies] SET [QuantityForPurchase] = 100");

            //dbcontext.SaveChanges();


        }



    }
}