using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using proj.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace proj.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationContext _db;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        IWebHostEnvironment _appEnvironment;
        public HomeController(ApplicationContext db, UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment appEnvironment)
        {
            _db = db;
            _signInManager = signInManager;
            _userManager = userManager;
            _appEnvironment = appEnvironment;
        }

        public IActionResult Index(string title = "", string description = "", int year1 = 2001, int year2 = 2022, int dwnl1 = 0, int dwnl2 = 1000, string author = "", bool descending = true)
        {
            try
            {
                if (descending)
                {
                    ViewBag.Book = _db.Books.Where(x => x.Title.Contains(title) && x.Description.Contains(description) && x.Author.Contains(author) && x.Year >= year1 && x.Year <= year2 && x.Downloads >= dwnl1 && x.Downloads <= dwnl2).OrderByDescending(x => x.Id).ToList();
                }
                else
                {
                    ViewBag.Book = _db.Books.Where(x => x.Title.Contains(title) && x.Description.Contains(description) && x.Author.Contains(author) && x.Year >= year1 && x.Year <= year2 && x.Downloads >= dwnl1 && x.Downloads <= dwnl2).OrderBy(x => x.Id).ToList();
                }
                return View();
            }
            catch (Exception)
            {

                return RedirectToAction("Error");
            }
        }

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult CreateBook()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        [RequestSizeLimit(209715200)]
        public IActionResult CreateBook(string title, string description, string author, int year , IFormFile formFile, IFormFile uploadedFile)
        {
            try
            {
                var dateStr = DateTime.Now.ToString().Replace(".", "").Replace(" ", "").Replace(":", "");

                if (formFile != null)
                {
                    string formfile = "/img/" + dateStr + formFile.FileName;
                    using (var filestream = new FileStream(_appEnvironment.WebRootPath + formfile, FileMode.Create))
                    {
                        formFile.CopyTo(filestream);
                    }
                }
                if (uploadedFile != null)
                {
                    string path = "/Files/" + dateStr + dateStr + uploadedFile.FileName;
                    using (var filestream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        formFile.CopyTo(filestream);
                    }
                }
                Book book = new Book();
                book.Title = title;
                book.Description = description;
                book.Author = author;
                book.Year = year;
                book.Img = "/img/" + dateStr + formFile.FileName;
                book.File = "/Files/" + dateStr + dateStr + uploadedFile.FileName;
                _db.Books.Add(book);
                _db.SaveChanges();
                return View();
            }
            catch (Exception)
            {

                return RedirectToAction("Error");
            }
        }
    }
}
