// SchoolGRPC.Server/Services/TeacherServiceImpl.cs
using Microsoft.Extensions.Logging; 
using SchoolGRPC.Shared.Models; 
using SchoolGRPC.Server.Repositories;
using SchoolGRPC.Server.IRepositories;
using SchoolGRPC.Shared.Contracts.Data;    
using SchoolGRPC.Shared.Contracts.Services; 
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGRPC.Server.Services
{
    public class TeacherServiceImpl : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly ILogger<TeacherServiceImpl> _logger;

        public TeacherServiceImpl(ITeacherRepository teacherRepository, ILogger<TeacherServiceImpl> logger)
        {
            _teacherRepository = teacherRepository;
            _logger = logger;
        }

        public async Task<TeacherListResponseDto> GetAllTeachersAsync()
        {
            _logger.LogInformation("GetAllTeachersAsync called");
            var teachers = await _teacherRepository.GetAllAsync();
            var response = new TeacherListResponseDto();
            if (teachers != null)
            {
                response.Teachers.AddRange(teachers.Select(t => t.ToDto()));
            }
            return response;
        }

        public async Task<TeacherDto?> GetTeacherByIdAsync(GetByIdRequestDto request)
        {
            _logger.LogInformation("GetTeacherByIdAsync called with ID: {TeacherId}", request.Id);
            var teacher = await _teacherRepository.GetByIdAsync(request.Id);
            return teacher?.ToDto();
        }

        public async Task<TeacherDto> CreateTeacherAsync(CreateTeacherRequestDto request)
        {
            _logger.LogInformation("CreateTeacherAsync called with Name: {TeacherName}", request.Name);
            var teacher = new Teacher 
            {
                Name = request.Name,
                Birthday = request.Birthday
            };
            await _teacherRepository.AddAsync(teacher);
            return teacher.ToDto(); 
        }

        public async Task<TeacherDto> UpdateTeacherAsync(UpdateTeacherRequestDto request)
        {
            _logger.LogInformation("UpdateTeacherAsync called for ID: {TeacherId}", request.Id);
            var teacher = await _teacherRepository.GetByIdAsync(request.Id);
            if (teacher == null)
            {
                _logger.LogWarning("Teacher with ID {TeacherId} not found for update.", request.Id);
                throw new System.ServiceModel.FaultException($"Teacher with ID {request.Id} not found."); 
            }

            teacher.Name = request.Name;
            teacher.Birthday = request.Birthday;

            await _teacherRepository.UpdateAsync(teacher);
            return teacher.ToDto();
        }

        public async Task<DeleteResponseDto> DeleteTeacherAsync(GetByIdRequestDto request)
        {
            _logger.LogInformation("DeleteTeacherAsync called for ID: {TeacherId}", request.Id);
            var teacher = await _teacherRepository.GetByIdAsync(request.Id);
            if (teacher == null)
            {
                return new DeleteResponseDto { Success = false, Message = $"Teacher with ID {request.Id} not found." };
            }
            await _teacherRepository.DeleteAsync(request.Id);
            return new DeleteResponseDto { Success = true, Message = "Teacher deleted successfully." };
        }
    }
}