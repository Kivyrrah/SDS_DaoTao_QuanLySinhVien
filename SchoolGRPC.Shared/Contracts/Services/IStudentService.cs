using SchoolGRPC.Shared.Contracts.Data;
using System.ServiceModel;
using System.Threading.Tasks;

namespace SchoolGRPC.Shared.Contracts.Services
{
    [ServiceContract(Name = "school.StudentService")]
    public interface IStudentService
    {
        [OperationContract]
        Task<StudentListResponseDto> GetAllStudentsAsync();
        [OperationContract]
        Task<StudentDto?> GetStudentByIdAsync(GetByIdRequestDto request);
        [OperationContract]
        Task<StudentDto> CreateStudentAsync(CreateStudentRequestDto request);
        [OperationContract]
        Task<StudentDto> UpdateStudentAsync(UpdateStudentRequestDto request);
        [OperationContract]
        Task<DeleteResponseDto> DeleteStudentAsync(GetByIdRequestDto request);
        [OperationContract]
        Task<StudentListResponseDto> GetStudentsByClassRoomAsync(GetStudentsByClassRoomRequestDto request);
    }
}