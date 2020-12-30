using System.Collections.Generic;
using System.Linq;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF
{
    public class ProductRepository : IProductRepository
    {
        private TwinkeltjeDbContext ctx = null;

        public ProductRepository()
        {
            ctx = new TwinkeltjeDbContext();
            TwinkeltjeDbContext.Initialize(ctx, true);
        }
        
        public Product CreateProduct(Product product)
        {
            ctx.Products.Add(product);
            ctx.SaveChanges();
            return product;
        }

        public Product ReadProduct(int productId)
        {
            return ctx.Products.Include(u => u.ProductAllergies).Single(x => x.ProductId == productId);
        }

      

        public void UpdateProduct(Product product)
        {
            ctx.Products.Update(product);
            ctx.SaveChanges();
        }

        public void DeleteProduct(Product product)
        {
            ctx.Products.Remove(product);
            ctx.SaveChanges();
        }

        public IEnumerable<Product> ReadProducts()
        {
            return ctx.Products.Include(u => u.ProductAllergies).AsEnumerable();
        }
        public IEnumerable<Allergie> ReadAllergies()
        {
            List<Allergie> allergies = new List<Allergie>();
            List<string> uniqueAllergies = new List<string>();

                foreach (Allergie readProductProductAllergy in ctx.Allergies.AsEnumerable())
                {
                    if (! uniqueAllergies.Contains(readProductProductAllergy.Naam))
                    {
                        uniqueAllergies.Add(readProductProductAllergy.Naam);
                        allergies.Add(readProductProductAllergy);
                    }
                }

            return allergies.OrderBy(o => o.Naam);
        }

        public IEnumerable<OpeningsUur> ReadOpeningstijden()
        {
            return ctx.OpeningsTijden.AsEnumerable();
        }

        public IEnumerable<Vakantie> ReadVakanties()
        {
            return ctx.Vakanties.AsEnumerable();
        }

        public OpeningsUur CreateOpeningsuur(OpeningsUur openingsUur)
        {
            ctx.OpeningsTijden.Add(openingsUur);
            ctx.SaveChanges();
            return openingsUur;
        }

        public OpeningsUur ReadOpeningsUur(int openingsuurId)
        {
            return ctx.OpeningsTijden.Find(openingsuurId);
        }

        public void UpdateOpeningsuur(OpeningsUur openingsUur)
        {
            ctx.OpeningsTijden.Update(openingsUur);
            ctx.SaveChanges();
        }

        public void DeleteOpeningsuur(OpeningsUur openingsUur)
        {
            ctx.OpeningsTijden.Remove(openingsUur);
            ctx.SaveChanges();
        }

        public Vakantie CreateVakantie(Vakantie vakantie)
        {
            ctx.Vakanties.Add(vakantie);
            ctx.SaveChanges();
            return vakantie;
        }

        public Vakantie ReadVakantie(int vakantieId)
        {
            return ctx.Vakanties.Find(vakantieId);
        }

        public void UpdateVakantie(Vakantie vakantie)
        {
            ctx.Vakanties.Update(vakantie);
            ctx.SaveChanges();
        }

        public void DeleteVakantie(Vakantie vakantie)
        {
            ctx.Vakanties.Remove(vakantie);
            ctx.SaveChanges();
        }

        public Allergie CreateAllergie(Allergie allergie)
        {
            ctx.Allergies.Add(allergie);
            ctx.SaveChanges();
            return allergie;
        }

        public Allergie ReadAllergie(int allergieId)
        {
            return ctx.Allergies.Find(allergieId);
        }

        public void DeleteAllergie(Allergie allergie)
        {
            ctx.Allergies.Remove(allergie);
            ctx.SaveChanges();
        }

        public void UpdateAllergie(Allergie allergie)
        {
            ctx.Allergies.Update(allergie);
            ctx.SaveChanges();
        }

        public HomeImage CreateHomeImage(HomeImage homeImage)
        {
            ctx.HomeImages.Add(homeImage);
            ctx.SaveChanges();
            return homeImage;
        }

        public HomeImage ReadHomeImage(int homeImageId)
        {
            return ctx.HomeImages.Find(homeImageId);
        }

        public void UpdateHomeImage(HomeImage homeImage)
        {
            ctx.HomeImages.Update(homeImage);
            ctx.SaveChanges();
        }

        public void DeteteHomeImage(HomeImage homeImage)
        {
            ctx.HomeImages.Remove(homeImage);
            ctx.SaveChanges();
        }

        public IEnumerable<HomeImage> ReadHomeImages()
        {
            return ctx.HomeImages.AsEnumerable();
        }
    }
}