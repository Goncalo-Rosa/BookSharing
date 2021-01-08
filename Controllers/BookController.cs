using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BookSharing.Models.ViewModels;
using BookSharing.Data;
using Microsoft.EntityFrameworkCore;
using BookSharing.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Linq;

namespace BookSharing.Controllers
{
    [AllowAnonymous]
    public class BookController : Controller
    {
        private readonly BookContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _webhost;

        public BookController(BookContext context, UserManager<IdentityUser> userManager, IWebHostEnvironment webHost)
        {
            _context = context;
            _userManager = userManager;
            _webhost = webHost;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var booksOfOthers = _context.Books.Where(b => b.EmailCreator == user.Email);

            return View(booksOfOthers);
        }

        [HttpGet]
        public async Task<IActionResult> CreateBook()
        {

            var user = await _userManager.GetUserAsync(User);
            var model = new CreateBookViewModel
            {
                EmailCreator = user.Email,
                Contact = Convert.ToInt32(user.PhoneNumber),
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> CreateBook([Bind("BookName, EmailCreator, BookDescription, Author, Contact, genero, idioma, isRented, Photo")] CreateBookViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFilename = null;

                if (model.Photo != null)
                {
                    uniqueFilename = Guid.NewGuid() + "_" + model.Photo.FileName;
                    string uploadFolderPath = Path.Combine(_webhost.WebRootPath, "images", uniqueFilename);

                    using (var fs = new FileStream(uploadFolderPath, FileMode.Create))
                    {
                        model.Photo.CopyTo(fs);
                    }
                }

                var book = new Book
                {
                    BookName = model.BookName,
                    EmailCreator = model.EmailCreator,
                    BookDescription = model.BookDescription,
                    Author = model.Author,
                    Contact = model.Contact,
                    genero = model.genero,
                    idioma = model.idioma,
                    isRented = model.isRented,
                    PhotoPath = uniqueFilename
                };

                _context.Add(book);
                await _context.SaveChangesAsync();
                TempData["CreateSuccess"] = book.BookName + " created successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {

                return NotFound();

            }

            var books = await _context.Books.FindAsync(id);

            if (books == null)
            {

                return NotFound();

            }

            var model = new EditBookViewModel
            {
                BookId = books.BookId,
                BookName = books.BookName,
                BookDescription = books.BookDescription,
                Author = books.Author,
                Contact = Convert.ToInt32((await _userManager.GetUserAsync(User)).PhoneNumber),
                genero = books.genero,
                idioma = books.idioma,
                EmailCreator = (await _userManager.GetUserAsync(User)).Email,
                ExistingPhotoPath = books.PhotoPath
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookId, BookName, EmailCreator, BookDescription, Author, Contact, genero, idioma, Photo, ExistingPhotoPath")] EditBookViewModel model)
        {
            if (id != model.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string uniqueFilename = model.ExistingPhotoPath;

                if (model.Photo != null)
                {
                    uniqueFilename = Guid.NewGuid() + "_" + model.Photo.FileName;
                    string uploadFolderPath = Path.Combine(_webhost.WebRootPath, "images", uniqueFilename);
                    using (var fs = new FileStream(uploadFolderPath, FileMode.Create))
                    {
                        model.Photo.CopyTo(fs);
                    }
                }

                var book = new Book
                {
                    BookId = model.BookId,
                    BookName = model.BookName,
                    EmailCreator = model.EmailCreator,
                    BookDescription = model.BookDescription,
                    Author = model.Author,
                    Contact = model.Contact,
                    genero = model.genero,
                    idioma = model.idioma,
                    PhotoPath = uniqueFilename
                };

                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _context.Books.FindAsync(book.BookId) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (System.IO.File.Exists(Path.Combine(_webhost.WebRootPath, "images", book.PhotoPath)))
            {
                System.IO.File.Delete(Path.Combine(_webhost.WebRootPath, "images", book.PhotoPath));
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                TempData["DeleteSuccess"] = book.BookName + " deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> IndexShare(string Value, string Language)
        {
            var user = await _userManager.GetUserAsync(User);

            var booksOfOthers = _context.Books.Where(b => b.EmailCreator != user.Email && b.isRented == false);

            ViewBag.Email = user.Email;

            if (Value != null)
            {
                var booksByGenrer = _context.Books.Where(b => b.genero == Value);
                return View(booksByGenrer);
            }
            else if (Language != null)
            {
                var booksByLanguage = _context.Books.Where(b => b.idioma == Language);
                return View(booksByLanguage);
            }

            return View(booksOfOthers);
        }

    }
}