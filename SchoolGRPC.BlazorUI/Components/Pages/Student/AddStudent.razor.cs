using Microsoft.AspNetCore.Components;
using SchoolGRPC.BlazorUI.ViewModels;
using SchoolGRPC.Shared.Contracts.Data;
using SchoolGRPC.Shared.Contracts.Services;
using SchoolGRPC.Shared.Models;
using System;
using System.Threading.Tasks;

namespace SchoolGRPC.BlazorUI.Components.Pages.Student
{
    public partial class AddStudent
    {
        [Inject]
        public IClassRoomService ClassRoomClient { get; set; } = default!;

        [Inject]
        public IStudentService StudentClient { get; set; } = default!;

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private StudentInputModel studentInput = new StudentInputModel();
        private List<ClassRoomDto>? classRooms;
        private string submitErrorMessage = string.Empty;
        private bool classRoomLoadFailed = false;
        private string classRoomErrorMessage = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var classRoomResponse = await ClassRoomClient.GetAllClassRoomsAsync();
                classRooms = classRoomResponse.Classrooms.ToList();
                classRoomLoadFailed = false;
            }
            catch (Exception ex)
            {
                classRoomLoadFailed = true;
                classRoomErrorMessage = $"Lỗi tải danh sách lớp học: {ex.Message}";
                classRooms = new List<ClassRoomDto>();
            }
        }

        async Task HandleValidSubmit()
        {
            submitErrorMessage = string.Empty;
            if (studentInput.ClassRoomId == 0)
            {
                submitErrorMessage = "Vui lòng chọn một lớp học.";
                return;
            }
            if (studentInput.Birthday == null)
            {
                submitErrorMessage = "Ngày sinh là bắt buộc.";
                return;
            }

            try
            {
                var request = new CreateStudentRequestDto
                {
                    Name = studentInput.Name,
                    Birthday = DateTime.SpecifyKind(studentInput.Birthday.Value, DateTimeKind.Utc),
                    Address = studentInput.Address,
                    ClassroomId = studentInput.ClassRoomId
                };
                await StudentClient.CreateStudentAsync(request);
                NavigationManager.NavigateTo("/students");
            }
            catch (Exception ex)
            {
                submitErrorMessage = $"Lỗi khi thêm sinh viên: {ex.Message}";
            }
        }

        void GoBack() => NavigationManager.NavigateTo("/students");
    }
}
