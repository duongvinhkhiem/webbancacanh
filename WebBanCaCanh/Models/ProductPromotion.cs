using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanCaCanh.Models
{
    public class ProductPromotion
    {
        [Key]
        public int ProductPromotionId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn sản phẩm")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn khuyến mãi")]
        public int PromotionId { get; set; }
        public virtual Promotion Promotion { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }
    }
}
