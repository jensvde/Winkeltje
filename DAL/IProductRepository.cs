using System.Collections;
using System.Collections.Generic;
using Domain;

namespace DAL
{
    public interface IProductRepository
    {
        Product CreateProduct(Product product);
        Product ReadProduct(int productId);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
        IEnumerable<Product> ReadProducts();
        
        Allergie CreateAllergie(Allergie allergie);
        Allergie ReadAllergie(int allergieId);
        void DeleteAllergie(Allergie allergie);
        void UpdateAllergie(Allergie allergie);
        IEnumerable<Allergie> ReadAllergies();
        IEnumerable<OpeningsUur> ReadOpeningstijden();
        IEnumerable<Vakantie> ReadVakanties();
        OpeningsUur CreateOpeningsuur(OpeningsUur openingsUur);
        OpeningsUur ReadOpeningsUur(int openingsuurId);
        void UpdateOpeningsuur(OpeningsUur openingsUur);
        void DeleteOpeningsuur(OpeningsUur openingsUur);

        Vakantie CreateVakantie(Vakantie vakantie);
        Vakantie ReadVakantie(int vakantieId);
        void UpdateVakantie(Vakantie vakantie);
        void DeleteVakantie(Vakantie vakantie);

        HomeImage CreateHomeImage(HomeImage homeImage);
        HomeImage ReadHomeImage(int homeImageId);
        void UpdateHomeImage(HomeImage homeImage);
        void DeteteHomeImage(HomeImage homeImage);
        IEnumerable<HomeImage> ReadHomeImages();
    }
}