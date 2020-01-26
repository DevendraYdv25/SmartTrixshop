using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Smart_Trix.Models.StData
{
    [Table("tblorderDetails")]
    public class StOrderDetailDT
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }


        [ForeignKey("OrderId")]
        public virtual StOrderDT Orders { get; set; }

        [ForeignKey("UserId")]
        public virtual StUserDT Users { get; set; }

        [ForeignKey("ProductId")]
        public virtual StUserDT Products { get; set; }
    }
}