// SchoolGRPC.BlazorUI/ViewModels/StudentInputModel.cs
using System.ComponentModel.DataAnnotations;

namespace SchoolGRPC.BlazorUI.ViewModels
{
    public class StudentInputModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên học sinh không được để trống")]
        [StringLength(50, ErrorMessage = "Tên học sinh không quá 50 ký tự")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ngày sinh không được để trống")]
        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; } = DateTime.Today.AddYears(-10);

        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        [StringLength(100, ErrorMessage = "Địa chỉ không quá 100 ký tự")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn lớp học")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn lớp học hợp lệ")]
        public int ClassRoomId { get; set; }
    }
}