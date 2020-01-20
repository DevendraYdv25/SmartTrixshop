using Smart_Trix.Models.StData;
using Smart_Trix.Models.StViewModel.StCart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Smart_Trix.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            //init the cart list
            var cart = Session["cart"] as List<CartVM> ?? new List<CartVM>();

            //check if cart is empty
            if (cart.Count==0 ||Session["cart"]==null)
            {
                ViewBag.message = "Your Cart is Empty";
                return View();
            }
            //calculate the view and save the total
            decimal total = 0m;

            foreach (var item in cart)
            {
                total += item.Total;
            }
            ViewBag.GrandTotal = total;

            //return view with list
            return View(cart);

        }
        //make partial method
        public ActionResult CartPartial()
        {
            //init the cartVM
            CartVM model = new CartVM();

            //init the quaninty
            int qty = 0;

            //init price
            decimal price = 0m;

            //Check for cart session
            if (Session["cart"] !=null)
            {
                //get the total quantity and price
                var list = (List<CartVM>)Session["cart"];

                foreach (var item in list)
                {
                    qty += item.Quantity;
                    price += item.Quantity * item.Price;
                }
                model.Quantity = qty;
                model.Price = price;
            }
            else
            {
                //set the quantity and price 0
                model.Quantity = 0;
                model.Price = 0m;
            }
            //return partial view with model
            return PartialView(model);
        }

        //****************AddToCartPartial *********************//
        public ActionResult AddToCartPartial(int id)
        {
            List<CartVM> cart = Session["cart"] as List<CartVM> ?? new List<CartVM>();

            //init thecartvm
            CartVM model = new CartVM();

            using (SmartDB db=new SmartDB())
            {
                //get the product
                StProductDT product = db.Product.Find(id);

                //check if product is already in cart
                var productInCart = cart.FirstOrDefault(a => a.ProductId == id);

                //if not ,add new product
                if (productInCart==null)
                {
                    cart.Add(new CartVM()
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Quantity=1,
                        Price=product.Price,
                        Image=product.ImageName

                    });
                }
                else
                {
                    //if it is increament
                    productInCart.Quantity++;
                }
            }
            //get the total and price and add to model
            int qty = 0;
            decimal price = 0m;

            foreach (var item in cart)
            {
                qty += item.Quantity;
                price += item.Quantity * item.Price;
            }
            model.Quantity = qty;
            model.Price = price;

            //save the cart to the session
            Session["cart"] = cart;

            //return thepartilview with model
            return PartialView(model);
      


        }

        //****************************IncrementProduct code**********************//
        public JsonResult IncrementProduct(int productid)
        {
            //init cart list
            List<CartVM> cart = Session["cart"] as List<CartVM>;

            using (SmartDB db = new SmartDB())
            {
                //Get the vcartvm from list
                CartVM model = cart.FirstOrDefault(a => a.ProductId == productid);

                //Increment the qty
                model.Quantity++;

                //store the need data
                var result = new { qty = model.Quantity, price = model.Price };

                //Return json with data
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            
        }
        //*********************DecrementProduct code*******************//
        public JsonResult DecrementProduct(int productid)
        {
            //init cart
            List<CartVM> cart = Session["cart"] as List<CartVM>;

            using (SmartDB db=new SmartDB())
            {
                //get the model from list
                CartVM model = cart.FirstOrDefault(a => a.ProductId == productid);

                //Decrement the qty
                if (model.Quantity > 1)
                {
                    model.Quantity--;
                }
                else
                {
                    model.Quantity = 0;
                    cart.Remove(model);
                }
                //store the data neede
                var result = new { qty = model.Quantity, price = model.Price };

                //Return json
                return Json(result, JsonRequestBehavior.AllowGet);
            }


        }
        //*************Removeroduct*****************//
        public void Removeroduct(int productid)
        {
            //init cart
            List<CartVM> cart = Session["cart"] as List<CartVM>;
          

            using (SmartDB db = new SmartDB())
            {

                //get themodelfrom list
                CartVM model = cart.FirstOrDefault(a => a.ProductId == productid);

                //remove from the list
                cart.Remove(model);
            }
        }
    }
}