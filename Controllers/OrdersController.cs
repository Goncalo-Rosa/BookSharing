using System.Threading.Tasks;
using BookSharing.Data;
using BookSharing.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BookSharing.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BookSharing.Controllers
{
    public class OrdersController : Controller
    {
        private readonly BookContext _context;

        private readonly UserManager<IdentityUser> _userManager;

        public OrdersController(BookContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var orders = _context.Orders.Where(o => o.RequisitorEmail != user.Email);

            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(int BookId, string RequisitorEmail, string BookName)
        {

            if (ModelState.IsValid)
            {
                var order = new Orders
                {
                    Book = await _context.Books.FindAsync(BookId),
                    BookName = BookName,
                    RequisitorEmail = RequisitorEmail
                };

                _context.Add(order);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("IndexShare", "Book");
        }

        [HttpPost]
        public async Task<IActionResult> AcceptOrder(int OrdersId, string BookName, string Email)
        {

            if (ModelState.IsValid)
            {

                var orderConfimation = await _context.Orders.FindAsync(OrdersId);

                if (orderConfimation == null)
                {
                    return NotFound();
                }
                var user = await _userManager.GetUserAsync(User);

                var result = _context.Books.SingleOrDefault(b => b.EmailCreator == user.Email && b.BookName == BookName);

                result.isRented = true;

                _context.Orders.Remove(orderConfimation);

                var xpto = _context.Orders.Where(b => b.BookName == BookName).ToList();

                _context.Orders.RemoveRange(xpto);

                var acceptedOrders = new AcceptedOrders
                {
                    BookName = BookName,
                    Email = Email
                };
                _context.AcceptedOrders.Add(acceptedOrders);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("");
        }

        [HttpPost]
        public async Task<IActionResult> DeclineOrder(int OrdersId, string BookName)
        {

            if (ModelState.IsValid)
            {

                var orderConfimation = await _context.Orders.FindAsync(OrdersId);

                if (orderConfimation == null)
                {
                    return NotFound();
                }
                var user = await _userManager.GetUserAsync(User);

                var result = _context.Books.SingleOrDefault(b => b.EmailCreator == user.Email && b.BookName == BookName);

                result.isRented = false;

                _context.Orders.Remove(orderConfimation);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("");

        }

        [HttpGet]
        public async Task<IActionResult> BooksRequisited(int OrdersId, string BookName)
        {
            var user = await _userManager.GetUserAsync(User);

            var books = _context.AcceptedOrders.Where(b => b.Email == user.Email);

            return View(books);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveBooksRequisited(int AcceptedOrdersId, string BookName, string email)
        {
            if (ModelState.IsValid)
            {

                var requisition = await _context.AcceptedOrders.FindAsync(AcceptedOrdersId);

                if (requisition == null)
                {
                    return NotFound();
                }
                var user = await _userManager.GetUserAsync(User);

                var result = _context.Books.SingleOrDefault(b => b.EmailCreator != email && b.BookName == BookName);

                result.isRented = false;

                _context.AcceptedOrders.Remove(requisition);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("BooksRequisited", "Orders");
        }
    }
}