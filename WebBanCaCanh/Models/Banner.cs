using System;
using System.ComponentModel.DataAnnotations;

namespace WebBanCaCanh.Models
{
    public class Banner
    {
        public int BannerId { get; set; }

        [Required(ErrorMessage = "Nội dung mô tả là bắt buộc")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Nội dung phải có ít nhất 2 và tối đa 255 ký tự")]
        [Display(Name = "Nội dung mô tả")]
        public string Content { get; set; }

        [Display(Name = "Đường dẫn ảnh")]
        public string ImageUrl { get; set; }

        [Display(Name = "Hiển thị")]
        public bool IsVisible { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Ngày bắt đầu")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Ngày kết thúc")]
        public DateTime EndDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Ngày tạo")]
        public DateTime CreatedAt { get; set; }
    }
}
