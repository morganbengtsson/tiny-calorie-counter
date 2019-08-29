using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Diet.Contexts;
using Diet.Models;

namespace Diet.Controllers
{ 
    [Authorize]
    public class AdminController : Controller
    {
        private readonly MainContext context;

        public AdminController(MainContext dietContext)
        {
            context = dietContext;
        }

        public string ListUsers()
        {     
            string result = "";

            foreach(var user in context.Users) {
                result += user.UserName + System.Environment.NewLine;
            }
            return result;
        }

    }
}