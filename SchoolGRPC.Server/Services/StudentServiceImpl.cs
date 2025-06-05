// SchoolGRPC.Server/Services/StudentServiceImpl.cs
using Grpc.Core;
using Microsoft.Extensions.Logging;
using SchoolGRPC.Server.IRepositories;
using SchoolGRPC.Server.Repositories;
using SchoolGRPC.Shared.Contracts.Data;    
using SchoolGRPC.Shared.Contracts.Services; 
using SchoolGRPC.Shared.Models;
using System.Linq;
using System.ServiceModel; 
using System.Threading.Tasks;

namespace SchoolGRPC.Server.Services
{
    public class StudentServiceImpl : IStudentService 
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IClassRoomRepository _classRoomRepository; 
        private readonly ILogger<StudentServiceImpl> _logger;

        public StudentServiceImpl(IStudentRepository studentRepository, IClassRoomRepository classRoomRepository, ILogger<StudentServiceImpl> logger)
        {
            _studentRepository = studentRepository;
            _classRoomRepository = classRoomRepository;
            _logger = logger;
        }

        public async Task<StudentListResponseDto> GetAllStudentsAsync()
        {
            _logger.LogInformation("GetAllStudentsAsync called");
            var students = await _studentRepository.GetAllWithDetailsAsync(); 
            var response = new StudentListResponseDto();
            if (students != null)
            {
                response.Students.AddRange(students.Select(s => s.ToDto()));
            }
            return response;
        }

        public async Task<StudentDto?> GetStudentByIdAsync(GetByIdRequestDto request)
        {
            _logger.LogInformation("GetStudentByIdAsync called with ID: {StudentId}", request.Id);
            var student = await _studentRepository.GetByIdWithDetailsAsync(request.Id); 
            return student?.ToDto();
        }

        public async Task<StudentDto> CreateStudentAsync(CreateStudentRequestDto request)
        {
            _logger.LogInformation("CreateStudentAsync called for Name: {StudentName}, ClassRoomID: {ClassRoomId}", request.Name, request.ClassroomId);
            var classRoom = await _classRoomRepository.GetByIdWithTeacherAsync(request.ClassroomId);
            if (classRoom == null)
            {
                _logger.LogWarning("ClassRoom with ID {ClassRoomId} not found. Cannot create student.", request.ClassroomId);
                throw new FaultException($"ClassRoom with ID {request.ClassroomId} not found. Cannot create student.");
            }

            var student = new Student
            {
                Name = request.Name,
                Birthday = request.Birthday,
                Address = request.Address,
                ClassRoom = classRoom 
            };
            await _studentRepository.AddAsync(student);
            return student.ToDto();
        }

        public async Task<StudentDto> UpdateStudentAsync(UpdateStudentRequestDto request)
        {
            _logger.LogInformation("UpdateStudentAsync called for ID: {StudentId}", request.Id);
            var student = await _studentRepository.GetByIdAsync(request.Id); 
            if (student == null)
            {
                _logger.LogWarning("Student with ID {StudentId} not found for update.", request.Id);
                throw new FaultException($"Student with ID {request.Id} not found for update.");
            }

            var classRoom = await _classRoomRepository.GetByIdWithTeacherAsync(request.ClassroomId);
            if (classRoom == null)
            {
                _logger.LogWarning("ClassRoom with ID {ClassRoomId} not found. Cannot update student.", request.ClassroomId);
                throw new FaultException($"ClassRoom with ID {request.ClassroomId} not found. Cannot update student.");
            }

            student.Name = request.Name;
            student.Birthday = request.Birthday;
            student.Address = request.Address;
            student.ClassRoom = classRoom; 

            await _studentRepository.UpdateAsync(student);
            var updatedStudent = await _studentRepository.GetByIdWithDetailsAsync(student.Id);
            return updatedStudent!.ToDto();
        }

        public async Task<DeleteResponseDto> DeleteStudentAsync(GetByIdRequestDto request)
        {
            _logger.LogInformation("DeleteStudentAsync called for ID: {StudentId}", request.Id);
            var student = await _studentRepository.GetByIdAsync(request.Id);
            if (student == null)
            {
                return new DeleteResponseDto { Success = false, Message = $"Student with ID {request.Id} not found." };
            }
            await _studentRepository.DeleteAsync(request.Id);
            return new DeleteResponseDto { Success = true, Message = "Student deleted successfully." };
        }

        public async Task<StudentListResponseDto> GetStudentsByClassRoomAsync(GetStudentsByClassRoomRequestDto request)
        {
            _logger.LogInformation("GetStudentsByClassRoomAsync called for ClassRoomID: {ClassRoomId}", request.ClassRoomId);
            if (request.ClassRoomId <= 0)
            {
                _logger.LogWarning("Invalid ClassRoomId received: {ClassRoomId}", request.ClassRoomId);
                return new StudentListResponseDto { Students = new List<StudentDto>() };
            }

            var studentsInDomain = await _studentRepository.GetByClassRoomIdAsync(request.ClassRoomId);

            var response = new StudentListResponseDto();
            if (studentsInDomain != null)
            {
                response.Students.AddRange(studentsInDomain.Select(s => s.ToDto()));
            }

            _logger.LogInformation("Found {StudentCount} students for ClassRoomID: {ClassRoomId}", response.Students.Count, request.ClassRoomId);
            return response;
        }
    }
}