using Smart_Trix.Models.StData;//for SmartDB
using Smart_Trix.Models.StViewModel.Account;//for login uservm
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;//For Authentication

namespace Smart_Trix.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return Redirect("~/account/login");
        }

        //*****************Login user*********************//
        [HttpGet]
        public ActionResult Login()
        {
            //confirm user is not logged in
            string username = User.Identity.Name;

            if (!string.IsNullOrEmpty(username))
            {
                return RedirectToAction("UserProfile");
                
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(LogInUserVM model)
        {
            //check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //check if the user is valied
            bool isvalied = false;

            using (SmartDB db = new SmartDB())
            {
                if (db.Users.Any(a => a.UserName.Equals(model.UserName) && a.Password.Equals(model.Password)))
                {
                    isvalied = true;
                }
            }
            if (!isvalied)
            {
                ModelState.AddModelError("", "Invalied user name & password");
                return View(model);
            }
            else
            {
                FormsAuthentication.SetAuthCookie(model.UserName, model.Rememberme);
                return Redirect(FormsAuthentication.GetRedirectUrl(model.UserName, model.Rememberme));
            }
            
        }




        //***************Create account page(login httpget page)********************//
        // GET: Account
        [ActionName("create-account")]
       [HttpGet]
        public ActionResult CreateAccount()
        {
            return View("CreateAccount");
        }
        //******************************//
        [HttpPost]
        [ActionName("create-account")]
        public ActionResult CreateAccount(UserVM model)
        {
            //checkmodel state
            if (!ModelState.IsValid)
            {
                return View("CreateAccount",model);
            }

            //check if password is matched
            if (!model.Password.Equals(model.ConfirmPassword))
            {
                ModelState.AddModelError("", "Password donot match");
                return View("CreateAccount",model);
            }
            using (SmartDB db = new SmartDB())
            {
                //Make sure the username is unique
                if (db.Users.Any(a => a.UserName.Equals(model.UserName)))
                {
                    ModelState.AddModelError("", "UserName  " +  model.UserName  + "  already taken please change this name!");
                    model.UserName = "";
                    return View("CreateAccount", model);
                }
                //Create userDTO
                StUserDT dt = new StUserDT()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmailAddress = model.EmailAddress,
                    UserName = model.UserName,
                    Password = model.Password

                };
                //Add the DT
                db.Users.Add(dt);

                //save the DT
                db.SaveChanges();

                //Add to user role model
                int id = dt.Id;

                StUselRoleDT dts = new StUselRoleDT()
                {
                    UserId = id,
                    RoleId = 2
                };

                //Add 
                db.UserRoles.Add(dts);
                //save
                db.SaveChanges();
            }
            //Create a temp message
            TempData["message"] = "You are now registered and can login";
            //Redirect
            return Redirect("~/account/login");
        }
        //**********************signout code here*********************//


        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/account/login");
        }
        //*****************UserNavPartial code************************//
        public ActionResult UserNavPartial()
        {
            //Get the username
            string username = User.Identity.Name;

            //Declare model
            UserNavPartialVM model;

            using (SmartDB db=new SmartDB())
            {
                //Get the user
                StUserDT dt = db.Users.FirstOrDefault(a => a.UserName == username);

                //Build the model
                model = new UserNavPartialVM()
                {
                    FirstName = dt.FirstName,
                    LastName = dt.LastName
                };
            }
            //return the view with partial view model
            return PartialView(model);
        }

        //**********************UserProfileDetails*******************//
        [HttpGet]
        [ActionName("User-Profile")]
        [Authorize]
        public ActionResult UserProfileEdit()
        {
            //Get the Username
            string username = User.Identity.Name;

           
            //Declare the model
            UserProfileVM model;

            using (SmartDB db = new SmartDB())
            {
                //Get the user
                StUserDT dt = db.Users.FirstOrDefault(a => a.UserName == username);

                //Build the model
                model = new UserProfileVM(dt);
            }
            //Retun view with model
            return View("UserProfileEdit", model);
        }


        [HttpPost]
        [ActionName("User-Profile")]
        [Authorize]
        public ActionResult UserProfileEdit(UserProfileVM model)
        {
            //check if the model is valied
            if (!ModelState.IsValid)
            {
                return View("UserProfileEdit", model);
            }

            //check if the password matched if need
            if (!string.IsNullOrEmpty(model.Password))
            {
                if (!model.Password.Equals(model.ConfirmPassword))
                {
                    ModelState.AddModelError("", "The password do not matched");
                    return View("UserProfileEdit", model);
                }
            }


            using (SmartDB db=new SmartDB())
            {
                //Get the username
                string username = User.Identity.Name;

                //Make sure the username is unique
                if (db.Users.Where(a=>a.Id !=model.Id).Any(a=>a.UserName==username))
                {
                    ModelState.AddModelError("", "username " + model.UserName + "Is already taken");
                    model.UserName = "";
                    return View("UserProfileEdit", model);
                }

                //Edit DT
                StUserDT dt = db.Users.Find(model.Id);

                dt.FirstName = model.FirstName;
                dt.LastName = model.LastName;
                dt.EmailAddress = model.EmailAddress;
                dt.UserName = model.UserName;

                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    dt.Password = model.Password;
                }
                //save
                db.SaveChanges();
            }

            //Set the temData
            TempData["message"] = "You have edit your profile succefully";
            return Redirect("~/account/user-profile");
        }
    }
}