using OnlineCatering.Data;
using OnlineCatering.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient.Server;
using System.Diagnostics;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.EntityFrameworkCore;

namespace OnlineCatering.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor contx;
        private readonly OnlineCateringContext dbase;


        public HomeController(ILogger<HomeController> logger, OnlineCateringContext dbase, IHttpContextAccessor contx)
		{
			_logger = logger;
            this.dbase = dbase;
            this.contx = contx;
        }

        public IActionResult Index()
        {
            var catererTable = dbase.UserInfos.Where(u => u.UserRole == "Caterer").ToList();

            var indexInfo = new HomeViewModel
            {
                menuTable = dbase.Menus.Take(5).ToList(),
                categoryTable = dbase.Categories.ToList(),
                userTable = catererTable
            };
            return View(indexInfo);
        }

        public IActionResult Shopmain()
        {
            var menu = dbase.Menus.Include(menu => menu.CatererMenuNavigation).Include(menu => menu.MenuCatNavigation).ToList();

            return View(menu);
        }

        public IActionResult Collectionmain()
        {
            var caterers = dbase.UserInfos.Where(u => u.UserRole == "Caterer").ToList();

            return View(caterers);
        }

        [HttpGet]
        public IActionResult BookingDetails(int id)
        {
            var userInfoWithMenus = dbase.UserInfos.Include(u => u.Menus.Take(4)).FirstOrDefault(u => u.UserId == id);

            return View(userInfoWithMenus);
        }

        [HttpGet]
        public IActionResult BookingForm(int id)
        {
            // Assuming id represents the caterer's ID (UserId)
            var catererInfo = dbase.UserInfos.FirstOrDefault(c => c.UserId == id);

            if (catererInfo == null)
            {
                return NotFound();
            }

            // Get menu options for the selected caterer
            var menus = dbase.Menus
                            .Where(m => m.CatererMenu == id) 
                            .ToList();

            // Get venue options for the selected caterer
            var venues = dbase.Venues
                             .Where(v => v.CatererVenue == id)
                             .ToList();

            var viewModel = new BookingViewModel
            {
                BookingInfo = new Booking(),
                Menus = menus,
                Venues = venues
            };

            return View(viewModel);
        }


        [HttpPost]
        public IActionResult BookingForm(int id, BookingViewModel viewModel)
        {


            viewModel.BookingInfo.BookingPrice = 200 * viewModel.BookingInfo.TotalGuest;
            viewModel.BookingInfo.BookingDate = DateTime.Now;


            var menus = dbase.Menus
                             .Where(m => m.CatererMenu == id)
                             .ToList();

            var venues = dbase.Venues
                               .Where(v => v.CatererVenue == id)
                               .ToList();

            viewModel.Menus = menus;
            viewModel.Venues = venues;


            dbase.Bookings.Add(viewModel.BookingInfo);
            dbase.SaveChanges();

            return RedirectToAction("Cart", new { id = id });

        }




        [HttpGet]
        public IActionResult Cart(int id)
        {
            // Retrieve the booking associated with the provided ID
            var booking = dbase.Bookings
                               .Include(b => b.BookingCatererNavigation)
                               .Include(b => b.BookingMenuNavigation)
                               .Include(b => b.BookingVenueNavigation)
                               .Where(u => u.BookingId == id)
                               .ToList(); // Fetch as a list

            if (booking == null || !booking.Any())
            {
                return NotFound();
            }

            var userId = booking.First().BookingCaterer; // Assuming BookingCaterer is the property that stores the user ID in Booking entity
            var caterers = dbase.UserInfos.Where(u => u.UserId == userId).ToList();
            var menus = dbase.Menus.Where(m => m.MenuCat == userId).ToList();
            var venues = dbase.Venues.Where(v => v.CatererVenue == userId).ToList();

            // Pass the data to the view
            ViewBag.caterer = caterers;
            ViewBag.Menuss = menus;
            ViewBag.Venuess = venues;

            return View(booking); 
        }





        public IActionResult DeleteOrder(int id)
        {
            var finduser = dbase.UserInfos.Find(id);
            var removeuser = dbase.UserInfos.Remove(finduser);
            dbase.SaveChanges();
            return RedirectToAction("catererlist");
        }

        public IActionResult About()
        {
            return View();
        }


        public IActionResult FAQ()
        {
            return View();
        }


        public IActionResult confirmation()
        {
            return View();
        }


       




        public IActionResult Checkout()
        {
            return View();
        }

        public IActionResult Contactus()
        {
            return View();
        }



        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }



        [HttpPost]
        public IActionResult Login(UserInfo nwdta)
        {
            var frontemail = nwdta.UserEmail;
            var frontpass = nwdta.UserPassword;

            var fettchuser = dbase.UserInfos.FirstOrDefault(options => options.UserEmail == frontemail && options.UserPassword == frontpass);

			if (fettchuser == null)
			{
				ViewBag.ErrorMessage = "Invalid email or password.";
				return View();
			}

			var backid = fettchuser.UserId;
            var backname = fettchuser.UserName;
            var backimage = fettchuser.UserImage;
            var backemail = fettchuser.UserEmail;
            var backpassword = fettchuser.UserPassword;
            var backaddress = fettchuser.UserAddress;
            var backphone = fettchuser.UserPhoneNo;
            var backrole = fettchuser.UserRole;



            if (backrole == "Customer")
            {
                contx.HttpContext.Session.SetInt32("CustomerId", backid);
                contx.HttpContext.Session.SetString("CustomerName", backname);
                contx.HttpContext.Session.SetString("CustomerEmail", backemail);
                contx.HttpContext.Session.SetString("CustomerPassword", backpassword);
                contx.HttpContext.Session.SetString("CustomerAddress", backaddress);
                contx.HttpContext.Session.SetString("CustomerPhoneno", backphone.ToString());
                contx.HttpContext.Session.SetString("CustomerRole", backrole);

                return RedirectToAction("Index", "Home");

            }
            else if (backrole == "Caterer")
            {
                contx.HttpContext.Session.SetInt32("CatererId", backid);
                contx.HttpContext.Session.SetString("CatererName", backname);
                contx.HttpContext.Session.SetString("CatererImage", backimage);
                contx.HttpContext.Session.SetString("CatererEmail", backemail);
                contx.HttpContext.Session.SetString("CatererPassword", backpassword);
                contx.HttpContext.Session.SetString("CatererAddress", backaddress);
                contx.HttpContext.Session.SetString("CatererPhoneno", backphone.ToString());
                contx.HttpContext.Session.SetString("CatererRole", backrole);

                return RedirectToAction("Index", "Caterer");
            }
            else if (backrole == "Admin")
            {
                contx.HttpContext.Session.SetInt32("AdminId", backid);
                contx.HttpContext.Session.SetString("AdminName", backname);
                contx.HttpContext.Session.SetString("AdminImage", backimage);
                contx.HttpContext.Session.SetString("AdminEmail", backemail);
                contx.HttpContext.Session.SetString("AdminPassword", backpassword);
                contx.HttpContext.Session.SetString("AdminRole", backrole);

                return RedirectToAction("Index", "Admin");
            }


            return View();
        }



        [HttpGet]
        public IActionResult Signupcaterer()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signupcaterer(UserInfo dat, IFormFile img)
        {

            if (img != null && img.Length > 0)
            {
                // GETTING IMAGE FILE EXTENSION 
                var fileExt = System.IO.Path.GetExtension(img.FileName).Substring(1);

                // GETTING IMAGE NAME
                var random = Path.GetFileName(img.FileName);

                // GUID ID COMBINE WITH IMAGE NAME - TO ESCAPE IMAGE NAME REDENDNCY 
                var FileName = Guid.NewGuid() + random;

                // GET PATH OF CUSTOM IMAGE FOLDER
                string imgFolder = Path.Combine(HttpContext.Request.PathBase.Value, "wwwroot/UserSignup");

                // CHECKING FOLDER EXIST OR NOT - IF NOT THEN CREATE F0LDER 
                if (!Directory.Exists(imgFolder))
                {
                    Directory.CreateDirectory(imgFolder);
                }

                // MAKING CUSTOM AND COMBINE FOLDER PATH WITH IMAHE 
                string filepath = Path.Combine(imgFolder, FileName);

                // COPY IMAGE TO REAL PATH TO DEVELOPER PATH
                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    img.CopyTo(stream);
                }

                // READY SEND PATH TO  IMAGE TO DB  
                var dbAddress = Path.Combine("UserSignup", FileName);

                // EQUALIZE TABLE (MODEL) PROPERTY WITH CUSTOM PATH 
                dat.UserImage = dbAddress;
                //MYIMAGES/imagetodbContext.JGP

                // SEND TO TABLE 
                dbase.UserInfos.Add(dat);

                dat.UserRole = "Caterer";
                dbase.SaveChanges();
                return RedirectToAction("Login");
            }

            return View();

        }


        [HttpGet]
        public IActionResult Signupcustomer()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signupcustomer(UserInfo datt)
        {

            dbase.UserInfos.Add(datt);
            datt.UserRole = "Customer";
            dbase.SaveChanges();
            return RedirectToAction("Login");
        }

        public IActionResult Account()
        {
            return View();
        }


        

        public IActionResult card()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Forget()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Forget(UserInfo forget)
        {
            var user = dbase.UserInfos.FirstOrDefault(u => u.UserEmail == forget.UserEmail);

            if (user != null)
            {
                HttpContext.Session.SetInt32("forgetid", user.UserId);
                return RedirectToAction("Forgettwo", "Home");
            }
            else
            {
                ViewBag.error = "User not found";
                return View();
            }
        }

        [HttpGet]
        public IActionResult Forgettwo()
        {
            var userId = HttpContext.Session.GetInt32("forgetid");

            if (userId.HasValue)
            {
                var user = dbase.UserInfos.Find(userId.Value);
                return View(user);
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult Forgettwo(UserInfo forgettwo)
        {
            var userId = HttpContext.Session.GetInt32("forgetid");

            if (userId.HasValue)
            {
                var user = dbase.UserInfos.Find(userId.Value);

                if (user != null)
                {
                    user.UserPassword = forgettwo.UserPassword;
                    dbase.SaveChanges();
                    return RedirectToAction("Login");
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }


       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
