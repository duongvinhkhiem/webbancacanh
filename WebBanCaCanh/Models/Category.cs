using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanCaCanh.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Tên loại sản phẩm là bắt buộc")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Tên loại sản phẩm phải có ít nhất 2 và tối đa 50 ký tự")]
        [RegularExpression(@"^[\p{L}\s]*$", ErrorMessage = "Tên loại sản phẩm chỉ được chứa các ký tự chữ cái và khoảng trắng")]
        public string CategoryName { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}