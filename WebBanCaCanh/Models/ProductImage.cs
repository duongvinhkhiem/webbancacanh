using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanCaCanh.Models
{
    public class ProductImage
    {
        public int ProductImageId { get; set; }

        [Required(ErrorMessage = "Đường dẫn hình ảnh là bắt buộc")]
        public string ImageUrl { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}