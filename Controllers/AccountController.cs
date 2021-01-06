using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BookSharing.Data;
using BookSharing.Models;
using BookSharing.Models.ViewModels;
using System;

namespace BookSharing
{
    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
        private readonly BookSharingDbContext _context;
        private readonly BookContext _contextBook;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(BookSharingDbContext context, BookContext contextBook, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _contextBook = contextBook;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ListAllUsers()
        {
            var model = _userManager.Users;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Contact()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new CreateContactViewModel
            {
                Email = user.Email
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Contact([Bind("Email, FullName, Message")] CreateContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new CreateContactViewModel
                {
                    Email = model.Email,
                    FullName = model.FullName,
                    Message = model.Message
                };
                var adminUsers = (from p in _context.Users
                                  join e in _context.UserRoles on p.Id equals e.UserId
                                  where e.RoleId == "b51d6fb0-1d08-4e6a-854c-10d1f1d84a71"
                                  select new
                                  {
                                      email = p.Email
                                  }).ToList();

                for (int i = 0; i < adminUsers.Count; i++)
                {
                    var email = new EmailComposer
                    {
                        emailAddressSender = "testeemail1@sapo.pt",
                        emailAddressRecipient = adminUsers[i].email,
                        smtpServiceAddress = "smtp.sapo.pt",
                        port = 25,
                        password = "testeEmail!12345",
                        subject = user.FullName + " is contacting the administration",
                        body = user.Message + "\nUser Email: " + user.Email,
                    };

                    email.SendEmail();
                }
            }

            return RedirectToAction("Contact");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([Bind("UserName, Email,Password,ConfirmPassword, Phone, PhoneNumber, ConfirmPhoneNumber")] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    user = await _userManager.FindByEmailAsync(user.Email);
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.ActionLink("ConfirmEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);

                    var email = new EmailComposer
                    {
                        emailAddressSender = "testeemail1@sapo.pt",
                        emailAddressRecipient = user.Email,
                        smtpServiceAddress = "smtp.sapo.pt",
                        port = 25,
                        password = "testeEmail!12345",
                        subject = "Email Confirmation",
                        body = "Please confirm your email in order to login on to our Book Sharing web application. Click on the following link...." + confirmationLink,
                    };

                    email.SendEmail();

                    ViewBag.Message = "We sent you a confirmation request for your email. Plese check your mailbox.";
                    return View("ConfirmEmail");

                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [AllowAnonymous]

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login");
            }
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.Message = "Account not exists!";
                return View();
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                ViewBag.Result = "Succeded";
                ViewBag.Message = "Your email is successfully confirmed!";
                return View();
            }

            ViewBag.Message = "Your email confirmation failed!";
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([Bind("UserName,Password,RememberMe")] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: model.RememberMe, lockoutOnFailure: true);
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user != null && !user.EmailConfirmed && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    ModelState.AddModelError(string.Empty, "Your Email is not confirmed!");
                    return View(model);
                }
                if (result.IsLockedOut)
                {
                    var a = new DateTime();
                    ModelState.AddModelError("", string.Format("Your account has been locked out for {0} minutes due to multiple failed login attempts.", user.LockoutEnd.Value.CompareTo(a)));
                }
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Login Failed");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            ViewBag.UserRoles = roles;

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Remove(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);

            if (user != null)
            {
                return View(user);
            }

            return RedirectToAction("ListAllUsers", "Account");
        }

        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveConfirmed(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);

            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {

                var x = _contextBook.Books.Where(b => b.EmailCreator == user.Email).ToList();

                _contextBook.Books.RemoveRange(x);

                _contextBook.SaveChanges();

                TempData["RemoveSuccess"] = user.UserName + "removed successfully!";
                return RedirectToAction("ListAllUsers", "Account");
            }

            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string Id)
        {

            var user = await _userManager.FindByIdAsync(Id);

            if (user == null)
            {
                return NotFound();
            }

            var listModel = new List<ManageUserRolesViewModel>();

            foreach (var item in _roleManager.Roles.ToList())
            {
                var model = new ManageUserRolesViewModel
                {
                    RoleId = item.Id,
                    RoleName = item.Name
                };

                model.IsSelected = await _userManager.IsInRoleAsync(user, item.Name) ? true : false;

                listModel.Add(model);
            }

            ViewBag.UserId = user.Id;
            ViewBag.UserName = user.UserName;

            return View(listModel);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUsersRoles(string Id, List<ManageUserRolesViewModel> model)
        {

            var user = await _userManager.FindByIdAsync(Id);

            if (user == null)
            {
                return NotFound();
            }

            foreach (var item in model)
            {
                var role = await _roleManager.FindByIdAsync(item.RoleId);

                if (item.IsSelected && !await _userManager.IsInRoleAsync(user, role.Name))
                {

                    await _userManager.AddToRoleAsync(user, role.Name);

                }
                else if (!item.IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {

                    await _userManager.RemoveFromRoleAsync(user, role.Name);

                }
            }

            return RedirectToAction("ListAllUsers", "Account");
        }
    }
}