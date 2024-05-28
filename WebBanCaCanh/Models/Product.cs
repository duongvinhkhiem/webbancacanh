using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanCaCanh.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Tên sản phẩm phải có ít nhất 2 và tối đa 100 ký tự")]

        public string ProductName { get; set; }

        [Required(ErrorMessage = "Giá sản phẩm là bắt buộc")]
        [Range(1000, 10000000, ErrorMessage = "Giá sản phẩm phải từ {1} đến {2}")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }

        public string Description { get; set; }
        [Required(ErrorMessage = "Số lượng tồn kho là bắt buộc")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho phải lớn hơn hoặc bằng 0")]
        public int StockQuantity { get; set; }

        public bool IsVisible { get; set; }
        [Required(ErrorMessage = "Loại sản phẩm là bắt buộc")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
    }
}
