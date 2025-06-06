// SchoolGRPC.Server/Services/StatisticsServiceImpl.cs
using Microsoft.Extensions.Logging;
using SchoolGRPC.Server.IRepositories;
using SchoolGRPC.Server.Repositories;
using SchoolGRPC.Shared.Contracts.Data;    
using SchoolGRPC.Shared.Contracts.Services; 
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGRPC.Server.Services
{
    public class StatisticsServiceImpl : IStatisticsService
    {
        private readonly IStatisticsRepository _statisticsRepository;
        private readonly ILogger<StatisticsServiceImpl> _logger;

        public StatisticsServiceImpl(IStatisticsRepository statisticsRepository, ILogger<StatisticsServiceImpl> logger)
        {
            _statisticsRepository = statisticsRepository;
            _logger = logger;
        }

        public async Task<ClassroomStudentCountChartResponseDto> GetClassroomStudentCountsForChartAsync(GetSchoolStatisticsRequestDto request)
        {
            _logger.LogInformation("GetClassroomStudentCountsForChartAsync called. TeacherID: {TeacherId}", request.TeacherId);

            var statsData = await _statisticsRepository.GetStudentCountsPerClassRoomPerTeacherAsync(
                request.TeacherId > 0 ? (int?)request.TeacherId : null
            );

            var response = new ClassroomStudentCountChartResponseDto();
            if (statsData != null)
            {
                response.DataPoints.AddRange(statsData.Select(poco => poco.ToDto())); 
            }
            return response;
        }
    }
}