using Smart_Trix.Models.StData;
using Smart_Trix.Models.StViewModel.StShop;
using System;
using System.Collections.Generic;
using System.IO;//for Directory image
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Smart_Trix.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Pages");
        }


        //******************CategoryMenupartial code to show categgory name**********************//

        public ActionResult CategoryMenuPartial()
        {
            //Declare the list of categoryvm
            List<CategoryVM> listofcategoryvm;

            //init the list
            using (SmartDB db = new SmartDB())
            {
                listofcategoryvm = db.Category.ToArray().OrderBy(a => a.Sorting).Select(a => new CategoryVM(a)).ToList();
            }
            //return the partial view with model
            return PartialView(listofcategoryvm);
        }

        //************************Category code to category Details******************//
       
        public ActionResult Category(string name)
        {
            //Declare a product of productvm
            List<ProductVM> listofproductvm;

            using (SmartDB db=new SmartDB())
            {
                //Get the category id
                StCategoryDT dt = db.Category.Where(a => a.Slug == name).FirstOrDefault();
                int catId = dt.Id;

                //init the list
                 listofproductvm = db.Product.ToArray().Where(a => a.CategoryId == catId).Select(a => new ProductVM(a)).ToList();

                //Get category name
                  var productcat = db.Product.Where(a => a.CategoryId == catId).FirstOrDefault();
                  ViewBag.categoryname = productcat.CategoryName;
            }
            //return view with list
            return View(listofproductvm);
        }


        //*********************product-details code *********************//
        [ActionName("product-details")]
        public ActionResult ProductDetails(string name)
        {
            //Declare the productvm amd DT
            StProductDT dt;
            ProductVM model;

            //init the product id
            int id = 0;

            using (SmartDB db=new SmartDB())
            {
                //Check if product exists
                if (!db.Product.Any(a => a.Slug.Equals(name)))
                {
                    return RedirectToAction("Index", "Shop");
                }

                //init the prodct DT
                dt = db.Product.Where(a => a.Slug.Equals(name)).FirstOrDefault();

                //get the id
                id = dt.Id;

                //init the model
                model = new ProductVM(dt);
            }

            //Get all the Gallery images
            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
            .Select(fn => Path.GetFileName(fn));
            
            //return view with model
            return View("ProductDetails",model);
        }
    }
}