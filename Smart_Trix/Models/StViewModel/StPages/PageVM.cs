using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Smart_Trix.Models.StData; //StpageDT

namespace Smart_Trix.Models.StViewModel.StPages
{
    public class PageVM
    {
        public PageVM()
        {

        }
        public PageVM(StPageDT yadav)
        {
            Id = yadav.Id;
            Title = yadav.Title;
            Slug = yadav.Slug;
            Body = yadav.Body;
            Sorting = yadav.Sorting;
            HasSidebar = yadav.HasSidebar;
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }
        public string Slug { get; set; }
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 3)]
        public string Body { get; set; }
        public int Sorting { get; set; }
        public bool HasSidebar { get; set; }
    }
}