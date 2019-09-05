using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;

        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet("Blog")]
        public IActionResult Blog()
        {
            List<User> AllUsers = dbContext.Users.Include(f => f.CreatedPosts).ToList();

            return View(AllUsers);
        }


        [HttpPost("Register")]

        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                var ExistingUser = dbContext.Users.Any(f => f.Email == user.Email);
                if (ExistingUser)
                {
                    ModelState.AddModelError("Email", "*This email already exist");
                    return View("Login");
                }
                var hasher = new PasswordHasher<User>();
                user.Password = hasher.HashPassword(user, user.Password);
                dbContext.Add(user);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("UserId", user.UserId);
                return RedirectToAction("Blog");
            } else {
                return View("Login");
            }
            
        }

        [HttpGet("")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("Login")]
        public IActionResult Login (User user)
        {
            if (ModelState.IsValid)
            {
                var userInDb = dbContext.Users.FirstOrDefault(a => a.Email == user.Email);

                if(userInDb == null)
                {
                    ModelState.AddModelError("Email", "Email is not registered");
                    return View("Login");
                }
                var hasher = new PasswordHasher<User>();
                var result = hasher.VerifyHashedPassword(user, userInDb.Password, user.Password);

                if(result == 0)
                {
                    ModelState.AddModelError("Password", "*Incorrect password");
                    return View("Login");
                }
                HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                return RedirectToAction("Blog");
            }

            return View("Login");
        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
