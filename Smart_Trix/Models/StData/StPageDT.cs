﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Smart_Trix.Models.StData
{
    [Table("tblpages")]
    public class StPageDT
    {
            [Key]
            public int Id { get; set; }
            public string Title { get; set; }
            public string Slug { get; set; }
            public string Body { get; set; }
            public int Sorting { get; set; }
            public bool HasSidebar { get; set; }
    }
}