using SchoolGRPC.Shared.Models; 
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolGRPC.Server.IRepositories
{
    public interface IStatisticsRepository
    {
        Task<IEnumerable<TeacherClassRoomStudentCountPoco>> GetStudentCountsPerClassRoomPerTeacherAsync(int? teacherId = null);
    }
}