using System;
using System.ComponentModel.DataAnnotations;

namespace WebBanCaCanh.Models
{
    public class Promotion
    {
        public int PromotionId { get; set; }

        [Required(ErrorMessage = "Tên khuyến mãi là bắt buộc")]
        public string PromotionName { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }
        [Required(ErrorMessage = "Tỷ lệ giảm giá là bắt buộc")]
        [Range(0.01, 100, ErrorMessage = "Tỷ lệ giảm giá phải từ {1} đến {2}")]
        public decimal DiscountPercentage { get; set; }
    }
}
