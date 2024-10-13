using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OnlineCatering.Data;
using OnlineCatering.Models;
using System;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;


namespace OnlineCatering.Controllers
{

	public class CatererController : Controller
	{

		private readonly OnlineCateringContext db;
		private readonly IHttpContextAccessor contx;




		public CatererController(OnlineCateringContext db, IHttpContextAccessor contx)
		{
			this.db = db;
			this.contx = contx;
		}

		private void CatererData()
		{
			var CatererId = contx.HttpContext.Session.GetInt32("CatererId");
			var CatererName = contx.HttpContext.Session.GetString("CatererName");
			var CatererImage = contx.HttpContext.Session.GetString("CatererImage");
			var CatererEmail = contx.HttpContext.Session.GetString("CatererEmail");
			var CatererPassword = contx.HttpContext.Session.GetString("CatererPassword");
			var CatererAddress = contx.HttpContext.Session.GetString("CatererAddress");
			var CatererPhoneno = contx.HttpContext.Session.GetString("CatererPhoneno");
			var CatererRole = contx.HttpContext.Session.GetString("CatererRole");

			ViewBag.CatererId = CatererId;
			ViewBag.CatererName = CatererName;
			ViewBag.CatererImage = CatererImage;
			ViewBag.CatererEmail = CatererEmail;
			ViewBag.CatererPassword = CatererPassword;
			ViewBag.CatererAddress = CatererAddress;
			ViewBag.CatererPhoneno = CatererPhoneno;
			ViewBag.CatererRole = CatererRole;

		}

		public IActionResult Index()
		{
			CatererData();

			return View();
		}


		// Add & List Menu Work

		public IActionResult MenuList()
		{
			CatererData();

            var CatererId = contx.HttpContext.Session.GetInt32("CatererId");

            if (CatererId.HasValue)
            {
                var menuList = db.Menus
                    .Include(menu => menu.CatererMenuNavigation)
                    .Include(menu => menu.MenuCatNavigation)
                    .Where(menu => menu.CatererMenu == CatererId.Value)
                    .ToList();

                return View(menuList);
            }

            /*var menuList = db.Menus.Include(menu => menu.CatererMenuNavigation).Include(menu => menu.MenuCatNavigation).ToList();

			return View(menuList);*/
			return View();
        }

		[HttpGet]
		public IActionResult AddMenu()
		{
			CatererData();

			MenuCustomModel menuCustomModel = new MenuCustomModel()
			{
				menuTable = new Menu(),
				categoryTable = db.Categories.ToList(),
			};
			return View(menuCustomModel);
		}

		[HttpPost]
		public IActionResult AddMenu(MenuCustomModel newMenu, IFormFile img)
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
				string imgFolder = Path.Combine(HttpContext.Request.PathBase.Value, "wwwroot/CatererDashboard/MenuImages");

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
				var dbAddress = Path.Combine("MenuImages", FileName);

				// EQUALIZE TABLE (MODEL) PROPERTY WITH CUSTOM PATH 
				newMenu.menuTable.MenuImage = dbAddress;
				//MYIMAGES/imagetodbContext.JGP

				// SEND TO TABLE 

				var catererID = contx.HttpContext.Session.GetInt32("CatererId");

				newMenu.menuTable.CatererMenu = catererID;

				db.Menus.Add(newMenu.menuTable);

				//SAVE TO DB 

				db.SaveChanges();

				return RedirectToAction("MenuList");
			}


			return View();
		}

		[HttpGet]
		public IActionResult updMenuBtn(int id)
		{

			CatererData();

			var showMenu = db.Menus.FirstOrDefault(options => options.MenuId == id);

			return View(showMenu);

		}



		[HttpPost]
		public IActionResult updMenuBtn(int id, Menu updMenu, IFormFile newImage)
		{
			var backk = db.Menus.Find(id);

			backk.MenuName = updMenu.MenuName;
			backk.MenuImage = updMenu.MenuImage;
			backk.MenuDescription = updMenu.MenuDescription;
			backk.MenuPrice = updMenu.MenuPrice;
			backk.MenuCat = updMenu.MenuCat;

			// Check if there's a new image
			if (newImage != null && newImage.Length > 0)
			{
				var imgFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "MenuImages");
				if (!Directory.Exists(imgFolder))
				{
					Directory.CreateDirectory(imgFolder);
				}

				var fileName = Guid.NewGuid().ToString() + Path.GetExtension(newImage.FileName);
				var filePath = Path.Combine(imgFolder, fileName);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					newImage.CopyTo(stream);
				}

				// Delete the previous image
				if (!string.IsNullOrEmpty(backk.MenuImage))
				{
					var previousImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", backk.MenuImage);
					if (System.IO.File.Exists(previousImagePath))
					{
						System.IO.File.Delete(previousImagePath);
					}
				}

				// Update the MenuImage property with the new image path
				backk.MenuImage = Path.Combine("MenuImages", fileName);
			}

			db.SaveChanges();
			return RedirectToAction("MenuList");
		}



		public IActionResult dltMenuBtn(int id)
		{
			var dltMenu = db.Menus.Find(id);
			db.Menus.Remove(dltMenu);
			db.SaveChanges();

			return RedirectToAction("MenuList");
		}

		// Add & List Menu Work

		// Add & List Menu Category Work

		public IActionResult MenuCategoryList()
		{
			CatererData();

			return View(db.Categories.ToList());
		}

		[HttpGet]
		public IActionResult AddMenuCategory()
		{
			CatererData();

			return View();
		}

		[HttpPost]
		public IActionResult AddMenuCategory(Category newCat, IFormFile img)
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
				string imgFolder = Path.Combine(HttpContext.Request.PathBase.Value, "wwwroot/CatererDashboard/CategoryImages");

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
				var dbAddress = Path.Combine("CategoryImages", FileName);

				// EQUALIZE TABLE (MODEL) PROPERTY WITH CUSTOM PATH 
				newCat.CategoryImage = dbAddress;
				//MYIMAGES/imagetodbContext.JGP

				// SEND TO TABLE 

				db.Categories.Add(newCat);

				//SAVE TO DB 


				db.SaveChanges();

				return RedirectToAction("MenuCategoryList");


			}

			return View();
		}

		public IActionResult dltCatBtn(int id)
		{
			var dltCat = db.Categories.Find(id);
			db.Categories.Remove(dltCat);
			db.SaveChanges();

			return RedirectToAction("MenuCategoryList");
		}

		[HttpGet]
		public IActionResult updCatBtn(int id)
		{
			CatererData();


			var fetchUser = db.Categories.FirstOrDefault(option => option.CategoryId == id);

			return View(fetchUser);
		}

		[HttpPost]
		public IActionResult updCatBtn(Category updCat, IFormFile img)
		{
			var category = db.Categories.Find(updCat.CategoryId);

			// Update existing category properties
			category.CategoryName = updCat.CategoryName;

			// Check if there's a new image
			if (img != null && img.Length > 0)
			{
				var imgFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CategoryImages");
				if (!Directory.Exists(imgFolder))
				{
					Directory.CreateDirectory(imgFolder);
				}

				// Generate unique filename using GUID and original file extension
				var fileName = $"{Guid.NewGuid()}{Path.GetExtension(img.FileName)}";
				var filePath = Path.Combine(imgFolder, fileName);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					img.CopyTo(stream);
				}

				// Delete the previous image
				if (!string.IsNullOrEmpty(category.CategoryImage))
				{
					var previousImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", category.CategoryImage);
					if (System.IO.File.Exists(previousImagePath))
					{
						System.IO.File.Delete(previousImagePath);
					}
				}

				// Update the CategoryImage property with the new image path
				category.CategoryImage = Path.Combine("CategoryImages", fileName);
			}

			db.SaveChanges();

			return RedirectToAction("MenuCategoryList");
		}






		// Add & List Menu Category Work

		// Add & List Menu Work

		public IActionResult VenueList()
		{
			CatererData();

            var CatererId = contx.HttpContext.Session.GetInt32("CatererId");

            if (CatererId.HasValue)
            {
                var venueList = db.Venues
                    .Include(option => option.CatererVenueNavigation)
                    .Where(venue => venue.CatererVenue == CatererId.Value)
                    .ToList();

                return View(venueList);
            }
			return View();
        }

		[HttpGet]
		public IActionResult AddVenue()
		{
			CatererData();

			return View();

		}

		[HttpPost]
		public IActionResult AddVenue(Venue newVen)
		{
			var catererID = contx.HttpContext.Session.GetInt32("CatererId");
			newVen.CatererVenue = catererID;

			db.Venues.Add(newVen);
			db.SaveChanges();

			return RedirectToAction("VenueList");
		}

		public IActionResult dltVenBtn(int id)
		{
			var dltVen = db.Venues.Find(id);
			db.Venues.Remove(dltVen);
			db.SaveChanges();

			return RedirectToAction("VenueList");
		}

		[HttpGet]
		public IActionResult updVenBtn(int id)
		{
			CatererData();


			var fetchVenue = db.Venues.FirstOrDefault(option => option.VenueId == id);

			return View(fetchVenue);
		}

		[HttpPost]
		public IActionResult updVenBtn(Venue updVen)
		{
			var ven = db.Venues.Find(updVen.VenueId);

			ven.VenueId = updVen.VenueId;
			ven.VenueName = updVen.VenueName;

			db.SaveChanges();
			return RedirectToAction("VenueList");

		}

		// Add & List Menu Work

		// Account Work

		public IActionResult MyProfile(int id)
		{

			CatererData();


			var CatererId = contx.HttpContext.Session.GetInt32("CatererId");

			id = (int)CatererId;

			var user = db.UserInfos.FirstOrDefault(option => option.UserId == id);

			if (user == null)
			{
				return View("Error");
			}

			return View(user);

		}

		[HttpGet]
		public IActionResult AccountSettings(int id)
		{
			CatererData();

			var fetchAccount = db.UserInfos.FirstOrDefault(option => option.UserId == id);
			return View(fetchAccount);


		}

		[HttpPost]
		public IActionResult AccountSettings(UserInfo updAcc, IFormFile avatar)
		{

			var update = db.UserInfos.Find(updAcc.UserId);

			update.UserId = updAcc.UserId;
			update.UserName = updAcc.UserName;
			update.UserImage = updAcc.UserImage;
			update.UserEmail = updAcc.UserEmail;
			update.UserPassword = updAcc.UserPassword;
			update.UserAddress = updAcc.UserAddress;
			update.UserPhoneNo = updAcc.UserPhoneNo;

			// Check if there's a new image
			if (avatar != null && avatar.Length > 0)
			{
				var imgFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UserImage");
				if (!Directory.Exists(imgFolder))
				{
					Directory.CreateDirectory(imgFolder);
				}

				var fileName = Guid.NewGuid().ToString() + Path.GetExtension(avatar.FileName);
				var filePath = Path.Combine(imgFolder, fileName);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					avatar.CopyTo(stream);
				}

				// Delete the previous image
				if (!string.IsNullOrEmpty(update.UserImage))
				{
					var previousImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", update.UserImage);
					if (System.IO.File.Exists(previousImagePath))
					{
						System.IO.File.Delete(previousImagePath);
					}
				}

				// Update the UserImage property with the new image path
				update.UserImage = Path.Combine("UserImage", fileName);

				// Update the session variable for CatererImage with the new image path
				contx.HttpContext.Session.SetString("CatererImage", update.UserImage);
			}

			db.SaveChanges();


			return RedirectToAction("MyProfile");


		}

		// Account Work


		public IActionResult Signout()
		{
			contx.HttpContext.Session.Clear();
			return RedirectToAction("Index", "Home");


		}



	}
}
