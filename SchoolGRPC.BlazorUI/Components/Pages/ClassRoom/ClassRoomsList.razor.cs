using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SchoolGRPC.Shared.Contracts.Data;
using SchoolGRPC.Shared.Contracts.Services;
using SchoolGRPC.Shared.Models;
using System;
using System.Threading.Tasks;

namespace SchoolGRPC.BlazorUI.Components.Pages.ClassRoom
{
    public partial class ClassRoomsList : ComponentBase
    {
        [Inject]
        public IClassRoomService ClassRoomClient { get; set; } = default!;

        [Inject] private IJSRuntime JSRuntime { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private List<ClassRoomDto>? classRooms;
        private bool loadFailed = false;
        private string errorMessage = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var response = await ClassRoomClient.GetAllClassRoomsAsync();
                classRooms = response.Classrooms.ToList();
                loadFailed = false;
            }
            catch (Exception ex)
            {
                loadFailed = true;
                errorMessage = ex.Message;
                classRooms = new List<ClassRoomDto>();
            }
        }

        void GoToAddClassRoom() => NavigationManager.NavigateTo("/classrooms/add");
        void GoToEditClassRoom(int id) => NavigationManager.NavigateTo($"/classrooms/edit/{id}");
        //void ViewStudentsInClass(int classRoomId) => NavigationManager.NavigateTo($"/students/byclassroom/{classRoomId}");


        async Task DeleteClassRoom(int id)
        {
            bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Bạn có chắc muốn xóa lớp học này? Việc này có thể ảnh hưởng đến các sinh viên trong lớp.");
            if (confirmed)
            {
                try
                {
                    await ClassRoomClient.DeleteClassRoomAsync(new GetByIdRequestDto { Id = id });
                    await OnInitializedAsync(); 
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
