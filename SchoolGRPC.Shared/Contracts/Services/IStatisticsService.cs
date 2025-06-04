using SchoolGRPC.Shared.Contracts.Data;
using System.ServiceModel;
using System.Threading.Tasks;

namespace SchoolGRPC.Shared.Contracts.Services
{
    [ServiceContract(Name = "school.StatisticsService")]
    public interface IStatisticsService
    {
        [OperationContract]
        Task<ClassroomStudentCountChartResponseDto> GetClassroomStudentCountsForChartAsync(GetSchoolStatisticsRequestDto request);
    }
}