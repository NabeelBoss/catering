using Microsoft.AspNetCore.Mvc;
using OnlineCatering.Data;
using OnlineCatering.Models;

namespace OnlineCatering.Controllers
{
    public class AdminController : Controller
    {
        private readonly OnlineCateringContext databse;
        private readonly IHttpContextAccessor contx;


        public AdminController(ILogger<HomeController> logger, OnlineCateringContext databse, IHttpContextAccessor contx)
        {
            this.databse = databse;
            this.contx = contx;




        }

        private void AdminData()
        {
            var AdminId = contx.HttpContext.Session.GetInt32("AdminId");
            var AdminName = contx.HttpContext.Session.GetString("AdminName");
            var AdminImage = contx.HttpContext.Session.GetString("AdminImage");
            var AdminEmail = contx.HttpContext.Session.GetString("AdminEmail");
            var AdminPassword = contx.HttpContext.Session.GetString("AdminPassword");
            var AdminRole = contx.HttpContext.Session.GetString("AdminRole");

            ViewBag.AdminId = AdminId;
            ViewBag.AdminName = AdminName;
            ViewBag.AdminImage = AdminImage;
            ViewBag.AdminEmail = AdminEmail;
            ViewBag.AdminPassword = AdminPassword;
            ViewBag.AdminRole = AdminRole;

        }

        public IActionResult Index()
        {
            AdminData();
            return View();
        }

        [HttpGet]
        public IActionResult Createuser()
        {
            AdminData();

            return View();
        }

        [HttpPost]
        public IActionResult Createuser(UserInfo deta)
        {
            databse.UserInfos.Add(deta);
            deta.UserRole = "Customer";
            databse.SaveChanges();
            return RedirectToAction("Userlist");
        }

        [HttpGet]
        public IActionResult Userlist()
        {
            AdminData();
            var customers = databse.UserInfos.Where(u => u.UserRole == "Customer").ToList();

            return View(customers);
        }



        [HttpGet]
        public IActionResult updateuser(int id)
        {
            AdminData();
            var front = databse.UserInfos.FirstOrDefault(options => options.UserId == id);
            return View(front);
        }

        [HttpPost]
        public IActionResult UpdateUser(UserInfo newdata)
        {
            var back = databse.UserInfos.Find(newdata.UserId);

            back.UserName = newdata.UserName;
            back.UserEmail = newdata.UserEmail;
            back.UserPassword = newdata.UserPassword;
            back.UserAddress = newdata.UserAddress;
            back.UserPhoneNo = newdata.UserPhoneNo;

            databse.SaveChanges();
            return RedirectToAction("UserList");
        }


        public IActionResult deleteuser(int id)
        {
            var findduser = databse.UserInfos.Find(id);
            var remove = databse.UserInfos.Remove(findduser);
            databse.SaveChanges();
            return RedirectToAction("Userlist");
        }




        [HttpGet]
        public IActionResult Createcaterer()
        {
            AdminData();
            return View();
        }

        [HttpPost]
        public IActionResult Createcaterer(UserInfo data, IFormFile image)
        {

            if (image != null && image.Length > 0)
            {
                var fileExt = System.IO.Path.GetExtension(image.FileName).Substring(1);
                var random = Path.GetFileName(image.FileName);
                var FileName = Guid.NewGuid() + random;

                string imgFolder = Path.Combine(HttpContext.Request.PathBase.Value, "wwwroot/UserImage");

                if (!Directory.Exists(imgFolder))
                {
                    Directory.CreateDirectory(imgFolder);
                }

                string filepath = Path.Combine(imgFolder, FileName);

                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }

                var dbAddress = Path.Combine("UserImage", FileName);

                data.UserImage = dbAddress;

                databse.UserInfos.Add(data);

                data.UserRole = "Caterer";

                databse.SaveChanges();

                return RedirectToAction("catererlist");
            }

            return View();
        }



        [HttpGet]
        public IActionResult catererlist()
        {
            AdminData();
            var customers = databse.UserInfos.Where(u => u.UserRole == "Caterer").ToList();

            return View(customers);
        }

        [HttpGet]
        public IActionResult add()
        {
            AdminData();
            return View();
        }




        public IActionResult lists()
        {
            AdminData();
            return View();
        }




        [HttpPost]
        public IActionResult add(UserInfo addsuperadmin, IFormFile adminimg)
        {
            if (adminimg != null && adminimg.Length > 0)
            {
                // GETTING IMAGE FILE EXTENSION 
                var fileExt = System.IO.Path.GetExtension(adminimg.FileName).Substring(1);

                // GETTING IMAGE NAME
                var random = Path.GetFileName(adminimg.FileName);

                // GUID ID COMBINE WITH IMAGE NAME - TO ESCAPE IMAGE NAME REDENDNCY 
                var FileName = Guid.NewGuid() + random;

                // GET PATH OF CUSTOM IMAGE FOLDER
                string imgFolder = Path.Combine(HttpContext.Request.PathBase.Value, "wwwroot/UserImage");

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
                    adminimg.CopyTo(stream);
                }

                // READY SEND PATH TO  IMAGE TO DB  
                var dbAddress = Path.Combine("UserImage", FileName);

                // EQUALIZE TABLE (MODEL) PROPERTY WITH CUSTOM PATH 
                addsuperadmin.UserImage = dbAddress;
                //MYIMAGES/imagetodbContext.JGP

                // SEND TO TABLE 



                databse.UserInfos.Add(addsuperadmin);

                databse.SaveChanges();

                return View(); ;
            }


            return View();
        }




        [HttpGet]
        public IActionResult updatecaterer(int id)
        {
            AdminData();
            var show = databse.UserInfos.FirstOrDefault(options => options.UserId == id);
            return View(show);
        }

        [HttpPost]
        public IActionResult updatecaterer(int id, UserInfo updatedCaterer, IFormFile newImage)
        {
            var backk = databse.UserInfos.Find(id);

            backk.UserName = updatedCaterer.UserName;
            backk.UserEmail = updatedCaterer.UserEmail;
            backk.UserPassword = updatedCaterer.UserPassword;
            backk.UserPhoneNo = updatedCaterer.UserPhoneNo;

            if (newImage != null && newImage.Length > 0)
            {
                var imgFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "myImages");
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

                // Update the UserImage property
                backk.UserImage = Path.Combine("myImages", fileName);
            }

            databse.SaveChanges();
            return RedirectToAction("catererlist");
        }



        public IActionResult deletecaterer(int id)
        {
            var finduser = databse.UserInfos.Find(id);
            var removeuser = databse.UserInfos.Remove(finduser);
            databse.SaveChanges();
            return RedirectToAction("catererlist");
        }


		// Account Work

		public IActionResult MyProfile(int id)
		{

			AdminData();


			var CatererId = contx.HttpContext.Session.GetInt32("AdminId");

			id = (int)CatererId;

			var user = databse.UserInfos.FirstOrDefault(option => option.UserId == id);

			if (user == null)
			{
				return View("Error");
			}

			return View(user);

		}

		[HttpGet]
		public IActionResult AccountSettings(int id)
		{
			AdminData();

			var fetchAccount = databse.UserInfos.FirstOrDefault(option => option.UserId == id);
			return View(fetchAccount);


		}

		[HttpPost]
		public IActionResult AccountSettings(UserInfo updAcc, IFormFile avatar)
		{

			var update = databse.UserInfos.Find(updAcc.UserId);

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
				contx.HttpContext.Session.SetString("AdminImage", update.UserImage);
			}

			databse.SaveChanges();


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
