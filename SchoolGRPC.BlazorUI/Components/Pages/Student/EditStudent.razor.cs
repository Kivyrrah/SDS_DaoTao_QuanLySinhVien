using Microsoft.AspNetCore.Components;
using SchoolGRPC.BlazorUI.ViewModels;
using SchoolGRPC.Shared.Contracts.Data;
using SchoolGRPC.Shared.Contracts.Services;
using SchoolGRPC.Shared.Models;
using System;
using System.Threading.Tasks;

namespace SchoolGRPC.BlazorUI.Components.Pages.Student
{
    public partial class EditStudent
    {
        [Parameter]
        public int StudentId { get; set; }

        [Inject]
        public IClassRoomService ClassRoomClient { get; set; } = default!;

        [Inject]
        public IStudentService StudentClient { get; set; } = default!;

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private StudentInputModel? studentInput;
        private List<ClassRoomDto>? classRooms;

        private bool loadFailed = false;
        private string loadErrorMessage = string.Empty;
        private bool classRoomLoadFailed = false;
        private string classRoomErrorMessage = string.Empty;
        private string submitErrorMessage = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            await LoadClassRooms();
            if (!classRoomLoadFailed)
            {
                await LoadStudent();
            }
        }

        private async Task LoadClassRooms()
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

        private async Task LoadStudent()
        {
            try
            {
                var student = await StudentClient.GetStudentByIdAsync(new GetByIdRequestDto { Id = StudentId });
                if (student != null)
                {
                    studentInput = new StudentInputModel
                    {
                        Id = student.Id,
                        Name = student.Name,
                        Birthday = student.Birthday,
                        Address = student.Address,
                        ClassRoomId = student.ClassroomId
                    };
                    loadFailed = false;
                }
                else
                {
                    loadFailed = true;
                    loadErrorMessage = "Không tìm thấy sinh viên.";
                }
            }
            catch (Exception ex)
            {
                loadFailed = true;
                loadErrorMessage = $"Lỗi tải dữ liệu sinh viên: {ex.Message}";
            }
        }

        async Task HandleValidSubmit()
        {
            if (studentInput == null) return;
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
                var request = new UpdateStudentRequestDto
                {
                    Id = studentInput.Id,
                    Name = studentInput.Name,
                    Birthday = DateTime.SpecifyKind(studentInput.Birthday.Value, DateTimeKind.Utc),
                    Address = studentInput.Address,
                    ClassroomId = studentInput.ClassRoomId
                };
                await StudentClient.UpdateStudentAsync(request);
                NavigationManager.NavigateTo("/students");
            }
            catch (Exception ex)
            {
                submitErrorMessage = $"Lỗi khi cập nhật: {ex.Message}";
            }
        }

        void GoBack() => NavigationManager.NavigateTo("/students");
    }
}
