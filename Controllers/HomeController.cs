using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Identity;
using Diet.Contexts;
using Diet.Models;
using Diet.ViewModels;
using System.Threading.Tasks;

namespace Diet.Controllers
{ 
    public class HomeController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        
        public HomeController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<ActionResult> Index()
        {     
            if (User.Identity.IsAuthenticated){
                return RedirectToRoute("diary", new { controller = "Diary", action = "Index", date = DateTime.Now.ToString("yyyy-MM-dd")});
            }       
            else {
                return View(new HomeView{ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()});
            }
        }

        public ActionResult Privacy()
        {
            return View();
        }
    }
}