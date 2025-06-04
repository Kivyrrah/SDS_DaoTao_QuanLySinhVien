using System.Runtime.Serialization;

namespace SchoolGRPC.Shared.Contracts.Data
{
    [DataContract]
    public class GetByIdRequestDto
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }
    }

    [DataContract]
    public class DeleteResponseDto
    {
        [DataMember(Order = 1)]
        public bool Success { get; set; }

        [DataMember(Order = 2)]
        public string Message { get; set; } = string.Empty;
    }
}