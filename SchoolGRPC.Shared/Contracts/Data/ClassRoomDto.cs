using System.Runtime.Serialization;
using System.Collections.Generic;

namespace SchoolGRPC.Shared.Contracts.Data
{
    [DataContract]
    public class ClassRoomDto
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; } = string.Empty;

        [DataMember(Order = 3)]
        public string Subject { get; set; } = string.Empty;

        [DataMember(Order = 4)]
        public int TeacherId { get; set; } 

        [DataMember(Order = 5)]
        public string TeacherName { get; set; } = string.Empty; 
    }

    [DataContract]
    public class ClassRoomListResponseDto
    {
        [DataMember(Order = 1)]
        public List<ClassRoomDto> Classrooms { get; set; } = new List<ClassRoomDto>();
    }

    [DataContract]
    public class CreateClassRoomRequestDto
    {
        [DataMember(Order = 1)]
        public string Name { get; set; } = string.Empty;
        [DataMember(Order = 2)]
        public string Subject { get; set; } = string.Empty;
        [DataMember(Order = 3)]
        public int TeacherId { get; set; }
    }

    [DataContract]
    public class UpdateClassRoomRequestDto
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }
        [DataMember(Order = 2)]
        public string Name { get; set; } = string.Empty;
        [DataMember(Order = 3)]
        public string Subject { get; set; } = string.Empty;
        [DataMember(Order = 4)]
        public int TeacherId { get; set; }
    }
}