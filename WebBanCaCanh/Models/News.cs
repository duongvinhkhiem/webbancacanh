using System;
using System.ComponentModel.DataAnnotations;

namespace WebBanCaCanh.Models
{
    public class News
    {
        public int NewsId { get; set; }

        [Required(ErrorMessage = "Tiêu đề là bắt buộc")]
        public string Title { get; set; }
    

        [Required(ErrorMessage = "Nội dung là bắt buộc")]
        public string Content { get; set; }
        public bool IsVisible { get; set; }
        public string ImageUrl { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }
    }
}
