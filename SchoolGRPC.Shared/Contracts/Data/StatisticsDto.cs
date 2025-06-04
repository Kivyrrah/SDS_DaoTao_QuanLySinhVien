using System.Runtime.Serialization;
using System.Collections.Generic;

namespace SchoolGRPC.Shared.Contracts.Data
{
    [DataContract]
    public class ChartDataItemDto 
    {
        [DataMember(Order = 1)]
        public string XAxisCategory { get; set; } = string.Empty;

        [DataMember(Order = 2)]
        public double YAxisValue { get; set; }

        [DataMember(Order = 3)]
        public string SeriesGroup { get; set; } = string.Empty;
    }

    [DataContract]
    public class ClassroomStudentCountChartResponseDto
    {
        [DataMember(Order = 1)]
        public List<ChartDataItemDto> DataPoints { get; set; } = new List<ChartDataItemDto>();
    }

    [DataContract]
    public class GetSchoolStatisticsRequestDto 
    {
        [DataMember(Order = 1)]
        public int TeacherId { get; set; }
    }
}