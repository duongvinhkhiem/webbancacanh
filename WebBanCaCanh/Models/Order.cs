using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebBanCaCanh.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Trạng thái đơn hàng là bắt buộc")]
        public string OrderStatus { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập họ và tên.")]
        [Display(Name = "Họ và tên")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        [Display(Name = "Số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập thành phố.")]
        [Display(Name = "Thành phố")]
        public string City { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập quận/huyện.")]
        [Display(Name = "Quận/Huyện")]
        public string District { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ.")]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Ghi chú")]
        public string Note { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
