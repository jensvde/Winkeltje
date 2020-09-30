using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace DAL.EF
{
    public class TwinkeltjeDbContext : DbContext
    {
        private static bool hasRunDuringAppExecution = false;

        public DbSet<Product> Products { get; set; }
        public DbSet<Allergie> Allergies { get; set; }
        public DbSet<OpeningsUur> OpeningsTijden { get; set; }
        public DbSet<Vakantie> Vakanties { get; set; }
        public DbSet<HomeImage> HomeImages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
          //  optionsBuilder.UseSqlite("Data Source=TwinkeltjeDb_EFCodeFirst1.db")
         // optionsBuilder.UseSqlite("Data Source=TwinkeltjeDb_EFCodeFirst2.db");
        optionsBuilder.UseMySql("server=localhost;port=3306;database=db;uid=winkel;password=Winkeltje@1234");
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductAllergie>()
                .HasKey(t => new {t.ProductId, t.AllergieId});

            modelBuilder.Entity<ProductAllergie>()
                .HasOne(pt => pt.Product)
                .WithMany(p => p.ProductAllergies)
                .HasForeignKey(pt => pt.ProductId);

            modelBuilder.Entity<ProductAllergie>()
                .HasOne(pt => pt.Allergie)
                .WithMany(t => t.ProductAllergies)
                .HasForeignKey(pt => pt.AllergieId);
        }

        public static void Initialize(TwinkeltjeDbContext context, bool dropCreateDatabase = false)
        {
            if (!hasRunDuringAppExecution)
            {
                // Delete database if requested
                if (dropCreateDatabase)
                    context.Database.EnsureDeleted();
                // Create database and initial data if needed
                if (context.Database.EnsureCreated())
                SeedProducts(context);
                SeedAllergies(context);
                SeedOpeneningsTijden(context);
                SeedVakanties(context);
                SeedCarouselItems(context);
                hasRunDuringAppExecution = true;
            }
        }
        private static void SeedTemplate(TwinkeltjeDbContext context)
        {
            context.SaveChanges();
            foreach (EntityEntry entry in context.ChangeTracker.Entries().ToList())
            {
                entry.State = EntityState.Detached;
            }
        }
        private static void SeedCarouselItems(TwinkeltjeDbContext context)
        {
            List<HomeImage> carouselItems = new List<HomeImage>();
            for (int i = 5; i >= 1; i--)
            {
                HomeImage image = new HomeImage();
                switch (i)
                {
                    case 1: image.Text = "Vooraanzicht"; break;
                    case 2: image.Text = "Charcuterie assortiment"; break;
                    case 3: image.Text = "Snoep assortiment"; break;
                    case 4: image.Text = "Drank assortiment"; break;
                    case 5: image.Text = "Assortiment algemene voeding"; break;
                }
                image.ImageData = File.ReadAllBytes("Images/IMG_" + i + ".jpg");
                carouselItems.Add(image);
                }
            context.HomeImages.AddRange(carouselItems);
            context.SaveChanges();
            foreach (EntityEntry entry in context.ChangeTracker.Entries().ToList())
            {
                entry.State = EntityState.Detached;
            }
        }
        
        private static void SeedVakanties(TwinkeltjeDbContext context)
        {
            Vakantie vakantie = new Vakantie
            {
                startDatum = new DateTime(2020,09,20),
                eindDatum = new DateTime(2020,10,25),
                reden = "jaarlijks verlof"
            };
            context.Vakanties.Add(vakantie);
            context.SaveChanges();
            foreach (EntityEntry entry in context.ChangeTracker.Entries().ToList())
            {
                entry.State = EntityState.Detached;
            }
        }
        private static void SeedOpeneningsTijden(TwinkeltjeDbContext context)
        {
            List<OpeningsUur> uren = new List<OpeningsUur>();
            for (int i = 6; i >= 0; i--)
            {
                OpeningsUur uur = new OpeningsUur();
                switch (i)
                {
                    case 0: uur.StartVoormiddag = "8.00";
                        uur.EindVoormiddag = "12.30";
                        uur.StartNamiddag = "13.00";
                        uur.EindNamiddag = "18.00";
                        uur.Dag = "Maandag";
                        uur.DagVanDeWeek = "Monday";
                        break;
                    case 1: uur.SluitingsDag = true;
                        uur.Dag = "Dinsdag";
                        uur.DagVanDeWeek = "Tuesday";
                        break;
                    case 2: uur.StartVoormiddag = "8.00";
                        uur.EindVoormiddag = "12.30";
                        uur.StartNamiddag = "13.00";
                        uur.EindNamiddag = "18.00";
                        uur.Dag = "Woensdag";
                        uur.DagVanDeWeek = "Wednesday";
                        break;
                    case 3: uur.StartVoormiddag = "8.00";
                        uur.EindVoormiddag = "13.00";
                        uur.SluitingsHalfDag = true;
                        uur.Dag = "Donderdag";
                        uur.DagVanDeWeek = "Thursday";
                        break;
                    case 4: uur.StartVoormiddag = "8.00";
                        uur.EindVoormiddag = "12.30";
                        uur.StartNamiddag = "13.00";
                        uur.EindNamiddag = "18.00";
                        uur.Dag = "Vrijdag";
                        uur.DagVanDeWeek = "Friday";
                        break;
                    case 5: uur.StartVoormiddag = "8.00";
                        uur.EindNamiddag = "17.00";
                        uur.Dag = "Zaterdag";
                        uur.DagVanDeWeek = "Saturday";
                        break;
                    case 6: uur.StartVoormiddag = "8.00";
                        uur.EindVoormiddag = "13.00";
                        uur.SluitingsHalfDag = true;
                        uur.Dag = "Zondag";
                        uur.DagVanDeWeek = "Sunday";
                        break;
                }
                uren.Add(uur);
            }
            context.OpeningsTijden.AddRange(uren);
            context.SaveChanges();
            foreach (EntityEntry entry in context.ChangeTracker.Entries().ToList())
            {
                entry.State = EntityState.Detached;
            }
        }
        private static void SeedAllergies(TwinkeltjeDbContext context)
        {
            
            foreach (Product contextProduct in context.Products.ToList())
            {
                Allergie Ei = context.Allergies.Single(x => x.Naam.Equals("Ei"));
                Allergie Melk = context.Allergies.Single(x => x.Naam.Equals("Melk"));
                Allergie Lactose = context.Allergies.Single(x => x.Naam.Equals("Lactose"));
                contextProduct.ProductAllergies = new List<ProductAllergie>();
                foreach (Allergie allergie in new[]{Ei,Lactose,Melk})
                {
                    contextProduct.ProductAllergies.Add(new ProductAllergie
                        {
                            Product = contextProduct,
                            Allergie = allergie,
                        });
                }
                context.Products.Update(contextProduct);
            }
            
            context.SaveChanges();
            foreach (EntityEntry entry in context.ChangeTracker.Entries().ToList())
            {
                entry.State = EntityState.Detached;
            }
        }

        private static void SeedProducts(TwinkeltjeDbContext context)
        {
            Allergie Gluten = new Allergie
            {
                Naam = "Gluten", Beschrijving = "Gluten"
            };

            Allergie Ei = new Allergie
            {
                Naam = "Ei", Beschrijving = "Eieren"
            };

            Allergie Lactose = new Allergie
            {
                Naam = "Lactose", Beschrijving = "Lactose"
            };

            Allergie Noten = new Allergie
            {
                Naam = "Noten", Beschrijving = "Noten"
            };

            Allergie Mosterd = new Allergie
            {
                Naam = "Mosterd", Beschrijving = "Mosterd"
            };

            Allergie Melk = new Allergie
            {
                Naam = "Melk", Beschrijving = "Melk"
            };

            Allergie Geen = new Allergie
            {
                Naam = "Geen", Beschrijving = "Geen"
            };
            context.Allergies.AddRange(new []{Ei, Lactose, Melk, Gluten, Mosterd, Noten});
            

            Product product1 = new Product
            {
                Beschrijving =
                    "Een lekker smeuïge paté, wat zorgt voor een zeer aangenaam mondgevoel, met een sublieme smaak. De paté wordt gepresenteerd in een witte terrine van slechts 1.5 kg, wat resulteert in een snelle rotatie. Kortom, met deze paté zal u zeker scoren bij uw klanten!",
                Naam = "Crèmepaté",
                ImageData = File.ReadAllBytes("Images/cremepate.jpg")
            };
            Product product2 = new Product
            {
                Beschrijving =
                    "Dit traditionele vleesbrood bestaat voor 100% uit grof gemalen varkensvlees. Dit product is verkrijgbaar in de artisanale broodvorm of in de rendabele blokvorm.",
                Naam = "Vleesbrood (1/2)",
                ImageData = File.ReadAllBytes("Images/vleesbrood.jpg")
            };
            Product product3 = new Product
            {
                Naam = "Blokpaté",
                Beschrijving =
                    "Deze blokpaté krijgt zijn unieke, intense smaak door het gebruik van verse lever. Door de verzorgde afwerking met natuurlijk lardeerspek en het nabranden verkrijgt deze smakelijke blokpaté zijn mooie presentatie.",
                ImageData = File.ReadAllBytes("Images/blokpate.jpg")
            };
            Product product4 = new Product
            {
                Naam = "Lunchworst Patron",
                Beschrijving =
                    "De Lunchworst Patron wordt vervaardigd uit varkensvlees met grove ham. De milde roking zorgt voor een extra smaakvol aspect. Door zijn typische ringvorm onderscheidt de Lunchworst Patron zich van de andere kookworsten.",
                ImageData = File.ReadAllBytes("Images/lunchworst_patron.jpg")
            };
            Product product5 = new Product
            {
                Naam = "Kalfsworst",
                Beschrijving =
                    "Met zijn frisse, zachte smaak is deze Kalfsworst de ideale klassieke, boterhamworst. Deze hespenworst is ook verkrijgbaar met look.",
                ImageData = File.ReadAllBytes("Images/kalfsworst.jpg")
            };
            Product product6 = new Product
            {
                Naam = "Hespenworst",
                Beschrijving =
                    "Met zijn frisse, zachte smaak is deze Hespenworst de ideale klassieke, boterhamworst. Deze hespenworst is ook verkrijgbaar met look.",
                ImageData = File.ReadAllBytes("Images/hespenworst.jpg")
            };
            Product product7 = new Product
            {
                Naam = "Mandolino",
                Beschrijving =
                    "Deze ambachtelijke ham vervaardigd uit de beste spierdelen, wordt op aloude wijze gerookt op een bed van smeulend beukenhout en jeneverbessen. Het resultaat is een traditionele ham met verfijne rooksmaak in een optimaal rendabele vorm. De Imperial kroon meer dan waardig!",
                ImageData = File.ReadAllBytes("Images/Mandolino.jpg")
            };       
            

            context.Products.AddRange(new []{product1, product2, product3, product4, product5, product6, product7});

           
            context.SaveChanges();
            foreach (EntityEntry entry in context.ChangeTracker.Entries().ToList())
            {
                entry.State = EntityState.Detached;
            }
        }


    }
}