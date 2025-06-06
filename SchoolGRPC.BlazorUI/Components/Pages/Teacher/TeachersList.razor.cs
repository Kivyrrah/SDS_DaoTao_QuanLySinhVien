using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SchoolGRPC.Shared.Contracts.Data;
using SchoolGRPC.Shared.Contracts.Services;
using SchoolGRPC.Shared.Models;
using System;
using System.Threading.Tasks;

namespace SchoolGRPC.BlazorUI.Components.Pages.Teacher
{
    public partial class TeachersList : ComponentBase
    {
        [Parameter]
        public int TeacherId { get; set; }

        [Inject]
        public ITeacherService TeacherClient { get; set; } = default!;

        [Inject] private IJSRuntime JSRuntime { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private List<TeacherDto>? teachers; 
        private bool loadFailed = false;
        private string errorMessage = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var response = await TeacherClient.GetAllTeachersAsync();
                teachers = response?.Teachers ?? new List<TeacherDto>();
                loadFailed = false;
            }
            catch (Exception ex)
            {
                loadFailed = true;
                errorMessage = ex.Message; 
                teachers = new List<TeacherDto>();
            }
        }

        void GoToAddTeacher() => NavigationManager.NavigateTo("/teachers/add");
        void GoToEditTeacher(int id) => NavigationManager.NavigateTo($"/teachers/edit/{id}");

        async Task DeleteTeacher(int id)
        {
            bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Bạn có chắc muốn xóa giáo viên này?");
            if (confirmed)
            {
                try
                {
                    var response = await TeacherClient.DeleteTeacherAsync(new GetByIdRequestDto { Id = id });
                    if (response.Success)
                    {
                        await OnInitializedAsync();
                    }
                    else
                    {
                        errorMessage = response.Message;
                    }
                }
                catch (Exception ex)
                {
                    errorMessage = $"Lỗi khi xóa: {ex.Message}";
                }
                StateHasChanged();
            }
        }
    }
}