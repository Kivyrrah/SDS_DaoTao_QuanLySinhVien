using Microsoft.AspNetCore.Components;
using SchoolGRPC.BlazorUI.ViewModels;
using SchoolGRPC.Shared.Contracts.Data;
using SchoolGRPC.Shared.Contracts.Services;
using SchoolGRPC.Shared.Models;
using System;
using System.Threading.Tasks;

namespace SchoolGRPC.BlazorUI.Components.Pages.Student
{
    public partial class StudentDetailView
    {
        [Parameter]
        public int StudentId { get; set; }

        [Inject]
        public IStudentService StudentClient { get; set; } = default!;

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private StudentDto? foundStudent;
        private bool isLoading = true;
        private bool loadFailed = false;
        private string errorMessage = string.Empty;

        protected override async Task OnParametersSetAsync()
        {
            isLoading = true;
            loadFailed = false;
            errorMessage = string.Empty;
            foundStudent = null;

            try
            {
                var request = new GetByIdRequestDto { Id = StudentId };
                foundStudent = await StudentClient.GetStudentByIdAsync(request);
            }
            catch (Grpc.Core.RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
            {
                errorMessage = $"Không tìm thấy sinh viên với ID: {StudentId}.";
                loadFailed = true;
            }
            catch (Exception ex)
            {
                errorMessage = $"Lỗi khi tải thông tin sinh viên: {ex.Message}";
                loadFailed = true;
            }
            finally
            {
                isLoading = false;
            }
        }

        void GoBackToList()
        {
            NavigationManager.NavigateTo("/students");
        }

        void GoToEditStudent()
        {
            if (foundStudent != null)
            {
                NavigationManager.NavigateTo($"/students/edit/{foundStudent.Id}");
            }
        }
    }
}
