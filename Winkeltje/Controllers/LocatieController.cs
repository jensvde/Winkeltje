﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Winkeltje.Controllers
{
    public class LocatieController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
