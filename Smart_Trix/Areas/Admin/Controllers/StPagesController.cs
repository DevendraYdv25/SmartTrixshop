using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Smart_Trix.Models.StViewModel.StPages; //for all StPages model are present here
using Smart_Trix.Models.StData; //for StData


namespace Smart_Trix.Areas.Admin.Controllers
{
    public class StPagesController : Controller
    {
        // GET: Admin/StPages
        public ActionResult Index()
        {
            //list of pagevm
            List<PageVM> pagelist;

            using (SmartDB db = new SmartDB())
            {
                //init the list
                pagelist = db.Pages.ToArray().OrderBy(a => a.Sorting).Select(a => new PageVM(a)).ToList();     
            }

            //return view with model
            return View(pagelist);
        }
        //*********************AddPage code*************//
        // GET: Admin/StPages/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }
        // POST: Admin/StPages/AddPage
        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            //Check the model state
            if (! ModelState.IsValid)
            {
                return View(model);
            }

            using (SmartDB Db=new SmartDB())
            {
                //Declare slu
                string slug;

                //init pageDT
                StPageDT dt = new StPageDT();

                //DT Title
                dt.Title = model.Title;

                //check for and set the slug if need
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }

                //Make sure the title and slug are unique
                if (Db.Pages.Any(a=>a.Title == model.Title) || Db.Pages.Any(a=>a.Slug==slug))
                {
                    ModelState.AddModelError("","The title and slug are not exists");
                    return View(model);
                }
                //Rest part DT
                dt.Slug = slug;
                dt.Body = model.Body;
                dt.HasSidebar = model.HasSidebar;
                dt.Sorting = 100; //max value of 100 items

                //save DT
                Db.Pages.Add(dt);
                Db.SaveChanges();
            }
            //set Tempmessage
            TempData["SM"] = "You have added the page successfully";
            return RedirectToAction("AddPage");
        }

        //*****************Edit code****************//

        [HttpGet]
        public ActionResult EditPage(int id)
        {
            //Declare pageVM
            PageVM model;

            using (SmartDB db=new SmartDB())
            {
                //get id
                StPageDT dt = db.Pages.Find(id);

                //Confirm page
                if (dt==null)
                {
                    return Content("The page does not exits");
                }
                //init pagevn
                model = new PageVM(dt);

            }
            return View(model);
        }

        [HttpPost]
        public ActionResult EditPage(PageVM model)
        {
            //check model
            if (! ModelState.IsValid)
            {
                return View(model);
            }

            using (SmartDB db=new SmartDB())
            {
                //Get page id from view(hidden id)
                int id = model.Id;

                //init slug
                string slug = "home";

                //get page id
                StPageDT dt = db.Pages.Find(id);

                //DT the title
                dt.Title = model.Title;

                //check for slug
                if (model.Slug !="home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                }
                //make sure the title and slug is unique
                if (db.Pages.Where(a=>a.Id !=id).Any(a=>a.Title==model.Title) || 
                    db.Pages.Where(a=>a.Id !=id).Any(a=>a.Slug==slug))
                {
                    ModelState.AddModelError("","The title and slug are exits");
                    return View(model);
                }
                //Rest Dt
                dt.Slug = slug;
                dt.Body = model.Body;
                dt.HasSidebar = model.HasSidebar;

                //save 
                db.SaveChanges();
            }
            //Set tempMessage
            TempData["SM"] = "The page successfully updated";
            return RedirectToAction("AddPage");
        }

        //**********************Details code****************//
        public ActionResult PageDetails(int id)
        {
            //Declare model
            PageVM model;

            using (SmartDB db=new SmartDB())
            {
                //get page
                StPageDT dt = db.Pages.Find(id);

                //confirm page
                if (dt==null)
                {
                    return Content("The page not exits");
                }
                //init pagevm
                model = new PageVM(dt);
            }
            return View(model);
        }

        //*****************Delete code ****************//
        public ActionResult DeletePage(int id)
        {
            using (SmartDB db=new SmartDB())
            {
                //get id
                StPageDT dt = db.Pages.Find(id);
                //Remove
                db.Pages.Remove(dt);
                //save
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        //***********************Recorder dode***********************//
        public void RecorderPages(int[] id)
        {
            using (SmartDB db =new SmartDB())
            {
                //set the intial count
                int count = 1; //becase home o place

                //Declare the page Dt
                StPageDT dt;
                foreach (var pageid in id)
                {
                    dt = db.Pages.Find(id);
                    dt.Sorting = count;
                    db.SaveChanges();
                    count++;
                }
            }
        }


        //*********************Edit Sidebar code***********************//
        [HttpGet]
        public ActionResult EditSidebar()
        {
            //Declare model
            SidebarVM model;
            using (SmartDB db=new SmartDB())
            {
                //Get the DT
                StSidebarDT dt = db.Sidebar.Find(1);
                //Init model
                model=new SidebarVM(dt);

            }
            //Return with model
            return View(model);
        }
        [HttpPost]
        public ActionResult EditSidebar(SidebarVM model)
        {
            using (SmartDB db=new SmartDB())
            {
                //Get DT
                StSidebarDT dt = db.Sidebar.Find(1);

                //Dt the body
                dt.Body = model.Body;
                //save 
                db.SaveChanges();
            }
            //Set the TemData Message
            TempData["SM"] = "You have Edit the sidebar successfully!";
            //Redirect the EditSidebar
            return RedirectToAction("EditSidebar");
        }
    }
}