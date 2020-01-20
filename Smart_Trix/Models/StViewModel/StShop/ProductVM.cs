using Smart_Trix.Models.StData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;//for IEnumerable SelectListItem

namespace Smart_Trix.Models.StViewModel.StShop
{
    public class ProductVM
    {
        public ProductVM()
        {

        }
        public ProductVM(StProductDT yadav)
        {
            Id = yadav.Id;
            Name = yadav.Name;
            Slug = yadav.Slug;
            Description = yadav.Description;
            Price = yadav.Price;
            CategoryName = yadav.CategoryName;
            CategoryId = yadav.CategoryId;
            ImageName = yadav.ImageName;
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Slug { get; set; }
        [Required]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public string ImageName { get; set; }


        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<string>  GalleryImages { get; set; }
    }
}