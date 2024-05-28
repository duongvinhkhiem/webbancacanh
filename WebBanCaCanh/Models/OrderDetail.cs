using System;
using System.ComponentModel.DataAnnotations;

namespace WebBanCaCanh.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }

        [Required(ErrorMessage = "Số lượng là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Đơn giá là bắt buộc")]
        [Range(1000, 1000000, ErrorMessage = "Đơn giá phải từ {1} đến {2}")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "Mã đơn hàng là bắt buộc")]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        [Required(ErrorMessage = "Mã sản phẩm là bắt buộc")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
