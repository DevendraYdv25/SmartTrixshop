﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Smart_Trix.Models.StData
{
    [Table("tblroleuser")]
    public class StUselRoleDT
    {
        [Key,Column(Order=0)]
        public int UserId { get; set; }
        [Key,Column(Order =1)]
        public int RoleId { get; set; }

        [ForeignKey("UserId")]
        public virtual StUserDT Users { get; set; }

        [ForeignKey("RoleId")]
        public virtual StRoleDT Roles{ get; set; }
    }
}