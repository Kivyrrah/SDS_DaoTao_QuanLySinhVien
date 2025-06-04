using Microsoft.AspNetCore.Components;
using SchoolGRPC.Shared.Contracts.Data;
using SchoolGRPC.Shared.Contracts.Services;
using System;
using System.Threading.Tasks;
using SchoolGRPC.Shared.Models;

namespace SchoolGRPC.BlazorUI.Components.Pages.Teacher
{
    public partial class AddTeacher : ComponentBase
    {
        [Parameter]
        public int TeacherId { get; set; }

        [Inject]
        public ITeacherService TeacherClient { get; set; } = default!;

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private TeacherInputModel teacherInput = new TeacherInputModel();
        private string errorMessage = string.Empty;

        async Task HandleValidSubmit()
        {
            errorMessage = string.Empty;
            try
            {
                if (teacherInput.Birthday == null)
                {
                    errorMessage = "Ngày sinh là bắt buộc.";
                    return;
                }

                var request = new CreateTeacherRequestDto
                {
                    Name = teacherInput.Name,
                    Birthday = teacherInput.Birthday.Value 
                };
                await TeacherClient.CreateTeacherAsync(request);
                NavigationManager.NavigateTo("/teachers");
            }
            catch (Exception ex)
            {
                errorMessage = $"Lỗi khi thêm: {ex.Message}";
            }
        }

        void GoBack() => NavigationManager.NavigateTo("/teachers");
    }
}
