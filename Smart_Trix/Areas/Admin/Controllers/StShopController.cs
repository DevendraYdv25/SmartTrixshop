using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Smart_Trix.Models.StViewModel.StShop;//For access Categoryvm
using Smart_Trix.Models.StData; //To access the Db
using System.IO; //for image Directory
using System.Web.Helpers; //for WebImage
using PagedList;//For pagination

namespace Smart_Trix.Areas.Admin.Controllers
{
    public class StShopController : Controller
    {
        // GET: Admin/StShop
        public ActionResult Categories()
        {
            //Declare the list of model
            List<CategoryVM> Categoryvmlist;

            using (SmartDB db =new SmartDB())
            {
                Categoryvmlist = db.Category
                    .ToArray()
                    .OrderBy(a => a.Sorting)
                    .Select(a => new CategoryVM(a))
                    .ToList();
            }
            //Return the view with list
            return View(Categoryvmlist);
        }

        //****************AddNewCategory***************//
        [HttpPost]
        public string AddNewCategory(string catName)
        {
            //Declare id
            string id;

            using (SmartDB db =new SmartDB())
            {
                //Check the category is unique
                if (db.Category.Any(a=>a.Name == catName))
                {
                    return "TitleTaken";
                }

                //init the DT
                StCategoryDT dt = new StCategoryDT();

                //Add the DT
                dt.Name = catName;
                dt.Slug = catName.Replace(" ", "-").ToLower();
                dt.Sorting = 100;

                //saveDT
                db.Category.Add(dt);
                db.SaveChanges();

                //Get DT Id
                id = dt.Id.ToString();
                return id;
            }
        }


        //***********************Recorder category code***********************//
        public void RecorderCategories(int[] id)
        {
            using (SmartDB db = new SmartDB())
            {
                //set the intial count
                int count = 1; //becase home o place

                //Declare the CategoryDT 
                StCategoryDT dt;

                //set the sorting for each category
                foreach (var catid in id)
                {
                    dt = db.Category.Find(catid);
                    dt.Sorting = count;
                    db.SaveChanges();
                    count++;
                }
            }
        }


        //*****************Delete category code ****************//
        public ActionResult DeleteCategory(int id)
        {
            using (SmartDB db = new SmartDB())
            {
                //get id
                StCategoryDT dt = db.Category.Find(id);

                //Remove
                db.Category.Remove(dt);

                //save
                db.SaveChanges();
            }
            return RedirectToAction("Categories");
        }


        //*******************Rename Category code**********//
        [HttpPost]
        public string RenameCategory(string newCatName,int id)
        {
            using (SmartDB db=new SmartDB())
            {
                //check the category name is unique
                if (db.Category.Any(a=>a.Name == newCatName))
                {
                    return "TitleTaken";
                }

                //Get DT
                StCategoryDT dt = db.Category.Find(id);

                //Edit the DT
                dt.Name = newCatName;
                dt.Slug = newCatName.Replace(" ", "-").ToLower();

                //save DT
                db.SaveChanges();

            }
            //retun
            return "ok";
        }

        //*******************Category  part complete*****************************//
        

            //*************Add product code*******************//
        [HttpGet]
        public ActionResult AddProduct()
        {
            //init model
            ProductVM model = new ProductVM();


            //Add select list of category to model
            using (SmartDB db=new SmartDB())
            {
                model.Categories = new SelectList(db.Category.ToList(), "Id", "Name");
            }

            //return view model
            return View(model);
        }
        

        [HttpPost]
        public ActionResult AddProduct(ProductVM model,HttpPostedFileBase file)
        {
            //check the model is valid 
            if (! ModelState.IsValid)
            {
                using (SmartDB db=new SmartDB())
                {
                    model.Categories = new SelectList(db.Category.ToList(),"Id","Name");
                    return View(model);
                }
            }

            //make sure the product name is unique
            using (SmartDB db=new SmartDB())
            {
                if (db.Product.Any(a=>a.Name == model.Name))
                {
                    model.Categories = new SelectList(db.Category.ToList(), "Id", "Name");
                    ModelState.AddModelError(" ", "The product name is already taken");
                    return View(model);
                }
            }


            //Declare the product id 
            int id;

            //init and save the product DT
            using (SmartDB db=new SmartDB())
            {
                StProductDT yadav = new StProductDT();
                yadav.Id = model.Id;
                yadav.Name = model.Name;
                yadav.Slug = model.Name.Replace(" ", "-").ToLower();
                yadav.Description = model.Description;
                yadav.Price = model.Price;
                yadav.CategoryId = model.CategoryId;

                StCategoryDT catDT = db.Category.FirstOrDefault(a=>a.Id == model.CategoryId);
                yadav.CategoryName = catDT.Name;

                //save it
                db.Product.Add(yadav);
                db.SaveChanges();

                //get the id
                id = yadav.Id;

            }
            //Set the TempData message
            TempData["SM"] = "You have Added product successfully";

            #region Image upload

            //Create Necessary Directory
            var OriginalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

            var pathString1 =Path.Combine(OriginalDirectory.ToString(), "Products");
            var pathString2=Path.Combine(OriginalDirectory.ToString(), "Products\\" + id.ToString());
            var pathString3=Path.Combine(OriginalDirectory.ToString(), "Products\\" + id.ToString() + "\\Thumbs");
            var pathString4=Path.Combine(OriginalDirectory.ToString(),"Products\\" +id.ToString() + "\\Gallery");
            var pathString5 = Path.Combine(OriginalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery\\Thumbs");

            if (!Directory.Exists(pathString1))
            {
                Directory.CreateDirectory(pathString1);
            }
            if (! Directory.Exists(pathString2))
            {
                Directory.CreateDirectory(pathString2);
            }
            if (!Directory.Exists(pathString3))
            {
                Directory.CreateDirectory(pathString3);
            }
            if (Directory.Exists(pathString4))
            {
                Directory.CreateDirectory(pathString4);
            }
            if (! Directory.Exists(pathString5))
            {
                Directory.CreateDirectory(pathString5);
            }
            

            //check if file  was upload
            if (file !=null && file.ContentLength >0)
            {
                //Get thte file Extension
                string ext = file.ContentType.ToLower();

                //verify the extension
                if (ext !="image/jpg" && 
                    ext !="image/jpeg" && 
                    ext !="image/pjpeg" && 
                    ext !="image/gif" &&
                    ext!="image/x-png" && 
                    ext !="image/png")
                {
                    using (SmartDB db=new SmartDB())
                    {
                        model.Categories = new SelectList(db.Category.ToList(), "Id", "Name");
                        ModelState.AddModelError("", "The image was not upload -wrong extension");
                        return View(model);
                    }
                }
                //init image name
                string imagename = file.FileName;

                //save image name to DT
                using (SmartDB db=new SmartDB())
                {
                    StProductDT dt = db.Product.Find(id);
                    dt.ImageName = imagename;

                    db.SaveChanges();
                }

                //set the original and thum imagepaths
                var path1 = string.Format("{0}\\{1}", pathString2, imagename);
                var path2 = string.Format("{0}\\{1}", pathString3, imagename);

                //save the original path
                file.SaveAs(path1);

                //create and thums the save
                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200);
                img.Save(path2);
            }
            #endregion
            return RedirectToAction("AddProduct");
        }

        //***********************Pagination code *****************************//
        public ActionResult Products(int? page,int? catId)
        {
            //Declare of list of productVM
            List<ProductVM> listofproductvm;


            //set the page number
            var PageNumber = page ?? 1;

            using (SmartDB db=new SmartDB())
            {
                //init the list
                listofproductvm = db.Product.ToArray()
                    .Where(a => catId == null || catId == 0 || a.CategoryId == catId)
                    .Select(a=>new ProductVM(a))
                    .ToList();

                //populate categories select list
                ViewBag.Categories = new SelectList(db.Category.ToList(),"Id","Name");

                //set the category
                ViewBag.SelectedCat = catId.ToString();
            }
            //set the pagination
            var onePageOfProducts = listofproductvm.ToPagedList(PageNumber,3);
            ViewBag.OnePageOfProducts = onePageOfProducts;
            //return view with list
            return View(listofproductvm);
        }


        //**************************Edit product code****************//
        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            //Declare the productVM
            ProductVM model;
            using (SmartDB db=new SmartDB() )
            {
                //Get the product DT
                StProductDT dt = db.Product.Find(id);

                //Make sure product is exists
                if (dt == null)
                {
                    return Content("The product does not exists");
                }
                //iinit model
                model = new ProductVM(dt);

                //Make a select list
                model.Categories = new SelectList(db.Category.ToList(), "Id", "Name");

                    //Get all the Gallery images
                    model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                    .Select(fn => Path.GetFileName(fn));
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult EditProduct(ProductVM model,HttpPostedFileBase file)
        {
            //Get the product id
            int id = model.Id;

            //populate categories  select list and gallery image
            using (SmartDB db=new SmartDB())
            {
                model.Categories = new SelectList(db.Category.ToList(),"Id","Name");
            }
            //Get all the Gallery images
            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
            .Select(fn => Path.GetFileName(fn));

            //check the model state
            if (! ModelState.IsValid)
            {
                return View(model);
            }
            //Make sure the product name is unique
            using (SmartDB db=new SmartDB())
            {
                if (db.Product.Where(a=>a.Id !=id).Any(a=>a.Name == model.Name))
                {
                    ModelState.AddModelError("","The product is taken");
                    return View(model);
                }
            }
            

            //update product
            using (SmartDB db=new SmartDB())
            {
                StProductDT dt = db.Product.Find(id);

                dt.Name = model.Name;
                dt.Slug = model.Name.Replace(" ","-").ToLower();
                dt.Description = model.Description;
                dt.Price = model.Price;
                dt.CategoryId = model.CategoryId;
                dt.ImageName = model.ImageName;

                StCategoryDT catDT = db.Category.FirstOrDefault(a=>a.Id==model.CategoryId);
                dt.CategoryName = catDT.Name;

                db.SaveChanges();
            }
            //Set the tempdata message
            TempData["SM"] = "You have Edit the product";
            #region Image Upload
            //check for file upload
            if (file !=null && file.ContentLength>0)
            {
                //Get the  extension
                string ext = file.ContentType.ToLower();
                //verify extension
                if (ext != "image/jpg" &&
                    ext != "image/jpeg" &&
                    ext != "image/pjpeg" &&
                    ext != "image/gif" &&
                    ext != "image/x-png" &&
                    ext != "image/png")
                {
                    using (SmartDB db = new SmartDB())
                    {
                        ModelState.AddModelError("", "The image was not upload -wrong extension");
                        return View(model);
                    }
                }

                //Set upload Directory path
                var OriginalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

                var pathString1 = Path.Combine(OriginalDirectory.ToString(), "Products\\" + id.ToString());
                var pathString2 = Path.Combine(OriginalDirectory.ToString(), "Products\\" + id.ToString() + "\\Thumbs");

                //Delete file from Directory
                DirectoryInfo di1 = new DirectoryInfo(pathString1);
                DirectoryInfo di2 = new DirectoryInfo(pathString2);

                foreach  (FileInfo files2 in di1.GetFiles())
                {
                    files2.Delete();
                }
                foreach (FileInfo files3 in di2.GetFiles())
                {
                    files3.Delete();
                }

                //save image name
                string imagename = file.FileName;

                using (SmartDB db = new SmartDB())
                {
                    StProductDT dt = db.Product.Find(id);
                    dt.ImageName = imagename;
                    db.SaveChanges();
                }
                //Save the original and thums image
                var path4 = string.Format("{0}\\{1}", pathString1, imagename);
                var path5 = string.Format("{0}\\{1}", pathString2, imagename);

                //save the original path
                file.SaveAs(path4);

                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200);
                img.Save(path5);
            }
            #endregion
            return RedirectToAction("EditProduct");
        }

        //******************Delete Product code*****************************//
        public ActionResult DeleteProduct(int id)
        {
            //Delete product from db
            using (SmartDB db=new SmartDB())
            {
                StProductDT dt = db.Product.Find(id);
                db.Product.Remove(dt);
                db.SaveChanges();
            }

            //Delete Product Folder
            var OriginalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
            string pathstring = Path.Combine(OriginalDirectory.ToString(), "Products\\" + id.ToString());

            if (Directory.Exists(pathstring))
            {
                Directory.Delete(pathstring,true);
            }
             return RedirectToAction("Products");
        }

        //************************SaveGalleryImages code *****************//
        [HttpPost]
        public void SaveGalleryImages(int id)
        {
            //Loop through file
            foreach (string fileName in Request.Files)
            {
                //init the file name
                HttpPostedFileBase file = Request.Files[fileName];

                //check it is not null
                if (file !=null && file.ContentLength >0)
                {
                    //set Directory path
                    var OriginalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

                    var pathString1 = Path.Combine(OriginalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery");
                    var pathString2 = Path.Combine(OriginalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery\\Thumbs");


                    //set the image path
                    var path1 = string.Format("{0}\\{1}", pathString1, file.FileName);
                    var path2 = string.Format("{0}\\{1}", pathString2, file.FileName);

                    //save the original path thumbs
                    file.SaveAs(path1);

                    //create and thums the save
                    WebImage img = new WebImage(file.InputStream);
                    img.Resize(200, 200);
                    img.Save(path2);

                }
            }
        }
        //******************************DeleteImage***************//
        [HttpPost]
        public void ImageDelete(int id,string imageName)
        {
            string fullpath1 = Request.MapPath("/Images/Uploads/Products" + id.ToString() + "/Gallery" + imageName);
            string fullpath2 = Request.MapPath("/Images/Uploads/Products" + id.ToString() + "/Gallery/Thumbs" + imageName);


            if (System.IO.File.Exists(fullpath1))
            {
                System.IO.File.Delete(fullpath1);
            }
            if (System.IO.File.Exists(fullpath2))
            {
                System.IO.File.Delete(fullpath2);
            }
         
        }

    }
}