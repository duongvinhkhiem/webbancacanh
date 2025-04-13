using System;
using System.ComponentModel.DataAnnotations;

namespace WebBanCaCanh.Models
{
    public class News
    {
        public int NewsId { get; set; }

        [Required(ErrorMessage = "Tiêu đề là bắt buộc")]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Nội dung là bắt buộc")]
        [Display(Name = "Nội dung")]
        public string Content { get; set; }

        [Display(Name = "Hiển thị")]
        public bool IsVisible { get; set; }

        [Display(Name = "Đường dẫn ảnh")]
        public string ImageUrl { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Ngày tạo")]
        public DateTime CreatedAt { get; set; }
    }
}
