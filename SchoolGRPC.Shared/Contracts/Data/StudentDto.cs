using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace SchoolGRPC.Shared.Contracts.Data
{
    [DataContract]
    public class StudentDto
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; } = string.Empty;

        [DataMember(Order = 3)]
        public DateTime Birthday { get; set; }

        [DataMember(Order = 4)]
        public string Address { get; set; } = string.Empty;

        [DataMember(Order = 5)]
        public int ClassroomId { get; set; }

        [DataMember(Order = 6)]
        public string ClassroomName { get; set; } = string.Empty;

        [DataMember(Order = 7)]
        public string TeacherNameViaClassroom { get; set; } = string.Empty;
    }

    [DataContract]
    public class StudentListResponseDto
    {
        [DataMember(Order = 1)]
        public List<StudentDto> Students { get; set; } = new List<StudentDto>();
    }

    [DataContract]
    public class CreateStudentRequestDto
    {
        [DataMember(Order = 1)]
        public string Name { get; set; } = string.Empty;
        [DataMember(Order = 2)]
        public DateTime Birthday { get; set; }
        [DataMember(Order = 3)]
        public string Address { get; set; } = string.Empty;
        [DataMember(Order = 4)]
        public int ClassroomId { get; set; }
    }

    [DataContract]
    public class UpdateStudentRequestDto
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }
        [DataMember(Order = 2)]
        public string Name { get; set; } = string.Empty;
        [DataMember(Order = 3)]
        public DateTime Birthday { get; set; }
        [DataMember(Order = 4)]
        public string Address { get; set; } = string.Empty;
        [DataMember(Order = 5)]
        public int ClassroomId { get; set; }
    }

    /* [DataContract]
    public class GetStudentsByClassRoomRequestDto
    {
        [DataMember(Order = 1)]
        public int ClassroomId { get; set; } 
    } */
}