// SchoolGRPC.BlazorUI/ViewModels/TeacherInputModel.cs
using System.ComponentModel.DataAnnotations;

namespace SchoolGRPC.BlazorUI.ViewModels
{
    public class TeacherInputModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên giáo viên không được để trống")]
        [StringLength(50, ErrorMessage = "Tên giáo viên không quá 50 ký tự")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ngày sinh không được để trống")]
        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; } = DateTime.Today.AddYears(-25); 
    }
}