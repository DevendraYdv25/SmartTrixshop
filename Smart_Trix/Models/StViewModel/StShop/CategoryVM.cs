using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Smart_Trix.Models.StData; //Access the StCategoryDT

namespace Smart_Trix.Models.StViewModel.StShop
{
    public class CategoryVM
    {
        public CategoryVM()
        {

        }
        public CategoryVM(StCategoryDT yadav)
        {
            Id = yadav.Id;
            Name = yadav.Name;
            Slug = yadav.Slug;
            Sorting = yadav.Sorting;
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int Sorting { get; set; }
    }
}