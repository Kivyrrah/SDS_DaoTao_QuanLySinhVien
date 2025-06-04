// SchoolGRPC.BlazorUI/ViewModels/ClassRoomInputModel.cs
using System.ComponentModel.DataAnnotations;

namespace SchoolGRPC.BlazorUI.ViewModels
{
    public class ClassRoomInputModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên lớp học không được để trống")]
        [StringLength(50, ErrorMessage = "Tên lớp học không quá 50 ký tự")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Môn học không được để trống")]
        [StringLength(100, ErrorMessage = "Môn học không quá 100 ký tự")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn giáo viên")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn giáo viên hợp lệ")]
        public int TeacherId { get; set; }
    }
}