using Microsoft.AspNetCore.Components;
using SchoolGRPC.BlazorUI.ViewModels;
using SchoolGRPC.Shared.Contracts.Data;
using SchoolGRPC.Shared.Contracts.Services;
using SchoolGRPC.Shared.Models;
using System;
using System.Threading.Tasks;

namespace SchoolGRPC.BlazorUI.Components.Pages.Student
{
    public partial class StudentsByClassRoom
    {
        [Parameter]
        public int ClassRoomId { get; set; }

        [Inject]
        public IClassRoomService ClassRoomClient { get; set; } = default!;

        [Inject]
        public IStudentService StudentClient { get; set; } = default!;

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private List<StudentDto>? students;
        private ClassRoomDto? currentClassRoom;
        private string currentClassRoomName = string.Empty;

        private bool loadFailed = false;
        private string errorMessage = string.Empty;
        private bool classRoomLoadFailed = false;
        private string classRoomErrorMessage = string.Empty;


        protected override async Task OnInitializedAsync()
        {
            await LoadClassRoomInfo();
            if (!classRoomLoadFailed)
            {
                // await LoadStudents();
            }
        }

        private async Task LoadClassRoomInfo()
        {
            try
            {
                currentClassRoom = await ClassRoomClient.GetClassRoomByIdAsync(new GetByIdRequestDto { Id = ClassRoomId });
                if (currentClassRoom != null)
                {
                    currentClassRoomName = $"{currentClassRoom.Name} ({currentClassRoom.Subject})";
                }
                classRoomLoadFailed = false;
            }
            catch (Exception ex)
            {
                classRoomLoadFailed = true;
                classRoomErrorMessage = $"Lỗi tải thông tin lớp học: {ex.Message}";
            }
        }

        /* private async Task LoadStudents()
        {
            try
            {
                var response = await StudentClient.GetStudentsByClassRoomAsync(new GetStudentsByClassRoomRequestDto { ClassroomId = ClassRoomId });
                students = response.Students.ToList();
                loadFailed = false;
            }
            catch (Exception ex)
            {
                loadFailed = true;
                errorMessage = $"Lỗi tải danh sách sinh viên: {ex.Message}";
                students = new List<StudentDto>();
            }
        } */

        void GoToClassRoomList() => NavigationManager.NavigateTo("/classrooms");
    }
}
