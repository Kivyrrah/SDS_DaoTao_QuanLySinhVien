using AntDesign.Charts;
using Microsoft.AspNetCore.Components;
using SchoolGRPC.Shared.Contracts.Data;
using SchoolGRPC.Shared.Contracts.Services;
using SchoolGRPC.Shared.Models;
using System;
using System.Threading.Tasks;

namespace SchoolGRPC.BlazorUI.Components.Pages.Charts
{
    public partial class SchoolDashboard : ComponentBase
    {
        [Parameter]
        public int TeacherId { get; set; }

        [Inject]
        public ITeacherService TeacherClient { get; set; } = default!;

        [Inject]
        public IStatisticsService StatisticsClient { get; set; } = default!;

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        // SỬA: Bỏ khai báo teachersList bị trùng và sai kiểu ở dòng ~40
        // Danh sách giáo viên cho dropdown (đã được sửa ở dưới)
        private List<TeacherDto> teachersList = new List<TeacherDto>();
        private int _selectedTeacherId;

        // SỬA: Kiểu dữ liệu cho chartData
        private List<ChartDataItemDto> chartData = new List<ChartDataItemDto>();
        private BarConfig barChartConfig = new BarConfig();

        private bool isLoadingTeachers = true;
        private bool isLoadingChart = false;
        private string pageErrorMessage = string.Empty;
        private string chartErrorMessage = string.Empty;

        // Khai báo teachersList ở đây là đúng và duy nhất
        // private List<TeacherDto> teachersList = new List<TeacherDto>(); // Đã có ở trên, không cần dòng này nữa


        protected override async Task OnInitializedAsync()
        {
            isLoadingTeachers = true;
            try
            {
                var response = await TeacherClient.GetAllTeachersAsync(); // Không cần new Empty()
                if (response != null && response.Teachers != null)
                {
                    teachersList = response.Teachers.ToList();
                }
                else
                {
                    teachersList = new List<TeacherDto>(); // Khởi tạo rỗng nếu response null
                }
            }
            catch (Exception ex)
            {
                pageErrorMessage = $"Lỗi tải danh sách giáo viên: {ex.Message}";
                teachersList = new List<TeacherDto>(); // Khởi tạo rỗng khi có lỗi
            }
            finally
            {
                isLoadingTeachers = false;
            }
            ConfigureInitialChart(); // Gọi sau khi đã có teachersList (dù có thể rỗng)
        }

        private async Task OnTeacherSelectedHandler(TeacherDto? selectedTeacher) // Cho phép selectedTeacher là null khi Clear
        {
            if (selectedTeacher != null)
            {
                _selectedTeacherId = selectedTeacher.Id;
                await LoadChartDataForTeacher(_selectedTeacherId);
            }
            else // Xảy ra khi người dùng Clear lựa chọn trong Select (AllowClear="true")
            {
                _selectedTeacherId = 0; // Hoặc giá trị mặc định phù hợp
                chartData.Clear();
                ConfigureChartForSelectedTeacher(); // Cập nhật tiêu đề biểu đồ cho trạng thái "chưa chọn"
                StateHasChanged();
            }
        }

        private async Task LoadChartDataForTeacher(int teacherId)
        {
            if (teacherId <= 0)
            {
                chartData = new List<ChartDataItemDto>();
                chartErrorMessage = string.Empty; // Xóa lỗi cũ nếu có
                ConfigureChartForSelectedTeacher(); // Cập nhật tiêu đề
                StateHasChanged();
                return;
            }

            isLoadingChart = true;
            chartErrorMessage = string.Empty;
            StateHasChanged();

            try
            {
                var request = new GetSchoolStatisticsRequestDto { TeacherId = teacherId };
                var response = await StatisticsClient.GetClassroomStudentCountsForChartAsync(request);

                if (response != null && response.DataPoints != null)
                {
                    chartData = response.DataPoints.ToList();
                    if (!chartData.Any())
                    {
                        // Không ghi đè chartErrorMessage nếu không có dữ liệu, Empty component sẽ xử lý
                        // chartErrorMessage = "Không có dữ liệu lớp học/học sinh cho giáo viên này.";
                    }
                }
                else
                {
                    chartData = new List<ChartDataItemDto>();
                    chartErrorMessage = "Không nhận được dữ liệu thống kê từ server.";
                }
            }
            catch (Exception ex)
            {
                chartErrorMessage = $"Lỗi tải dữ liệu biểu đồ: {ex.Message}";
                chartData = new List<ChartDataItemDto>();
                _logger.LogError(ex, "Lỗi khi tải dữ liệu biểu đồ cho TeacherID: {TeacherId}", teacherId);
            }
            finally
            {
                isLoadingChart = false;
                ConfigureChartForSelectedTeacher(); // Luôn cập nhật cấu hình/tiêu đề biểu đồ
                StateHasChanged();
            }
        }

        private void ConfigureInitialChart() // Đổi tên cho rõ nghĩa hơn là cấu hình chung
        {
            var titleText = "Số lượng Sinh viên theo Lớp học (Chọn giáo viên để xem)";
            if (_selectedTeacherId > 0 && teachersList.Any())
            {
                var selectedTeacherName = teachersList.FirstOrDefault(t => t.Id == _selectedTeacherId)?.Name;
                if (!string.IsNullOrEmpty(selectedTeacherName))
                {
                    titleText = $"Số lượng Sinh viên theo Lớp - GV: {selectedTeacherName}";
                }
                else if (_selectedTeacherId > 0)
                { // TeacherId được chọn nhưng không tìm thấy tên (ít xảy ra nếu teachersList đúng)
                    titleText = $"Số lượng Sinh viên theo Lớp - GV ID: {_selectedTeacherId}";
                }
            }

            barChartConfig = new BarConfig
            {
                Title = new Title
                {
                    Visible = true,
                    Text = titleText
                },
                // Đối với Bar chart (thanh ngang):
                // YField là trục danh mục (Tên Lớp)
                // XField là trục giá trị (Số lượng Sinh viên)
                YField = "xAxisCategory",
                XField = "yAxisValue",
                AutoFit = true, // Nên có để biểu đồ tự điều chỉnh

                Meta = new Dictionary<string, IMeta>
        {
            { "xAxisCategory", new Meta { Alias = "Lớp học" } },
            { "yAxisValue", new Meta { Alias = "Số lượng sinh viên" } }
        },
                Legend = new Legend { Visible = false }, // Thường không cần legend nếu chỉ có 1 series dữ liệu chính

                Label = new BarViewConfigLabel
                {
                    Visible = true,
                    Position = "right",
                },
                Tooltip = new AntDesign.Charts.Tooltip { ShowTitle = true } // Giữ lại ShowTitle
            };
        }

        private void ConfigureChartForSelectedTeacher()
        {
            ConfigureInitialChart();
        }

        [Inject] ILogger<SchoolDashboard> _logger { get; set; } = default!;
    }
}
