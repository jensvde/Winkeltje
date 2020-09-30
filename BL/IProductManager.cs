using System.Collections.Generic;
using Domain;

namespace BL
{
    public interface IProductManager
    {
        IEnumerable<Product> GetProducts();
        IEnumerable<Allergie> GetAllergies();
        IEnumerable<OpeningsUur> GetOpeningstijden();
        IEnumerable<Vakantie> GetVakanties();
        IEnumerable<HomeImage> GetHomeImages();
        Product AddProduct(Product product);
        Allergie AddAllergie(Allergie allergie);
        Product GetProduct(int productId);
        Allergie GetAllergie(int allergieId);
        void ChangeProduct(Product product);
        void ChangeAllergie(Allergie allergie);
        void DeleteProduct(Product product);
        void DeleteAllergie(Allergie allergie);
        
        OpeningsUur AddOpeningsUur(OpeningsUur openingsUur);
        OpeningsUur GetOpeningsUur(int openingsUurId);
        void ChangeOpeningsUur(OpeningsUur openingsUur);
        void DeleteOpeningsUur(OpeningsUur openingsUur);
        
        Vakantie AddVakantie(Vakantie vakantie);
        Vakantie GetVakantie(int vakantieId);
        void ChangeVakantie(Vakantie vakantie);
        void DeleteVakantie(Vakantie vakantie);

        HomeImage AddHomeImage(HomeImage homeImage);
        HomeImage GetHomeImage(int homeImageId);
        void ChangeHomeImage(HomeImage homeImage);
        void DeleteHomeImage(HomeImage homeImage);

    }
}