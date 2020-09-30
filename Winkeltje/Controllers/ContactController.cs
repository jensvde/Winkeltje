using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Winkeltje.Models;

namespace Winkeltje.Controllers
{
    public class ContactController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(ContactViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MailMessage msz = new MailMessage();
                    msz.From = new MailAddress("twinkeltjecontact@jensvde.be");//Email which you are getting 
                    //from contact us page 
                    msz.To.Add("twinkeltjecontact@jensvde.be");//Where mail will be sent 
                    msz.Subject = "Vraag van " + vm.Naam;
                    msz.Body = "Naam: " + vm.Naam + "\nReply naar " + vm.Email + "\n\nBericht:\n" + vm.Bericht;
                    SmtpClient smtp = new SmtpClient();

                    smtp.Host = "send.one.com";

                    smtp.Port = 587;

                    smtp.Credentials = new System.Net.NetworkCredential
                        ("twinkeltjecontact@jensvde.be", "6ha9^DeKY8KuBq7J");

                    smtp.EnableSsl = true;

                    smtp.Send(msz);

                    ModelState.Clear();
                    ViewBag.Message = "Bedankt voor uw bericht!";
                }
                catch (Exception ex)
                {
                    ModelState.Clear();
                    ViewBag.Message = $"Onze excuses, er is een fout opgetreden: {ex.Message}";
                }
            }

            return View();
        }
    }
}
