using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SchoolGRPC.BlazorUI.ViewModels;
using SchoolGRPC.Shared.Contracts.Data;
using SchoolGRPC.Shared.Contracts.Services;
using SchoolGRPC.Shared.Models;
using System;
using System.Threading.Tasks;

namespace SchoolGRPC.BlazorUI.Components.Pages.Student
{
    public partial class StudentsList
    {
        [Inject]
        public IStudentService StudentClient { get; set; } = default!;

        [Inject] private IJSRuntime JSRuntime { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private List<StudentDto>? students;
        private bool loadFailed = false;
        private string errorMessage = string.Empty;

        private int? studentIdToSearch;
        private string searchErrorMessage = string.Empty;

        private string currentSortColumn = string.Empty;
        private bool sortAscending = true;

        protected override async Task OnInitializedAsync()
        {
            currentSortColumn = string.Empty;
            await LoadStudentsList();
        }

        private async Task LoadStudentsList()
        {
            try
            {
                var response = await StudentClient.GetAllStudentsAsync();
                students = response.Students.ToList();
                loadFailed = false;
            }
            catch (Exception ex)
            {
                loadFailed = true;
                errorMessage = ex.Message;
                students = new List<StudentDto>();
            }
        }

        void GoToAddStudent() => NavigationManager.NavigateTo("/students/add");
        void GoToEditStudent(int id) => NavigationManager.NavigateTo($"/students/edit/{id}");
        void ViewStudentDetail(int id) => NavigationManager.NavigateTo($"/students/view/{id}");

        async Task DeleteStudent(int id)
        {
            bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Bạn có chắc muốn xóa sinh viên này?");
            if (confirmed)
            {
                try
                {
                    await StudentClient.DeleteStudentAsync(new GetByIdRequestDto { Id = id });
                    await LoadStudentsList();
                    if (!string.IsNullOrEmpty(currentSortColumn))
                    {
                        ApplySort();
                    }
                }
                catch (Exception ex)
                {
                    errorMessage = $"Lỗi khi xóa: {ex.Message}";
                }
                StateHasChanged();
            }
        }

        void SearchStudentById()
        {
            searchErrorMessage = string.Empty;
            if (studentIdToSearch.HasValue && studentIdToSearch.Value > 0)
            {
                NavigationManager.NavigateTo($"/students/view/{studentIdToSearch.Value}");
            }
            else
            {
                searchErrorMessage = "Vui lòng nhập ID sinh viên hợp lệ (số dương).";
            }
        }

        void SortStudentsByName()
        {
            if (students == null || !students.Any()) return;

            if (currentSortColumn == "Name")
            {
                sortAscending = !sortAscending;
            }
            else
            {
                currentSortColumn = "Name";
                sortAscending = true;
            }
            ApplySort();
        }

        void ApplySort()
        {
            if (students == null || !students.Any() || string.IsNullOrEmpty(currentSortColumn)) return;

            if (currentSortColumn == "Name")
            {
                if (sortAscending)
                {
                    students = students.OrderBy(s => s.Name, StringComparer.OrdinalIgnoreCase).ToList();
                }
                else
                {
                    students = students.OrderByDescending(s => s.Name, StringComparer.OrdinalIgnoreCase).ToList();
                }
            }

            StateHasChanged();
        }
    }
}
