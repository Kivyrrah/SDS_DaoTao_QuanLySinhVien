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

        private List<TeacherDto> teachersList = new List<TeacherDto>();
        private int _selectedTeacherId;

        private List<ChartDataItemDto> chartData = new List<ChartDataItemDto>();
        private BarConfig barChartConfig = new BarConfig();

        private bool isLoadingTeachers = true;
        private bool isLoadingChart = false;
        private string pageErrorMessage = string.Empty;
        private string chartErrorMessage = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            isLoadingTeachers = true;
            try
            {
                var response = await TeacherClient.GetAllTeachersAsync(); 
                if (response != null && response.Teachers != null)
                {
                    teachersList = response.Teachers.ToList();
                }
                else
                {
                    teachersList = new List<TeacherDto>(); 
                }
            }
            catch (Exception ex)
            {
                pageErrorMessage = $"Lỗi tải danh sách giáo viên: {ex.Message}";
                teachersList = new List<TeacherDto>(); 
            }
            finally
            {
                isLoadingTeachers = false;
            }
            ConfigureInitialChart(); 
        }

        private async Task OnTeacherSelectedHandler(TeacherDto? selectedTeacher) 
        {
            if (selectedTeacher != null)
            {
                _selectedTeacherId = selectedTeacher.Id;
                await LoadChartDataForTeacher(_selectedTeacherId);
            }
            else 
            {
                _selectedTeacherId = 0; 
                chartData.Clear();
                ConfigureChartForSelectedTeacher(); 
                StateHasChanged();
            }
        }

        private async Task LoadChartDataForTeacher(int teacherId)
        {
            if (teacherId <= 0)
            {
                chartData = new List<ChartDataItemDto>();
                chartErrorMessage = string.Empty; 
                ConfigureChartForSelectedTeacher(); 
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
                ConfigureChartForSelectedTeacher(); 
                StateHasChanged();
            }
        }

        private void ConfigureInitialChart() 
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
                {
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
                YField = "xAxisCategory",
                XField = "yAxisValue",
                AutoFit = true, 

                Meta = new Dictionary<string, IMeta>
                {
                    { "xAxisCategory", new Meta { Alias = "Lớp học" } },
                    { "yAxisValue", new Meta { Alias = "Số lượng sinh viên" } }
                },
                Legend = new Legend { Visible = false }, 

                Label = new BarViewConfigLabel
                {
                    Visible = true,
                    Position = "right",
                },
                Tooltip = new AntDesign.Charts.Tooltip { ShowTitle = true } 
            };
        }

        private void ConfigureChartForSelectedTeacher()
        {
            ConfigureInitialChart();
        }

        [Inject] ILogger<SchoolDashboard> _logger { get; set; } = default!;
    }
}
