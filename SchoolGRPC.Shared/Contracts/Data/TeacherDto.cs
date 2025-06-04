using System;
using System.Runtime.Serialization;

namespace SchoolGRPC.Shared.Contracts.Data 
{
    [DataContract]
    public class TeacherDto
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; } = string.Empty;

        [DataMember(Order = 3)]
        public DateTime Birthday { get; set; } 
    }

    [DataContract]
    public class TeacherListResponseDto
    {
        [DataMember(Order = 1)]
        public List<TeacherDto> Teachers { get; set; } = new List<TeacherDto>();
    }

    [DataContract]
    public class CreateTeacherRequestDto
    {
        [DataMember(Order = 1)]
        public string Name { get; set; } = string.Empty;

        [DataMember(Order = 2)]
        public DateTime Birthday { get; set; }
    }

    [DataContract]
    public class UpdateTeacherRequestDto
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; } = string.Empty;

        [DataMember(Order = 3)]
        public DateTime Birthday { get; set; }
    }
}