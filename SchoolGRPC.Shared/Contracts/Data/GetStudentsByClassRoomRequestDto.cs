using System.Runtime.Serialization;

namespace SchoolGRPC.Shared.Contracts.Data
{
    [DataContract]
    public class GetStudentsByClassRoomRequestDto
    {
        [DataMember(Order = 1)]
        public int ClassRoomId { get; set; }
    }
}