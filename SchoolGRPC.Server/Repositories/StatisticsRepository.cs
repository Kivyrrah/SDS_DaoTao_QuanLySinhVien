using NHibernate;
using NHibernate.Transform; 
using SchoolGRPC.Server.Data;
using SchoolGRPC.Shared.Models; 
using SchoolGRPC.Server.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolGRPC.Server.Repositories
{
    public class StatisticsRepository : IStatisticsRepository
    {
        public async Task<IEnumerable<TeacherClassRoomStudentCountPoco>> GetStudentCountsPerClassRoomPerTeacherAsync(int? teacherId = null)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                string sqlBaseQuery = @"
                    SELECT
                        T.Name AS TeacherName,
                        CR.Name AS ClassRoomName,
                        CR.ID AS ClassRoomID,
                        COUNT(S.ID) AS StudentCount
                    FROM Teacher T
                    INNER JOIN ClassRoom CR ON T.ID = CR.TeacherID
                    LEFT JOIN Student S ON CR.ID = S.ClassRoomID";

                string groupByAndOrder = @"
                    GROUP BY T.Name, T.ID, CR.Name, CR.ID
                    ORDER BY T.Name, CR.Name;"; 

                string whereClause = "";
                if (teacherId.HasValue && teacherId.Value > 0)
                {
                    whereClause = " WHERE T.ID = :teacherIdParam ";
                }

                string finalQuery = sqlBaseQuery + whereClause + groupByAndOrder;

                var query = session.CreateSQLQuery(finalQuery)
                                   .AddScalar("TeacherName", NHibernateUtil.String)
                                   .AddScalar("ClassRoomName", NHibernateUtil.String)
                                   .AddScalar("ClassRoomID", NHibernateUtil.Int32)
                                   .AddScalar("StudentCount", NHibernateUtil.Int32);

                if (teacherId.HasValue && teacherId.Value > 0)
                {
                    query.SetParameter("teacherIdParam", teacherId.Value);
                }

                query.SetResultTransformer(Transformers.AliasToBean<TeacherClassRoomStudentCountPoco>());
                return await query.ListAsync<TeacherClassRoomStudentCountPoco>();
            }
        }
    }
}