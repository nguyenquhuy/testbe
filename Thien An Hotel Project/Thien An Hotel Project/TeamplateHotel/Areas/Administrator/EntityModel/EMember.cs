using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TeamplateHotel.Areas.Administrator.EntityModel
{
    public class EMember
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DisplayName("Thẻ mô tả")]
        [MaxLength(250, ErrorMessage = "Tối đa 250 ký tự")]
        public string Description { get; set; }
        [DisplayName("Điện thoại")]
        [MaxLength(10, ErrorMessage = "Tối đa 10 ký tự")]
        public string PhoneNumber { get; set; }
        [DisplayName("Địa chỉ email")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ email")]
        [EmailAddress(ErrorMessage = "vui lòng nhập đúng địa chỉ email")]
        public string Email { get; set; }
        [DisplayName("Ảnh đại diện")]
        [MaxLength(300, ErrorMessage = "Tối đa 300 ký tự")]
        [Required(ErrorMessage = "Vui lòng chọn ảnh đại diện")]
        public string Image {  get; set; }
    }
}