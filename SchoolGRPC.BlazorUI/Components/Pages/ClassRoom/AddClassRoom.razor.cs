using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SchoolGRPC.BlazorUI.ViewModels;
using SchoolGRPC.Shared.Contracts.Data;
using SchoolGRPC.Shared.Contracts.Services;
using SchoolGRPC.Shared.Models;
using System;
using System.Threading.Tasks;

namespace SchoolGRPC.BlazorUI.Components.Pages.ClassRoom
{
    public partial class AddClassRoom : ComponentBase
    {
        [Inject]
        public ITeacherService TeacherClient { get; set; } = default!;

        [Inject]
        public IClassRoomService ClassRoomClient { get; set; } = default!;

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private ClassRoomInputModel classRoomInput = new ClassRoomInputModel();
        private List<TeacherDto>? teachers;
        private string submitErrorMessage = string.Empty;
        private bool teacherLoadFailed = false;
        private string teacherErrorMessage = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var teacherResponse = await TeacherClient.GetAllTeachersAsync();
                teachers = teacherResponse.Teachers.ToList();
                teacherLoadFailed = false;
            }
            catch (Exception ex)
            {
                teacherLoadFailed = true;
                teacherErrorMessage = $"Lỗi tải danh sách giáo viên: {ex.Message}";
                teachers = new List<TeacherDto>();
            }
        }

        async Task HandleValidSubmit()
        {
            submitErrorMessage = string.Empty;
            if (classRoomInput.TeacherId == 0)
            {
                submitErrorMessage = "Vui lòng chọn một giáo viên.";
                return;
            }

            try
            {
                var request = new CreateClassRoomRequestDto
                {
                    Name = classRoomInput.Name,
                    Subject = classRoomInput.Subject,
                    TeacherId = classRoomInput.TeacherId
                };
                await ClassRoomClient.CreateClassRoomAsync(request);
                NavigationManager.NavigateTo("/classrooms");
            }
            catch (Exception ex)
            {
                submitErrorMessage = $"Lỗi khi thêm: {ex.Message}";
            }
        }

        void GoBack() => NavigationManager.NavigateTo("/classrooms");
    }
}
