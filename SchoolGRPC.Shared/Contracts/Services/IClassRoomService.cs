using SchoolGRPC.Shared.Contracts.Data;
using System.ServiceModel;
using System.Threading.Tasks;

namespace SchoolGRPC.Shared.Contracts.Services
{
    [ServiceContract(Name = "school.ClassRoomService")]
    public interface IClassRoomService
    {
        [OperationContract]
        Task<ClassRoomListResponseDto> GetAllClassRoomsAsync();
        [OperationContract]
        Task<ClassRoomDto?> GetClassRoomByIdAsync(GetByIdRequestDto request);
        [OperationContract]
        Task<ClassRoomDto> CreateClassRoomAsync(CreateClassRoomRequestDto request);
        [OperationContract]
        Task<ClassRoomDto> UpdateClassRoomAsync(UpdateClassRoomRequestDto request);
        [OperationContract]
        Task<DeleteResponseDto> DeleteClassRoomAsync(GetByIdRequestDto request);
    }
}