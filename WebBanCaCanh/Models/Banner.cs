using System;
using System.ComponentModel.DataAnnotations;

namespace WebBanCaCanh.Models
{
    public class Banner
    {
        public int BannerId { get; set; }

        [Required(ErrorMessage = "Nội dung mô tả là bắt buộc")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Nội dung phải có ít nhất 2 và tối đa 255 ký tự")]

        public string Content { get; set; }

        public string ImageUrl { get; set; }
        public bool IsVisible { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }
    }
}
