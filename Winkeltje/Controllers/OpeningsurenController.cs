using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Winkeltje.Models;

namespace Winkeltje.Controllers
{
    public class OpeningsurenController : Controller
    {
        private ProductManager _manager = new ProductManager();

        // GET
        public IActionResult Index()
        {
            OpeningsurenModel model = new OpeningsurenModel
            {
                OpeningsUren = _manager.GetOpeningstijden().ToList(),
                huidigOpeningsuur = _manager.GetOpeningstijden().Single(c => c.DagVanDeWeek.ToLower().Equals(DateTime.Now.ToString("dddd").ToLower()))
            };
            if (model.huidigOpeningsuur != null)
            {
                model.IsGeopend = model.huidigOpeningsuur.isGeopend(DateTime.Now);
            }

            foreach (Vakantie vakantie in _manager.GetVakanties())
            {
                if (vakantie.HeeftVakantie(DateTime.Now))
                {
                    model.Vakantie = vakantie;
                    model.HeeftVakantie = true;
                }
            }
            return View(model);
        }
    }
}
