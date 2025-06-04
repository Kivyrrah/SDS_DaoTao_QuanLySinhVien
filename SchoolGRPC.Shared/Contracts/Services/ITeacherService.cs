using SchoolGRPC.Shared.Contracts.Data; 
using System.ServiceModel; 
using System.Threading.Tasks;

namespace SchoolGRPC.Shared.Contracts.Services
{
    [ServiceContract(Name = "school.TeacherService")] 
    public interface ITeacherService
    {
        [OperationContract]
        Task<TeacherListResponseDto> GetAllTeachersAsync(); 

        [OperationContract]
        Task<TeacherDto?> GetTeacherByIdAsync(GetByIdRequestDto request); 

        [OperationContract]
        Task<TeacherDto> CreateTeacherAsync(CreateTeacherRequestDto request);

        [OperationContract]
        Task<TeacherDto> UpdateTeacherAsync(UpdateTeacherRequestDto request);

        [OperationContract]
        Task<DeleteResponseDto> DeleteTeacherAsync(GetByIdRequestDto request); 
    }
}