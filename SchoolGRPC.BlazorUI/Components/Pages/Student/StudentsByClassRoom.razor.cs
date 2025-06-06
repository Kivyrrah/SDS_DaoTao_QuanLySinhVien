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
        public IStudentService StudentClient { get; set; } = default!;

        [Inject]
        public IClassRoomService ClassRoomClient { get; set; } = default!;

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private List<StudentDto>? studentsList;
        private ClassRoomDto? currentClassRoom;

        private bool isLoadingStudents = true;
        private bool studentLoadFailed = false;
        private string studentErrorMessage = string.Empty;

        private bool isLoadingClassRoomInfo = true;
        private bool classRoomLoadFailed = false;
        private string classRoomErrorMessage = string.Empty;

        protected override async Task OnParametersSetAsync()
        {
            await LoadClassRoomInformation();
            if (currentClassRoom != null && !classRoomLoadFailed)
            {
                await LoadStudentsForClassRoom();
            }
            else if (currentClassRoom == null && !classRoomLoadFailed)
            {
                isLoadingStudents = false; 
            }
        }

        private async Task LoadClassRoomInformation()
        {
            isLoadingClassRoomInfo = true;
            classRoomLoadFailed = false;
            currentClassRoom = null; 
            try
            {
                var request = new GetByIdRequestDto { Id = ClassRoomId };
                currentClassRoom = await ClassRoomClient.GetClassRoomByIdAsync(request);
                if (currentClassRoom == null)
                {
                    classRoomErrorMessage = $"Không tìm thấy thông tin cho lớp học có ID: {ClassRoomId}.";
                    _logger.LogWarning(classRoomErrorMessage);
                }
            }
            catch (Exception ex)
            {
                classRoomLoadFailed = true;
                classRoomErrorMessage = $"Lỗi tải thông tin lớp học: {ex.Message}";
                _logger.LogError(ex, "Lỗi khi LoadClassRoomInformation cho ClassRoomID: {ClassRoomId}", ClassRoomId);
            }
            finally
            {
                isLoadingClassRoomInfo = false;
            }
        }

        private async Task LoadStudentsForClassRoom()
        {
            isLoadingStudents = true;
            studentLoadFailed = false;
            studentsList = null; 
            try
            {
                var request = new GetStudentsByClassRoomRequestDto { ClassRoomId = this.ClassRoomId };
                var response = await StudentClient.GetStudentsByClassRoomAsync(request);

                if (response != null && response.Students != null)
                {
                    studentsList = response.Students;
                }
                else
                {
                    studentsList = new List<StudentDto>(); 
                }
            }
            catch (Exception ex)
            {
                studentLoadFailed = true;
                studentErrorMessage = $"Lỗi tải danh sách học sinh: {ex.Message}";
                studentsList = new List<StudentDto>(); 
                _logger.LogError(ex, "Lỗi khi LoadStudentsForClassRoom cho ClassRoomID: {ClassRoomId}", ClassRoomId);
            }
            finally
            {
                isLoadingStudents = false;
            }
        }

        void GoToClassRoomList() => NavigationManager.NavigateTo("/classrooms");

        [Inject] ILogger<StudentsByClassRoom> _logger { get; set; } = default!;
    }
}
