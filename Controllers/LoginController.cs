using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Alejof.SimpleBlog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Alejof.SimpleBlog.Controllers
{
    public class LoginController : Controller
    {
        public LoginController()
        {
            
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm]Models.LoginViewModel loginModel)
        {
            var role = loginModel.Username switch 
            {
                "Writer" => "writer",
                "Editor" => "editor",
                _ => "none"
            };

            var roleClaim = new Claim(ClaimTypes.Role, role);
            var nameClaim = new Claim(ClaimTypes.Name, role);
            User.AddIdentity(new ClaimsIdentity(new[] { nameClaim, roleClaim }));

            return Redirect("/");
        }
    }
}
