using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using winkeltje.Models;
using Winkeltje.Helpers;
using Winkeltje.Models;

namespace Winkeltje.Controllers
{
    [Authorize]
    public class DatabaseController : Controller
    {
        private ProductManager _manager = new ProductManager();
        private string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            text = text.Normalize(NormalizationForm.FormD);
            var chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            return new string(chars).Normalize(NormalizationForm.FormC);
        }
        // GET
        public IActionResult Index()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            List<SelectListItem> itemsProduct = new List<SelectListItem>();

            ManagementModel model = new ManagementModel();
            foreach (Allergie allergie in _manager.GetAllergies())
            {
                items.Add(new SelectListItem
                {
                    Value = allergie.AllergieId + "",
                    Text = allergie.Naam
                });
            }

            foreach (Product product in _manager.GetProducts())
            {
                itemsProduct.Add(new SelectListItem
                {
                    Value = product.ProductId + "",
                    Text = product.Naam
                });
            }

            model.Items = items;
            model.ItemsProduct = itemsProduct;
            return View(model);
        }

        [HttpGet("/Database/Upload/")]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpGet("/Database/Download/")]
        public IActionResult Download()
        {
            return View();
        }

        [HttpGet("/Database/DbExport/")]
        public async Task<ActionResult> DbExport(string Database)
        {
            await $"/home/{Environment.UserName}/mysql.sh --export {Database} /home/{Environment.UserName}/{Database}.db".Bash();
            string filePath = Environment.UserName + "/" + Database + ".db";
            string fileName = Database + ".db";
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/force-download", fileName);
        }

        [HttpGet("/Database/Test/")]
        public async Task<IActionResult> TestAsync()
        {
                await $"/home/{Environment.UserName}/mysql.sh --import db /home/{Environment.UserName}/db.db".Bash();
                return Ok();
        }
        public async Task<IActionResult> UploadDbUsers(IFormFile formFile)
        {
            long size = formFile.Length;
            string filePath = "";

            if (formFile.Length > 0)
            {
                // full path to file in temp location
                filePath = Environment.UserName + "/db_users.db"; //we are using Temp file name just for the example. Add your own file path.
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }
            }
            await $"/home/{Environment.UserName}/mysql.sh --import db /home/{Environment.UserName}/db.db".Bash();
            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { size, filePath });
        }
        public async Task<IActionResult> UploadDb(IFormFile formFile)
        {
            long size = formFile.Length;
            string filePath = "";

            if (formFile.Length > 0)
            {
                // full path to file in temp location
                filePath = Environment.UserName + "/db.db"; //we are using Temp file name just for the example. Add your own file path.
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }
            }

            await $"/home/{Environment.UserName}/mysql.sh --import db_users /home/{Environment.UserName}/db_users.db".Bash();
            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            return Ok(new { size, filePath});
        }

        [HttpGet("/Database/DeleteProduct/{id:int?}")]
        public IActionResult DeleteProduct(int id)
        {
            Product product = _manager.GetProduct(id);
            _manager.DeleteProduct(product);
            return RedirectToAction("IndexAdmin", "Database");
        }
        public IActionResult NewProduct()
        {
            WinkelViewModel model = new WinkelViewModel();
            model.Allergies = _manager.GetAllergies().ToList();
            model.Product = new Product();

            model.SelectedAllergies = new List<WinkelItem>();
            foreach (Allergie allergie in model.Allergies)
            {
                model.SelectedAllergies.Add(new WinkelItem()
                {
                    Id = allergie.AllergieId,
                    Name = allergie.Naam
                });
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> NewProduct(WinkelViewModel model)
        {
            if (model.ImageFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await model.ImageFile.CopyToAsync(memoryStream);

                    // Upload the file if less than 8 MB
                    if (memoryStream.Length < 8388608)
                    {
                        model.Product.ImageData = memoryStream.ToArray();
                    }
                    else
                    {
                        ModelState.AddModelError("File", "The file is too large.");
                    }
                }
            }

            List<ProductAllergie> productAllergies = new List<ProductAllergie>();
            if (ModelState.IsValid)
            {
                foreach (WinkelItem item in model.SelectedAllergies)
                {
                    if (item.Selected)
                    {
                        productAllergies.Add(new ProductAllergie
                        {
                            Product = model.Product,
                            Allergie = _manager.GetAllergie(item.Id)
                        });
                    }


                }



                model.Product.ProductAllergies = productAllergies;
                _manager.AddProduct(model.Product);
            }

            return RedirectToAction("NewProduct", "Database");
        }

        [HttpGet("/Database/EditProduct/{id:int?}")]
        public IActionResult EditProduct(int id = 1)
        {
            WinkelViewModel model = new WinkelViewModel();
            model.Allergies = _manager.GetAllergies().ToList();
            model.Product = _manager.GetProduct(id);

            model.SelectedAllergies = new List<WinkelItem>();
            foreach (Allergie allergie in model.Allergies)
            {
                bool selected = false;
                foreach (ProductAllergie productAllergie in model.Product.ProductAllergies)
                {
                    if (productAllergie.Allergie.Naam.Equals(allergie.Naam))
                    {
                        selected = true;
                    }
                }
                model.SelectedAllergies.Add(new WinkelItem()
                {
                    Id = allergie.AllergieId,
                    Name = allergie.Naam,
                    Selected = selected

                });

            }


            return View(model);
        }

        [HttpPost("/Database/EditProduct/{id:int?}")]
        public async Task<ActionResult> EditProduct(int id, WinkelViewModel model)
        {
            if (model.ImageFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await model.ImageFile.CopyToAsync(memoryStream);

                    // Upload the file if less than 8 MB
                    if (memoryStream.Length < 8388608)
                    {
                        model.Product.ImageData = memoryStream.ToArray();
                    }
                    else
                    {
                        ModelState.AddModelError("File", "The file is too large.");
                    }
                }
            }

            List<ProductAllergie> productAllergies = new List<ProductAllergie>();
            if (ModelState.IsValid)
            {
                foreach (WinkelItem item in model.SelectedAllergies)
                {
                    if (item.Selected)
                    {
                        productAllergies.Add(new ProductAllergie
                        {
                            Product = model.Product,
                            Allergie = _manager.GetAllergie(item.Id)
                        });
                    }


                }
                Product toChange = _manager.GetProduct(id);
                toChange.ProductAllergies = productAllergies;
                toChange.Naam = model.Product.Naam;
                toChange.Beschrijving = model.Product.Beschrijving;
                if (model.ImageFile != null)
                {
                    toChange.ImageData = model.Product.ImageData;
                }

                model.Product.ProductAllergies = productAllergies;
                _manager.ChangeProduct(toChange);
            }

            return RedirectToAction("IndexAdmin", "Database");
        }
        public IActionResult IndexAdmin(string modelIndex, string search, int id)
        {
            ProductIndexModel model;
            if (id != 0)
            {
                model = new ProductIndexModel
                {
                    Allergies = _manager.GetAllergies().ToList(),
                    Products = new List<Product>(),
                    WinkelItems = new List<WinkelItem>()
                };
                model.Products.Add(_manager.GetProduct(id));
                foreach (Allergie allergie in _manager.GetAllergies().ToList())
                {
                    model.WinkelItems.Add(new WinkelItem
                    {
                        Id = allergie.AllergieId,
                        Name = allergie.Naam
                    });
                }
            }
            else
            {
                if (search != null)
                {
                    IList<Product> products = new List<Product>();
                    IList<Allergie> allergies = _manager.GetAllergies().ToList();

                    foreach (Product product in _manager.GetProducts().ToList())
                    {
                        if (RemoveDiacritics(product.Naam.ToLower()).Contains(RemoveDiacritics(search.ToLower())))
                        {
                            products.Add(product);
                        }
                    }

                    model = new ProductIndexModel
                    {
                        Allergies = allergies,
                        Products = products,
                        WinkelItems = new List<WinkelItem>()
                    };
                    foreach (Allergie allergie in allergies)
                    {
                        model.WinkelItems.Add(new WinkelItem
                        {
                            Id = allergie.AllergieId,
                            Name = allergie.Naam
                        });
                    }
                }
                else
                {
                    if (modelIndex != null)
                    {
                        model = new ProductIndexModel();
                        if (modelIndex.Length > 0)
                        {
                            IList<Product> products = new List<Product>();
                            IList<Allergie> allergies = _manager.GetAllergies().ToList();
                            IList<String> strings = modelIndex.Split("/").ToList();

                            foreach (string modelIndexWinkelItem in strings)
                            {
                                foreach (Product product in _manager.GetProducts().ToList())
                                {
                                    if (product.ProductAllergies.Find(x => x.Allergie.Naam == modelIndexWinkelItem) != null)
                                    {
                                        products.Add(product);
                                    }
                                }

                            }

                            model = new ProductIndexModel
                            {
                                Allergies = allergies,
                                Products = products,
                                WinkelItems = new List<WinkelItem>()
                            };
                            foreach (Allergie allergie in allergies)
                            {
                                model.WinkelItems.Add(new WinkelItem
                                {
                                    Id = allergie.AllergieId,
                                    Name = allergie.Naam,
                                    Selected = modelIndex.Contains(allergie.Naam) ? true : false
                                });
                            }
                        }
                    }
                    else
                    {
                        IList<Product> products = _manager.GetProducts().ToList();
                        IList<Allergie> allergies = _manager.GetAllergies().ToList();
                        model = new ProductIndexModel
                        {
                            Allergies = allergies,
                            Products = products,
                            WinkelItems = new List<WinkelItem>()
                        };
                        foreach (Allergie allergie in allergies)
                        {
                            model.WinkelItems.Add(new WinkelItem
                            {
                                Id = allergie.AllergieId,
                                Name = allergie.Naam
                            });
                        }
                    }
                }
            }

            return View(model);
        }
        [HttpPost]
        public IActionResult IndexNewAdmin(ProductIndexModel model)
        {
            string index = "";
            foreach (WinkelItem modelWinkelItem in model.WinkelItems)
            {
                if (modelWinkelItem.Selected)
                {
                    index += modelWinkelItem.Name + "/";
                }
            }

            return RedirectToRoute(new
            {
                controller = "Database",
                action = "IndexAdmin",
                modelIndex = index,
            });
        }

        [HttpPost]
        public IActionResult IndexSearchAdmin(ProductIndexModel model)
        {
            return RedirectToRoute(new
            {
                controller = "Database",
                action = "IndexAdmin",
                modelIndex = "",
                search = model.SearchString,
            });
        }

        public IActionResult NewAllergie()
        {
            return View();
        }
        public IActionResult IndexAllergie()
        {
            return View(_manager.GetAllergies().ToList());
        }
        public IActionResult DeleteAllergie(int id)
        {
            _manager.DeleteAllergie(_manager.GetAllergie(id));
            return RedirectToAction("IndexAllergie", "Database");
        }
        [HttpGet("/Database/EditAllergie/{id:int?}")]
        public IActionResult EditAllergie(int id)
        {
            return View(_manager.GetAllergie(id));
        }
        [HttpPost]
        public IActionResult NewAllergie(Allergie allergie)
        {
            if (ModelState.IsValid)
            {
                _manager.AddAllergie(allergie);
                return RedirectToAction("IndexAllergie", "Database");
            }

            return View();
        }

        [HttpPost("/Database/EditAllergie/{id:int?}")]
        public IActionResult EditAllergie(int id, Allergie allergie)
        {
            if (ModelState.IsValid)
            {
                Allergie aToC = _manager.GetAllergie(allergie.AllergieId);
                aToC.Naam = allergie.Naam;
                aToC.Beschrijving = allergie.Beschrijving;
                _manager.ChangeAllergie(aToC);
                return RedirectToAction("IndexAllergie", "Database");
            }

            return View();
        }

        public IActionResult IndexOpeningsuur()
        {
            return View(_manager.GetOpeningstijden().ToList());
        }

        public IActionResult EditOpeningsuur(int id)
        {
            return View(_manager.GetOpeningsUur(id));
        }
        [HttpPost("/Database/EditOpeningsuur/{id:int?}")]
        public IActionResult EditOpeningsuur(int id, OpeningsUur openingsUur)
        {
            if (ModelState.IsValid)
            {
                _manager.ChangeOpeningsUur(openingsUur);
                return RedirectToAction("IndexOpeningsuur", "Database");
            }

            return View();
        }
        public IActionResult IndexVakantie()
        {
            return View(_manager.GetVakanties().ToList());
        }

        public IActionResult NewVakantie()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewVakantie(Vakantie vakantie)
        {
            if (ModelState.IsValid)
            {
                _manager.AddVakantie(vakantie);
                return RedirectToAction("IndexVakantie", "Database");
            }

            return View();
        }

        public IActionResult EditVakantie(int id)
        {
            return View(_manager.GetVakantie(id));
        }

        [HttpPost("/Database/EditVakantie/{id:int?}")]
        public IActionResult EditVakantie(int id, Vakantie vakantie)
        {
            if (ModelState.IsValid)
            {
                _manager.ChangeVakantie(vakantie);
                return RedirectToAction("IndexVakantie", "Database");
            }

            return View();
        }
        public IActionResult DeleteVakantie(int id)
        {
            _manager.DeleteVakantie(_manager.GetVakantie(id));
            return RedirectToAction("IndexVakantie", "Database");
        }

        public IActionResult EditHomeImage(int id)
        {
            return View(new HomeViewModel{homeImage = _manager.GetHomeImage(id) });
        }

        [HttpPost("/Database/EditHomeImage/{id:int?}")]
        public async Task<ActionResult> EditHomeImage(int id, HomeViewModel model)
        {
            if (model.ImageFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await model.ImageFile.CopyToAsync(memoryStream);

                    // Upload the file if less than 8 MB
                    if (memoryStream.Length < 8388608)
                    {
                        model.homeImage.ImageData = memoryStream.ToArray();
                    }
                    else
                    {
                        ModelState.AddModelError("File", "The file is too large.");
                    }
                }
            }
            HomeImage toChange = _manager.GetHomeImage(id);
                if (model.ImageFile != null)
                {
                    toChange.ImageData = model.homeImage.ImageData;
                }
            toChange.Text = model.homeImage.Text;
                
                _manager.ChangeHomeImage(toChange);
            

            return RedirectToAction("IndexHomeImage", "Database");
        }
            public IActionResult DeleteHomeImage(int id)
        {
            _manager.DeleteHomeImage(_manager.GetHomeImage(id));
            return RedirectToAction("IndexHomeImage", "Database");
        }

        public IActionResult IndexHomeImage()
        {
            return View(_manager.GetHomeImages().ToList());
        }

        public IActionResult NewHomeImage()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> NewHomeImage(HomeViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await model.ImageFile.CopyToAsync(memoryStream);

                    // Upload the file if less than 8 MB
                    if (memoryStream.Length < 8388608)
                    {
                        model.homeImage.ImageData = memoryStream.ToArray();
                    }
                    else
                    {
                        ModelState.AddModelError("File", "The file is too large.");
                    }
                    _manager.AddHomeImage(model.homeImage);
                }
            }
            
            return RedirectToAction("NewHomeImage", "Database");
        }

        public FileContentResult getImg(int id)
        {
            byte[] byteArray = _manager.GetProduct(id).ImageData;
            return byteArray != null
                ? new FileContentResult(byteArray, "image/jpeg")
                : null;
        }


    }
}
