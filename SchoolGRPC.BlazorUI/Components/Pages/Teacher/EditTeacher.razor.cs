using Microsoft.AspNetCore.Components;
using SchoolGRPC.Shared.Contracts.Data;
using SchoolGRPC.Shared.Contracts.Services;
using System;
using System.Threading.Tasks;
using SchoolGRPC.Shared.Models;

namespace SchoolGRPC.BlazorUI.Components.Pages.Teacher 
{
    public partial class EditTeacher : ComponentBase
    {
        [Parameter]
        public int TeacherId { get; set; }

        [Inject]
        public ITeacherService TeacherClient { get; set; } = default!;

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private TeacherInputModel? teacherInput;
        private string errorMessage = string.Empty;
        private string errorMessageOnSubmit = string.Empty;
        private bool loadFailed = false;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var teacher = await TeacherClient.GetTeacherByIdAsync(new GetByIdRequestDto { Id = TeacherId });
                if (teacher != null)
                {
                    teacherInput = new TeacherInputModel
                    {
                        Id = teacher.Id,
                        Name = teacher.Name,
                        Birthday = teacher.Birthday
                    };
                }
                else
                {
                    loadFailed = true;
                    errorMessage = "Không tìm thấy giáo viên.";
                }
            }
            catch (Exception ex)
            {
                loadFailed = true;
                errorMessage = $"Lỗi tải dữ liệu: {ex.Message}";
            }
        }

        private async Task HandleValidSubmit()
        {
            if (teacherInput == null) return;
            errorMessageOnSubmit = string.Empty;

            try
            {
                if (teacherInput.Birthday == null)
                {
                    errorMessageOnSubmit = "Ngày sinh là bắt buộc.";
                    return;
                }
                var request = new UpdateTeacherRequestDto
                {
                    Id = teacherInput.Id,
                    Name = teacherInput.Name,
                    Birthday = DateTime.SpecifyKind(teacherInput.Birthday.Value, DateTimeKind.Utc)
                };
                await TeacherClient.UpdateTeacherAsync(request);
                NavigationManager.NavigateTo("/teachers");
            }
            catch (Exception ex)
            {
                errorMessageOnSubmit = $"Lỗi khi cập nhật: {ex.Message}";
            }
        }

        private void GoBack() => NavigationManager.NavigateTo("/teachers");
    }

    public class TeacherInputModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? Birthday { get; set; }
    }
}