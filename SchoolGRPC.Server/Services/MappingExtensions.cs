using SchoolGRPC.Shared.Models; 
using SchoolGRPC.Shared.Contracts.Data; 
using SchoolGRPC.Shared.Models;
using System;
using System.Linq;

namespace SchoolGRPC.Server.Services 
{
    public static class MappingExtensions
    {
        public static TeacherDto ToDto(this Teacher teacher)
        {
            if (teacher == null) return null!; 
            return new TeacherDto
            {
                Id = teacher.Id,
                Name = teacher.Name ?? string.Empty,
                Birthday = teacher.Birthday 
            };
        }

        public static Teacher ToDomain(this CreateTeacherRequestDto dto)
        {
            return new Teacher
            {
                Name = dto.Name,
                Birthday = dto.Birthday
            };
        }

        public static void UpdateDomain(this Teacher teacher, UpdateTeacherRequestDto dto)
        {
            teacher.Name = dto.Name;
            teacher.Birthday = dto.Birthday;
        }

        public static ClassRoomDto ToDto(this ClassRoom classRoom)
        {
            if (classRoom == null) return null!;
            return new ClassRoomDto
            {
                Id = classRoom.Id,
                Name = classRoom.Name ?? string.Empty,
                Subject = classRoom.Subject ?? string.Empty,
                TeacherId = classRoom.Teacher?.Id ?? 0, 
                TeacherName = classRoom.Teacher?.Name ?? "N/A"
            };
        }

        public static StudentDto ToDto(this Student student)
        {
            if (student == null) return null!;
            return new StudentDto
            {
                Id = student.Id,
                Name = student.Name ?? string.Empty,
                Birthday = student.Birthday,
                Address = student.Address ?? string.Empty,
                ClassroomId = student.ClassRoom?.Id ?? 0,
                ClassroomName = student.ClassRoom?.Name ?? "N/A",
                TeacherNameViaClassroom = student.ClassRoom?.Teacher?.Name ?? "N/A"
            };
        }

        public static ChartDataItemDto ToDto(this TeacherClassRoomStudentCountPoco poco)
        {
            return new ChartDataItemDto
            {
                XAxisCategory = poco.ClassRoomName,
                YAxisValue = poco.StudentCount,
                SeriesGroup = poco.TeacherName
            };
        }
    }
}