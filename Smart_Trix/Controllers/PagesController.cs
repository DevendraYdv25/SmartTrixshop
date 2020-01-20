using Smart_Trix.Models.StData; //For pageDT
using Smart_Trix.Models.StViewModel.StPages; //For Pagevm
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Smart_Trix.Controllers
{
    public class PagesController : Controller
    {
        // GET: Pages
        public ActionResult Index(string page = "")
        {
            //Get set page slug
            if (page == "")
            {
                page = "home";
            }

            //Declare the model and DT
            PageVM model;
            StPageDT dt;

            //check if page exists
            using (SmartDB db = new SmartDB())
            {
                if (!db.Pages.Any(a => a.Slug.Equals(page)))
                {
                    return RedirectToAction("Index", new { page = "" });
                }
            }

            //Get page  DT
            using (SmartDB db = new SmartDB())
            {
                dt = db.Pages.Where(a => a.Slug == page).FirstOrDefault();
            }

            //set page title
            ViewBag.PageTitle = dt.Title;

            //check the sidebar
            if (dt.HasSidebar == true)
            {
                ViewBag.Sidebar = "Yes";
            }
            else
            {
                ViewBag.Sidebar = "NO";
            }

            //init the model
            model = new PageVM(dt);

            return View(model);
         }

        //*******************PageMenu Partial code ******************//
        public ActionResult PagesMenuPartial()
        {
            //Declare a list of pagevm
            List<PageVM> pagelist;

            //Get all the page Except
            using (SmartDB db=new SmartDB())
            {
                pagelist = db.Pages.ToArray().OrderBy(a => a.Sorting).Where(a => a.Slug != "home").Select(a=>new PageVM(a)).ToList();
            }
            //Return partial view list

            return PartialView(pagelist);
        }

        //****************************SidebarPartial******************//
        public ActionResult SidebarPartial()
        {
            //Declare Model
            SidebarVM model;

            using (SmartDB db=new SmartDB())
            {
                StSidebarDT dt = db.Sidebar.Find(1);

                model = new SidebarVM(dt);
            }
            //Return partial view with model
            return PartialView(model);
        }

    }
}