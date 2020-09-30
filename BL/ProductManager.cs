using System.Collections.Generic;
using DAL;
using DAL.EF;
using Domain;

namespace BL
{
    public class ProductManager : IProductManager
    {
        private readonly IProductRepository repo;

        public ProductManager()
        {
            repo = new ProductRepository();
        }
        public IEnumerable<Product> GetProducts()
        {
            return repo.ReadProducts();
        }

        public IEnumerable<Allergie> GetAllergies()
        {
            return repo.ReadAllergies();
        }

        public IEnumerable<OpeningsUur> GetOpeningstijden()
        {
            return repo.ReadOpeningstijden();
        }

        public IEnumerable<Vakantie> GetVakanties()
        {
            return repo.ReadVakanties();
        }

        public Product AddProduct(Product product)
        {
            return repo.CreateProduct(product);
        }

        public Allergie AddAllergie(Allergie allergie)
        {
            return repo.CreateAllergie(allergie);
        }

        public Product GetProduct(int productId)
        {
            return repo.ReadProduct(productId);
        }

        public Allergie GetAllergie(int allergieId)
        {
            return repo.ReadAllergie(allergieId);
        }

        public void ChangeProduct(Product product)
        {
            repo.UpdateProduct(product);
        }

        public void ChangeAllergie(Allergie allergie)
        {
            repo.UpdateAllergie(allergie);
        }

        public void DeleteProduct(Product product)
        {
            repo.DeleteProduct(product);
        }

        public void DeleteAllergie(Allergie allergie)
        {
            repo.DeleteAllergie(allergie);
        }

        public OpeningsUur AddOpeningsUur(OpeningsUur openingsUur)
        {
            return repo.CreateOpeningsuur(openingsUur);
        }

        public OpeningsUur GetOpeningsUur(int openingsUurId)
        {
            return repo.ReadOpeningsUur(openingsUurId);
        }

        public void ChangeOpeningsUur(OpeningsUur openingsUur)
        {
            repo.UpdateOpeningsuur(openingsUur);
        }

        public void DeleteOpeningsUur(OpeningsUur openingsUur)
        {
            repo.DeleteOpeningsuur(openingsUur);
        }

        public Vakantie AddVakantie(Vakantie vakantie)
        {
            return repo.CreateVakantie(vakantie);
        }

        public Vakantie GetVakantie(int vakantieId)
        {
            return repo.ReadVakantie(vakantieId);
        }

        public void ChangeVakantie(Vakantie vakantie)
        {
            repo.UpdateVakantie(vakantie);
        }

        public void DeleteVakantie(Vakantie vakantie)
        {
            repo.DeleteVakantie(vakantie);
        }

        public IEnumerable<HomeImage> GetHomeImages()
        {
            return repo.ReadHomeImages();
        }

        public HomeImage AddHomeImage(HomeImage homeImage)
        {
            return repo.CreateHomeImage(homeImage);
        }

        public HomeImage GetHomeImage(int homeImageId)
        {
            return repo.ReadHomeImage(homeImageId);
        }

        public void ChangeHomeImage(HomeImage homeImage)
        {
            repo.UpdateHomeImage(homeImage);
        }

        public void DeleteHomeImage(HomeImage homeImage)
        {
            repo.DeteteHomeImage(homeImage);
        }
    }
}