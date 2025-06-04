using Microsoft.AspNetCore.Components;
using SchoolGRPC.BlazorUI.ViewModels;
using SchoolGRPC.Shared.Contracts.Data;
using SchoolGRPC.Shared.Contracts.Services;
using SchoolGRPC.Shared.Models;
using System;
using System.Threading.Tasks;

namespace SchoolGRPC.BlazorUI.Components.Pages.ClassRoom
{
    public partial class EditClassRoom : ComponentBase
    {
        [Parameter]
        public int ClassRoomId { get; set; }

        [Inject]
        public IClassRoomService ClassRoomClient { get; set; } = default!;

        [Inject]
        public ITeacherService TeacherClient { get; set; } = default!;

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private ClassRoomInputModel? classRoomInput;
        private List<TeacherDto>? teachers;

        private bool loadFailed = false;
        private string loadErrorMessage = string.Empty;
        private bool teacherLoadFailed = false;
        private string teacherErrorMessage = string.Empty;
        private string submitErrorMessage = string.Empty;


        protected override async Task OnInitializedAsync()
        {
            await LoadTeachers();
            if (!teacherLoadFailed) // Chỉ load classroom nếu load teacher thành công
            {
                await LoadClassRoom();
            }
        }

        private async Task LoadTeachers()
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

        private async Task LoadClassRoom()
        {
            try
            {
                var classRoom = await ClassRoomClient.GetClassRoomByIdAsync(new GetByIdRequestDto { Id = ClassRoomId });
                if (classRoom != null)
                {
                    classRoomInput = new ClassRoomInputModel
                    {
                        Id = classRoom.Id,
                        Name = classRoom.Name,
                        Subject = classRoom.Subject,
                        TeacherId = classRoom.TeacherId 
                    };
                    loadFailed = false;
                }
                else
                {
                    loadFailed = true;
                    loadErrorMessage = "Không tìm thấy lớp học.";
                }
            }
            catch (Exception ex)
            {
                loadFailed = true;
                loadErrorMessage = $"Lỗi tải dữ liệu lớp học: {ex.Message}";
            }
        }

        async Task HandleValidSubmit()
        {
            if (classRoomInput == null) return;
            submitErrorMessage = string.Empty;

            if (classRoomInput.TeacherId == 0)
            {
                submitErrorMessage = "Vui lòng chọn một giáo viên.";
                return;
            }

            try
            {
                var request = new UpdateClassRoomRequestDto
                {
                    Id = classRoomInput.Id,
                    Name = classRoomInput.Name,
                    Subject = classRoomInput.Subject,
                    TeacherId = classRoomInput.TeacherId
                };
                await ClassRoomClient.UpdateClassRoomAsync(request);
                NavigationManager.NavigateTo("/classrooms");
            }
            catch (Exception ex)
            {
                submitErrorMessage = $"Lỗi khi cập nhật: {ex.Message}";
            }
        }

        void GoBack() => NavigationManager.NavigateTo("/classrooms");
    }
}
