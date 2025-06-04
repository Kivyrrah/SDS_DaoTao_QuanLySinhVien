// SchoolGRPC.Server/Services/ClassRoomServiceImpl.cs
using Microsoft.Extensions.Logging;
using SchoolGRPC.Shared.Models;
using SchoolGRPC.Server.Repositories;
using SchoolGRPC.Server.IRepositories;
using SchoolGRPC.Shared.Contracts.Data;    
using SchoolGRPC.Shared.Contracts.Services; 
using System.Linq;
using System.ServiceModel; 
using System.Threading.Tasks;

namespace SchoolGRPC.Server.Services
{
    public class ClassRoomServiceImpl : IClassRoomService 
    {
        private readonly IClassRoomRepository _classRoomRepository;
        private readonly ITeacherRepository _teacherRepository; 
        private readonly ILogger<ClassRoomServiceImpl> _logger;

        public ClassRoomServiceImpl(IClassRoomRepository classRoomRepository, ITeacherRepository teacherRepository, ILogger<ClassRoomServiceImpl> logger)
        {
            _classRoomRepository = classRoomRepository;
            _teacherRepository = teacherRepository;
            _logger = logger;
        }

        public async Task<ClassRoomListResponseDto> GetAllClassRoomsAsync()
        {
            _logger.LogInformation("GetAllClassRoomsAsync called");
            var classRooms = await _classRoomRepository.GetAllWithTeachersAsync(); 
            var response = new ClassRoomListResponseDto();
            if (classRooms != null)
            {
                response.Classrooms.AddRange(classRooms.Select(cr => cr.ToDto()));
            }
            return response;
        }

        public async Task<ClassRoomDto?> GetClassRoomByIdAsync(GetByIdRequestDto request)
        {
            _logger.LogInformation("GetClassRoomByIdAsync called with ID: {ClassRoomId}", request.Id);
            var classRoom = await _classRoomRepository.GetByIdWithTeacherAsync(request.Id); 
            return classRoom?.ToDto();
        }

        public async Task<ClassRoomDto> CreateClassRoomAsync(CreateClassRoomRequestDto request)
        {
            _logger.LogInformation("CreateClassRoomAsync called for Name: {ClassRoomName}, TeacherID: {TeacherId}", request.Name, request.TeacherId);
            var teacher = await _teacherRepository.GetByIdAsync(request.TeacherId);
            if (teacher == null)
            {
                _logger.LogWarning("Teacher with ID {TeacherId} not found. Cannot create classroom.", request.TeacherId);
                throw new FaultException($"Teacher with ID {request.TeacherId} not found. Cannot create classroom.");
            }

            var classRoom = new ClassRoom
            {
                Name = request.Name,
                Subject = request.Subject,
                Teacher = teacher 
            };
            await _classRoomRepository.AddAsync(classRoom);
            return classRoom.ToDto();
        }

        public async Task<ClassRoomDto> UpdateClassRoomAsync(UpdateClassRoomRequestDto request)
        {
            _logger.LogInformation("UpdateClassRoomAsync called for ID: {ClassRoomId}", request.Id);
            var classRoom = await _classRoomRepository.GetByIdAsync(request.Id); 
            if (classRoom == null)
            {
                _logger.LogWarning("ClassRoom with ID {ClassRoomId} not found for update.", request.Id);
                throw new FaultException($"ClassRoom with ID {request.Id} not found for update.");
            }

            var teacher = await _teacherRepository.GetByIdAsync(request.TeacherId);
            if (teacher == null)
            {
                _logger.LogWarning("Teacher with ID {TeacherId} not found. Cannot update classroom.", request.TeacherId);
                throw new FaultException($"Teacher with ID {request.TeacherId} not found. Cannot update classroom.");
            }

            classRoom.Name = request.Name;
            classRoom.Subject = request.Subject;
            classRoom.Teacher = teacher; 

            await _classRoomRepository.UpdateAsync(classRoom);
            var updatedClassRoom = await _classRoomRepository.GetByIdWithTeacherAsync(classRoom.Id);
            return updatedClassRoom!.ToDto(); 
        }

        public async Task<DeleteResponseDto> DeleteClassRoomAsync(GetByIdRequestDto request)
        {
            _logger.LogInformation("DeleteClassRoomAsync called for ID: {ClassRoomId}", request.Id);
            var classRoom = await _classRoomRepository.GetByIdAsync(request.Id);
            if (classRoom == null)
            {
                return new DeleteResponseDto { Success = false, Message = $"ClassRoom with ID {request.Id} not found." };
            }
            await _classRoomRepository.DeleteAsync(request.Id);
            return new DeleteResponseDto { Success = true, Message = "ClassRoom deleted successfully." };
        }
    }
}