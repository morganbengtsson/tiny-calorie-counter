using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System;
using Microsoft.AspNetCore.Html;
using System.Collections.Generic;
using Diet.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace Diet.ViewModels
{
    public class HomeView {
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
    }
}